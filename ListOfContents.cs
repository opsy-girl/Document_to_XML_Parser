using System;
using System.Collections.Generic;
using System.Text;

namespace XParser
{
    public class ListOfContents
    //cp = sectionTitle,table,pageRange
    //Note that list of contents can be more than a page, hence the use of page range to tell the number of pages
    {
        public string sectionTitle { get; set; }
        public List<string> table { get; set; } //We will read every table figure for now as a string, maybe the table title only for now
        public string pageRange { get; set; }

    }
}
