using System;

namespace SMK.Data.Attributes
{
    public class FilenamePatternAttribute : Attribute
    {
        public string FilenamePattern { get; set; }
        public FilenamePatternAttribute(string filenamePattern)
        {
            FilenamePattern = filenamePattern;
        }
    }
}
