using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMK.OutPutTxt.Main
{
    public class Check_Files
    {
        public void Check_Files_Route(string path, string fileName)
        {
            if (Directory.Exists(path))
            {
                string filePath = Path.Combine(path, fileName);
                // 資料夾已存在，可以進行相關操作
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            else
            {
                // 資料夾不存在，可以使用 Directory.CreateDirectory() 方法進行新增
                Directory.CreateDirectory(path);
            }
        }
    }
}
