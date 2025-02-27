using SMK.Data;
using SMK.Worker.Services;

namespace SMK.Worker.FileProcess
{
    public interface IFileInProcessor
    {
        void Start();
        bool IsMatched();
        string FileName { get; set; }
    }
}