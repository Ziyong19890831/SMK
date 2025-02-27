using System;

namespace SMK.Worker.FileProcess
{
    public class FileColumn
    {
        public string Name { get; set; }
        public string Example { get; set; }
        public int Length { get; set; }
        public string ColumnName { get; set; }

        public FileColumn(string name, string example, int length)
        {
            Name = name;
            ColumnName = name;
            Example = example;
            Length = Length;
        }
        public FileColumn(string name, int length)
        {
            Name = name;
            ColumnName = name;
            Length = Length;
        }
    }
}