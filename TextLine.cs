using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XParser
{
    public class TextLine
    {
        // key properties
        // public string LineId { get; private set; }
        public string Section { get; private set; }
        public int Page { get; private set; }
        //public int Line { get; private set; }
        public string LineText { get; private set; }
        public string FontType { get; private set; }
        public float FontSize { get; private set; }
        public float FontLeft { get; private set; }
        public float FontRight { get; private set; }
        public float FontTop { get; private set; }
        public float FontBottom { get; private set; }
        public float LineTopSpacing { get; private set; }
        public float LineBottomSpacing { get; private set; }
        public string LineAlignType { get; private set; }


        public TextLine(string lineId, string section, Int32 page, Int32 line, string lineText, string fontType, float fontSize,
            float fontLeft, float fontRight, float fontTop, float fontBottom,
           float lineTopSpacing, float lineBottomSpacing, string lineAlignType)
        {
            // LineId = lineId;
            Section = section;
            Page = page;
            //Line = line;
            LineText = lineText;
            FontType = fontType;
            FontSize = fontSize;
            FontLeft = fontLeft;
            FontRight = fontRight;
            FontTop = fontTop;
            FontBottom = fontBottom;
            LineTopSpacing = lineTopSpacing;
            LineBottomSpacing = lineBottomSpacing;
            LineAlignType = lineAlignType;


        }
    }
}
