using System;
using System.Collections.Generic;
using System.Text;

namespace XParser
{
    public class ListOfTables
    //lt = sectionTitle, list|table, pageRange 
    //Note that list of tables can be more than a page, hence the use of page range to tell the number of pages
    {
        public string sectionTitle { get; set; }
        public List<string> table { get; set; } //Table or list can be assigned to null if and when not needed
        public string pageRange { get; set; }

    }
}
