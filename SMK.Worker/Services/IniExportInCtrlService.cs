using System;
using System.Linq;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Enums;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Worker.Services
{
    [SingletonService]
    public class IniExportInCtrlService
    {
        public IniExportInCtrlService(SMKWEBContext context)
        {
            Context = context;
        }

        public SMKWEBContext Context { get; set; }

        public int Initialize(string fee_ym)
        {
            var single = Context.IniExportInCtrl.Where(x => x.fee_ym == fee_ym).FirstOrDefault();
            if (single == null)
            {
                var iniExportInCtl = new IniExportInCtrl()
                {
                    fee_ym = fee_ym,
                    StartedAt = DateTime.Now,
                };
                Context.IniExportInCtrl.Add(iniExportInCtl);
                Context.SaveChanges();
                return iniExportInCtl.Id;
            }
            else
            {
                single.StartedAt = DateTime.Now;
                single.Status = FileInStatus.Initialized;
            }
            return single.Id;
        }
        
        public void ChangeIniDrDtlStatus(int id, FileInStatus status)
        {
            var IniExportInCtrl = Context.IniExportInCtrl.Find(id);
            if (IniExportInCtrl == null)
                throw new ArgumentException("IniExportInCtrl is not existed. " + id);
            IniExportInCtrl.Status = status;
            Context.SaveChanges();
        }
        public void ChangeIniDrOrdStatus(int id, FileInStatus status)
        {
            var IniExportInCtrl = Context.IniExportInCtrl.Find(id);
            if (IniExportInCtrl == null)
                throw new ArgumentException("IniExportInCtrl is not existed. " + id);
            IniExportInCtrl.Status = status;
            Context.SaveChanges();
        }
        public void ChangeIniOpDtlStatus(int id, FileInStatus status)
        {
            var IniExportInCtrl = Context.IniExportInCtrl.Find(id);
            if (IniExportInCtrl == null)
                throw new ArgumentException("IniExportInCtrl is not existed. " + id);
            IniExportInCtrl.Status = status;
            Context.SaveChanges();
        }
        public void ChangeIniOpOrdStatus(int id, FileInStatus status)
        {
            var IniExportInCtrl = Context.IniExportInCtrl.Find(id);
            if (IniExportInCtrl == null)
                throw new ArgumentException("IniExportInCtrl is not existed. " + id);
            IniExportInCtrl.Status = status;
            Context.SaveChanges();
        }

        public void ChangeStatus(string fee_ym, FileInStatus status)
        {
            var IniExportInCtrl = Context.IniExportInCtrl.Where(x => x.fee_ym == fee_ym).FirstOrDefault();
            if (IniExportInCtrl == null)
                throw new ArgumentException("IniExportInCtrl is not existed. " + fee_ym);
            IniExportInCtrl.Status = status;
            Context.SaveChanges();
        }
    }
}