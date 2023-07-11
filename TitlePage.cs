using System;
using System.Collections.Generic;
using System.Text;

namespace XParser
{
    public class TitlePage
    //tp = title,author,supervisor{1 or multiple},titleParagragh1,titleParagraph2,date
    //titleParagragh{1,2} is explicit in this class to avoid another class abstraction
    //We can make the other property class null if not needed in the instance of any proposal document

    {
        public string title { get; set; }
        public string author { get; set; }
        public List<string> supervisors { get; set; }
        public string titleParagragh1 { get; set; }

        public string titleParagraph2 { get; set; }
        public string submissionDate { get; set; }

    }
}
