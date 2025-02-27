using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Web.Extensions;
using SMK.Data.Utility;
using SMK.Worker.FileProcess;
using SMK.Worker.FileProcess.Handler;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Worker.Services
{
    [SingletonService]
    public class IniDrOrdService
    {
        private SMKWEBContext Context { get; set; }

        public IniDrOrdService(SMKWEBContext context)
        {
            Context = context;
        }

        public void DeleteIniDrOrd(string startDate, string endDate)
        {
            Context.IniDrOrd
                .Where(e => e.TranDate.CompareTo(startDate) >= 0 && e.TranDate.CompareTo(endDate) <= 0)
                .BatchDelete();
        }
    }
}
