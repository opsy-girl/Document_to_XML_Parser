using System;
using System.Collections.Generic;
using System.Text;

namespace XParser
{
    public class References
    // pr = sectionTitle, listOfReferences, pageRange
    //Note that References can be more than a page, hence the use of page range to tell the number of pages

    {
        public string sectionTitle { get; set; }
        public List<string> listOfReferences { get; set; }
        public string pageRange { get; set; }

    }
}
