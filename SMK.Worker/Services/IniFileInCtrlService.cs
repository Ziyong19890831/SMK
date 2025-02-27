using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Enums;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Worker.Services
{
    [SingletonService]
    public class IniFileInCtrlService
    {
        public IniFileInCtrlService(SMKWEBContext context)
        {
            Context = context;
        }

        public SMKWEBContext Context { get; set; }

        public int Initialize(string filename)
        {
            var single = Context.IniFileInCtrl.Where(x => x.Filename == filename).FirstOrDefault();
            if (single == null)
            {
                var iniFileInCtl = new IniFileInCtrl()
                {
                    Filename = filename,
                    StartedAt = DateTime.Now,
                };
                Context.IniFileInCtrl.Add(iniFileInCtl);
                Context.SaveChanges();
                return iniFileInCtl.Id;
            }
            else
            {
                single.StartedAt = DateTime.Now;
                single.Status = FileInStatus.Initialized;
                Context.IniFileInCtrl.Update(single);
                Context.SaveChanges();
            }
            return single.Id;
        }
        
        public void ChangeIniDrDtlStatus(int id, FileInStatus status)
        {
            var iniFileInCtrl = Context.IniFileInCtrl.Find(id);
            if (iniFileInCtrl == null)
                throw new ArgumentException("InitFileInCtrl is not existed. " + id);
            iniFileInCtrl.Status = status;
            Context.IniFileInCtrl.Update(iniFileInCtrl);
            Context.SaveChanges();
        }
        public void ChangeIniDrOrdStatus(int id, FileInStatus status)
        {
            var iniFileInCtrl = Context.IniFileInCtrl.Find(id);
            if (iniFileInCtrl == null)
                throw new ArgumentException("InitFileInCtrl is not existed. " + id);
            iniFileInCtrl.Status = status;
            Context.IniFileInCtrl.Update(iniFileInCtrl);
            Context.SaveChanges();
        }
        public void ChangeIniOpDtlStatus(int id, FileInStatus status)
        {
            var iniFileInCtrl = Context.IniFileInCtrl.Find(id);
            if (iniFileInCtrl == null)
                throw new ArgumentException("InitFileInCtrl is not existed. " + id);
            iniFileInCtrl.Status = status;
            Context.IniFileInCtrl.Update(iniFileInCtrl);
            Context.SaveChanges();
        }
        public void ChangeIniOpOrdStatus(int id, FileInStatus status)
        {
            var iniFileInCtrl = Context.IniFileInCtrl.Find(id);
            if (iniFileInCtrl == null)
                throw new ArgumentException("InitFileInCtrl is not existed. " + id);
            iniFileInCtrl.Status = status;
            Context.IniFileInCtrl.Update(iniFileInCtrl);
            Context.SaveChanges();
        }
        
        public void ChangeMhbtQsData2Status(int id, FileInStatus status)
        {
            var iniFileInCtrl = Context.IniFileInCtrl.Find(id);
            if (iniFileInCtrl == null)
                throw new ArgumentException("InitFileInCtrl is not existed. " + id);
            iniFileInCtrl.Status = status;
            iniFileInCtrl.UpdatedAt = DateTime.Now;
            Context.IniFileInCtrl.Update(iniFileInCtrl);
            Context.SaveChanges();
        }

        public void ChangeStatus(string filename, FileInStatus status)
        {
            var iniFileInCtrl = Context.IniFileInCtrl.Where(x => x.Filename == filename).FirstOrDefault();
            if (iniFileInCtrl == null)
                throw new ArgumentException("InitFileInCtrl is not existed. " + filename);
            iniFileInCtrl.Status = status;
            Context.IniFileInCtrl.Update(iniFileInCtrl);
            Context.SaveChanges();
        }
    }
}