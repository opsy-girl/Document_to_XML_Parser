using System;
using System.Collections.Generic;
using System.Text;

namespace XParser
{
    public class ContentSection
        //Sc −→ cintro ·clit ·cmethod ·(ccontr |λ)·(cplan|λ)·cconcl
    {
        public ChapterOne chapterOne { get; set; }
        public ChapterTwo chapterTwo { get; set; }
        public ChapterThree chapterThree { get; set; }
        public ChapterFour chapterFour { get; set; }
        public ChapterFive chapterFive { get; set; }
        public ChapterSix chapterSix { get; set; }
        public ChapterSeven chapterSeven { get; set; }
        public ChapterEight chapterEight { get; set; }
        public ChapterNine chapterNine { get; set; }
        public ChapterTen chapterTen { get; set; }

    }
}
