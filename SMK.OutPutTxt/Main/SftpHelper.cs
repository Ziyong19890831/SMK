using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMK.OutPutTxt.Main
{
    public class SftpHelper
    {
        public void UploadFile(string host, int port, string username, string password, string remotePath, string localPath)
        {
            using (var sftp = new SftpClient(host, port, username, password))
            {
                sftp.Connect();
                sftp.ChangeDirectory(remotePath);

                using (var fileStream = new FileStream(localPath, FileMode.Open))
                {
                    var fileName = Path.GetFileName(localPath);
                    sftp.UploadFile(fileStream, fileName);
                }

                sftp.Disconnect();
            }
        }
    }

}
