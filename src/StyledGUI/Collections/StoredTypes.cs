﻿using System;

namespace AnN3x.StyledGUI.Collections
{
    public interface IStoredType
    {
        StringHandle Name { get; }
    }

    public interface ITextContent : IStoredType
    {
        string Title { get; }
        string Description { get; }
    }
    
    [Serializable]
    public class TextContent : ITextContent
    {
        public StringHandle Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
