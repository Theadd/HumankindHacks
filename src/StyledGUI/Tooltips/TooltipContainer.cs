using AnN3x.StyledGUI.Collections;
using UnityEngine;

namespace AnN3x.StyledGUI.Tooltips
{
    public class TooltipContainer : MetaContainer
    {
        public static TooltipContainer Empty = new TooltipContainer(StringHandle.Empty);
        
        public float DelayIn { get; set; } = 1f;
        public float DelayOut { get; set; } = 1f;
        public TextAnchor Anchor { get; set; } = TextAnchor.MiddleCenter;
        public Vector2 Size { get; set; } = Vector2.zero;
        
        public TooltipContainer(StringHandle value) : base(value) { }
    }
}
