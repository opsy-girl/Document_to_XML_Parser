using System;
using System.Collections.Generic;
using System.Text;

namespace XParser
{
    public class DeclarationPage
    {
        // dp -> sectionTitle, paragraph{1 or multiple}, page number
        //Note that declaration page cannot be more than a page, hence the name is referenced as page
        public string sectionTitle { get; set; }
        public List<Paragraph> paragraph { get; set; }
        public int pageNumber { get; set; }
       

    }
}
