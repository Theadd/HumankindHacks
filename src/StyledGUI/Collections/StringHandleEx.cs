using UnityEngine;

namespace AnN3x.StyledGUI.Collections;

public static class StringHandleEx
{
    public static GUIContent ToGUIContent(this StringHandle self, string title = null) =>
        new GUIContent(title ?? self.GetLocalizedTitle(), self.ToIdentityString());

    public static string GetLocalizedTitle(this StringHandle self) =>
        Storage.Get<ITextContent>(self)?.Title ?? self.ToString();
        
    public static string GetLocalizedDescription(this StringHandle self) =>
        Storage.Get<ITextContent>(self)?.Description ?? self.ToString();
}
