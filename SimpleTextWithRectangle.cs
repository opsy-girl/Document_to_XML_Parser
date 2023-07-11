using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;

namespace XParser
{
    class SimpleTextWithRectangle
    {

        private Rectangle rectangle;
        private String text;

        public SimpleTextWithRectangle(Rectangle rectangle, String text)
        {
            this.rectangle = rectangle;
            this.text = text;
        }

        public Rectangle getRectangle()
        {
            return rectangle;
        }
    }
}
