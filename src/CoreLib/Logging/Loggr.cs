#if !BEPINEX
#if !NOLOGGR
#define USE_DEFAULT
#endif
#define NOLOGGR
#endif
using System;
using System.Runtime.CompilerServices;
#if !NOLOGGR
using AnN3x.CoreLib.Logging;
using BepInEx;
using System.Linq;
using System.IO;
using System.Reflection;
using BepInEx.Logging;
#endif

namespace AnN3x.CoreLib
{
    /// <summary>
    ///     A static logger with color support, only for printing to BepInEx's console window.
    /// </summary>
    public class Loggr
    {
        public static bool Enabled { get; set; } = true;
        public static bool WriteLogToDisk { get; set; } = false;

#if NOLOGGR
        
#if USE_DEFAULT
        public static void Log(string message, ConsoleColor defaultColor, bool appendNewLine = true) => Log(message);

        public static void Log(Exception ex,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string caller = null) => Log(ex.ToString());

        public static void Log(string message) => Console.WriteLine(message);

        public static void Log(object obj) => Log(obj.ToString());

        public static void Log(object obj, ConsoleColor defaultColor) => Log(obj.ToString());

        public static void LogAll(object obj) => Log(obj.ToString());

        public static void LogAll(object obj, ConsoleColor defaultColor) => Log(obj.ToString());

        public static void Debug(string message,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string caller = null) => Log(message);
#else
        public static void Log(string message, ConsoleColor defaultColor, bool appendNewLine = true)
        {
        }

        public static void Log(Exception ex,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string caller = null)
        {
        }

        public static void Log(string message)
        {
        }

        public static void Log(object obj)
        {
        }

        public static void Log(object obj, ConsoleColor defaultColor)
        {
        }

        public static void LogAll(object obj)
        {
        }

        public static void LogAll(object obj, ConsoleColor defaultColor)
        {
        }

        public static void Debug(string message,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string caller = null)
        {
        }
#endif
#else
        private static bool _initialized;
        private static PropertyInfo _consoleStream;
        private static MethodInfo _setConsoleColor;
        private static Type _consoleManager;

        /// <summary>
        ///     Prints a <c>message</c> to BepInEx's console with <c>defaultColor</c> as default text color. Use `%Color%` inline to change text color, where `Color` is any value within <c>ConsoleColor</c> enum.
        /// Available colors are: Default, Black, DarkBlue, DarkGreen, DarkCyan, DarkRed, DarkMagenta, DarkYellow, Gray, DarkGray, Blue, Green, Cyan, Red, Magenta, Yellow, White.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="defaultColor"></param>
        /// <param name="appendNewLine"></param>
        public static void Log(string message, ConsoleColor defaultColor, bool appendNewLine = true)
        {
            if (!Enabled)
                return;

            _LogEx(message, defaultColor, appendNewLine);
        }
        
        public static void Log(Exception ex,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string caller = null)
        {
            _LogEx(ex.ToString(), ConsoleColor.Red, true);
            _LogEx("[IN " + caller + " @ " + lineNumber + "]", ConsoleColor.Red, true);
        }

        public static void Log(string message) => Log(message, ConsoleColor.White);

        public static void Log(object obj) => Log(obj, ConsoleColor.White);

        public static void Log(object obj, ConsoleColor defaultColor)
        {
            if (!Enabled)
                return;

            _LogEx((new PrintableObject(obj)).ToString(), defaultColor, true);
        }

        public static void LogAll(object obj) => LogAll(obj, ConsoleColor.White);

        public static void LogAll(object obj, ConsoleColor defaultColor)
        {
            if (!Enabled)
                return;

            _LogEx((new PrintableObject(obj)
            {
                NonPublicFields = true,
                NonPublicProperties = true,
                Methods = true
            }).ToString(), defaultColor, true);
        }

        public static void Debug(string message,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string caller = null)
        {
            var now = DateTime.Now;
            _Log($"[{caller}:{lineNumber} @ {now.Second}.{now.Millisecond}s] ", ConsoleColor.Gray, false);
            _Log(message, ConsoleColor.Yellow);
        }

        private static void _Log(string message, ConsoleColor color, bool appendNewLine = true)
        {
            if (!_initialized)
                Initialize();

            _setConsoleColor.Invoke(null, new object[] { color });
            ((TextWriter)_consoleStream.GetValue(_consoleManager, null)).Write(appendNewLine ? message + Environment.NewLine : message);
            _setConsoleColor.Invoke(null, new object[] { ConsoleColor.Gray });
        }

        private static void Initialize()
        {
            var bepInExAss = Assembly.GetAssembly(typeof(BaseUnityPlugin));
            var type = bepInExAss.GetType("BepInEx.ConsoleManager");

            _consoleStream = type.GetProperty("ConsoleStream", BindingFlags.Static | BindingFlags.Public);
            _setConsoleColor = type.GetMethod("SetConsoleColor", BindingFlags.Static | BindingFlags.Public);
            _consoleManager = type;

            _initialized = true;
        }

        private static void _LogEx(string message, ConsoleColor defaultColor, bool appendNewLine = true)
        {
            if (!_initialized)
                Initialize();

            TextWriter writer = (TextWriter)_consoleStream.GetValue(_consoleManager, null);
            ConsoleColor color;
            bool lastWasMatch = true;
            bool match = false;
            int ignore;

            _setConsoleColor.Invoke(null, new object[] { defaultColor });

            string[] parts = message.Split('%');

            foreach (var text in parts)
            {
                match = false;

                if (text.Length < 12 && !int.TryParse(text, out ignore))
                {
                    if (ConsoleColor.TryParse(text, true, out color))
                    {
                        match = true;
                        _setConsoleColor.Invoke(null, new object[] { color });
                    }
                    else if (text.ToUpper() == "DEFAULT")
                    {
                        match = true;
                        _setConsoleColor.Invoke(null, new object[] { defaultColor });
                    }
                }

                if (!match)
                {
                    if (!lastWasMatch)
                    {
                        writer.Write("%" + text);
                        if (WriteLogToDisk)
                            WriteToDisk("%" + text);
                    }
                    else
                    {
                        writer.Write(text);
                        if (WriteLogToDisk)
                            WriteToDisk(text);
                    }
                }

                lastWasMatch = match;
            }

            if (appendNewLine)
            {
                writer.Write(Environment.NewLine);
                if (WriteLogToDisk)
                    WriteToDisk(Environment.NewLine);
            }

            _setConsoleColor.Invoke(null, new object[] { ConsoleColor.Gray });
        }

        private static void WriteToDisk(string message)
        {
            if (Logger.Listeners.FirstOrDefault(l =>
                    l is DiskLogListener) is
                DiskLogListener diskLogger)
            {
                diskLogger.LogWriter.Write(message);
            }
        }
#endif
    }
}
