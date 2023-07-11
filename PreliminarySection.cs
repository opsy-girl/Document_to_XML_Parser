using System;
using System.Collections.Generic;
using System.Text;

namespace XParser
{
    public class PreliminarySection
     //Sp −→ tp(dp|λ)(ap|λ)cp(lf |λ)(lt|λ)abp
    {
        public TitlePage titlePage { get; set; }
        public DeclarationPage declarationPage { get; set; }
        public AcknowledgementPage acknowledgementPage { get; set; }
        public ListOfContents listOfContentsPage { get; set; }
        public ListOfFigures listOfFiguresPage { get; set; }
        public ListOfTables listOfTablesPage { get; set; }
        public AbstractPage abstractPage { get; set; }

    }
}
