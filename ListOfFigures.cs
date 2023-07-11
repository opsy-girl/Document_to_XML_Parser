using System;
using System.Collections.Generic;
using System.Text;

namespace XParser
{
    public class ListOfFigures

    //lf = sectionTitle, list|table, pageRange 
    //Note that list of figures can be more than a page, hence the use of page range to tell the number of pages
    {
        public string sectionTitle { get; set; }
        public List<string> figures { get; set; } //We will read every table figure for now as a string, maybe the table title only for now
        public string pageRange { get; set; }

    }

}
