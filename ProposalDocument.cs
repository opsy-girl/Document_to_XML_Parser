using System;
using System.Collections.Generic;
using System.Text;

namespace XParser

{
    public class ProposalDocument
    {
        //S_doc −→ S_p, S_c, S_a, S_r
        public PreliminarySection preliminarySection { get; set; }
        public ContentSection contentSection { get; set; }
        public AppendixSection appendixSection { get; set; }
        public ReferenceSection referenceSection { get; set; }

    }
}
