using System;
using System.Collections.Generic;
using System.Text;

namespace XParser
{
    public class Appendices

    // pa = sectionTitle, listOfFigures, pageRange
    //Note that Appendices can be more than a page, hence the use of page range to tell the number of pages

    {
        public string sectionTitle { get; set; }
        public List<string> listOfFigures { get; set; }
        public string pageRange { get; set; }

    }
}

