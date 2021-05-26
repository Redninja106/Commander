using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Commander
{
    public struct OutputStyle
    {
        public static readonly OutputStyle Default = new OutputStyle(Color.Green, Color.White, Color.DarkGray, Color.Yellow, Color.Red);

        public OutputStyle(Color textColor, Color highlightColor, Color lowlightColor, Color warningColor, Color errorColor)
        {
            Text = textColor;
            Highlight = highlightColor;
            Lowlight = lowlightColor;
            WarningColor = warningColor;
            Error = errorColor;
        }

        public Color Text { get; set; }     

        public Color Highlight { get; set; }

        public Color Lowlight { get; set; } 

        public Color WarningColor { get; set; }  

        public Color Error { get; set; }    
    }
}
