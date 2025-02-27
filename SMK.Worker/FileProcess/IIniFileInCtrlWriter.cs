using SMK.Data.Enums;

namespace SMK.Worker.FileProcess
{
    public interface IIniFileInCtrlWriter
    {
        void Write(int id, FileInStatus status);
        int Initialize(string filename);
    }
}