using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using SMK.Data.Entity;
using System.Collections.Generic;

namespace SMK.Web.Models
{
    public class CallBoardViewModel : CallBoard
    {
        public List<(string Path, string FileName)> Files { get; set;}
        public string StartDate { get; set; }
    }
}
