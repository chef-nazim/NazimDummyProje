using System;

namespace NCG.template.utility.camera
{
    [Serializable]
    public class Padding
    {
        public float Top;
        public float Bottom;
        public float Left;
        public float Right;
        
        public Padding(float top, float bottom, float left, float right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }
    }
}