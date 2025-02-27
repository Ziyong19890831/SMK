using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using static Dapper.SqlMapper;

namespace SMK.Web.Services.Foundation
{

    [ScopedService]
    public class CallBoardService : GenericService
    {
        public CallBoardService(SMKWEBContext context, SessionManager smgr)
           : base(context, smgr)
        {
        }
        /// <summary>
        /// 查詢布告欄資訊
        /// </summary>
        /// <returns></returns>
        public LogicRtnModel<List<CallBoardViewModel>> QueryCallBoard()
        {
            var rtnModel = new LogicRtnModel<List<CallBoardViewModel>>();
            var data = context.CallBoard
                       .Where(x => x.Action == false)
                       .Select(x => new CallBoardViewModel
                       {
                           SysNo = x.SysNo,
                           StartDate = x.AnnouncementDate.ToString("d"),
                           Note = x.Note.Replace("\n", "<br />"),
                           Condition = x.Condition,
                           Action = x.Action,
                       })
                       .OrderByDescending(x => x.SysNo)
                       .AsNoTracking()
                       .ToList();
            rtnModel.Data = data;
            return rtnModel;
        }

        /// <summary>
        /// 新增布告欄資訊
        /// </summary>
        /// <param name="model"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<CallBoard>> CreateCallBoard(CallBoard model, List<IFormFile> files)
        {
            LogicRtnModel<CallBoard> ret = new LogicRtnModel<CallBoard>() { IsSuccess = true };
            try
            {
                model.UpdatedBy = identity.Name;
                context.CallBoard.Add(model);
                context.SaveChanges();
                ret.Data = model;
                ret.Msg = "新增成功";
            }
            catch (Exception e)
            {
                return new LogicRtnModel<CallBoard>
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace
                };
            }
            return ret;
        }

        /// <summary>
        /// 刪除公告與顯示/隱藏公告
        /// </summary>
        /// <param name="Sysno">Key</param>
        /// <param name="Action">刪除；1刪除；0未刪除</param>
        /// <param name="Condition">是否顯示:1顯示；0未顯示</param>
        /// <returns></returns>
        public async Task<LogicRtnModel<CallBoard>> DeleteOrConditionCallBoard(int Sysno, bool Action = false, bool Condition = false)
        {
            LogicRtnModel<CallBoard> ret = new LogicRtnModel<CallBoard>() { IsSuccess = true };
            try
            {
                var existingCallBoard = await context.CallBoard.FindAsync(Sysno);
                if (existingCallBoard == null)
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "找不到資料";
                    return ret;
                }
                else
                {
                    existingCallBoard.Action = Action;
                    existingCallBoard.Condition = Condition;
                    existingCallBoard.UpdatedBy = identity.Name;
                    existingCallBoard.TxtDate = DateTime.Now;
                    await context.SaveChangesAsync();
                }
                ret.Data = existingCallBoard;
                ret.Msg = "更新成功";
            }
            catch (Exception e)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = e.Message;
                ret.StackTrace = e.StackTrace;
            }
            return ret;
        }
        /// <summary>
        /// 更新布告欄資訊
        /// </summary>
        /// <param name="model"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<CallBoard>> UpdateCallBoard(CallBoard model, List<IFormFile> files)
        {
            LogicRtnModel<CallBoard> ret = new LogicRtnModel<CallBoard>() { IsSuccess = true };
            model.UpdatedBy = identity.Name;
            model.TxtDate = DateTime.Now;
            try
            {
                var existingCallBoard = await context.CallBoard.FindAsync(model.SysNo);
                if (existingCallBoard == null)
                {
                    ret.IsSuccess = false;
                    ret.ErrMsg = "找不到資料";
                    return ret;
                }
                else
                {
                    existingCallBoard.AnnouncementDate = model.AnnouncementDate;
                    existingCallBoard.Action = model.Action;
                    existingCallBoard.Condition = model.Condition;
                    existingCallBoard.Note = model.Note;
                    existingCallBoard.UpdatedBy = model.UpdatedBy;
                    existingCallBoard.TxtDate = model.TxtDate;
                    await context.SaveChangesAsync();
                }
                ret.Data = existingCallBoard;
                ret.Msg = "更新成功";
            }
            catch (Exception e)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = e.Message;
                ret.StackTrace = e.StackTrace;
            }
            return ret;
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="files"></param>
        /// <param name="_folder"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<CallBoard>> UpdateFiles(int sysNo, List<IFormFile> files, string _folder)
        {
            LogicRtnModel<CallBoard> ret = new LogicRtnModel<CallBoard>() { IsSuccess = true };
            try
            {
                foreach (var file in files)
                {
                    var fileName = $"{file.FileName}";
                    if (file.Length == 0)
                    {
                        // 文件為空
                        ret.Msg += $"【{fileName}】文件為空，所以不支援。";
                        continue; // 跳過當前文件，處理下一個文件
                    }
                    // 檢查擴展名是否符合要求
                    var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
                    if (fileExtension != ".doc" && fileExtension != ".docx" &&
                        fileExtension != ".xls" && fileExtension != ".xlsx" &&
                        fileExtension != ".pdf")
                    {
                        // 不是受支持的檔案格式
                        ret.Msg += $"【{fileName}】為檔案格式不支援。";
                        continue; // 跳過當前文件，處理下一個文件
                    }
                    AddFiles(file, _folder, sysNo, fileName);
                }
                return ret;
            }
            catch (Exception e)
            {
                ret.IsSuccess = false;
                ret.ErrMsg = e.Message;
                ret.StackTrace = e.StackTrace;
                return ret;
            }
        }

        /// <summary>
        /// 新增檔案
        /// </summary>
        /// <param name="formFile">檔案</param>
        /// <param name="_folder">檔案路徑</param>
        /// <param name="sysNo">流水號</param>
        /// <param name="fileName">檔案名稱</param>
        public async Task AddFiles(IFormFile formFile, string _folder, int sysNo, string fileName)
        {
            var filePath = $"{_folder}/{sysNo}";
            var AllfilePath = $"{_folder}/{sysNo}/{fileName}";
            FileInfo fileInfo = new FileInfo(AllfilePath);
            if (!fileInfo.Exists)
            {
                fileInfo.Directory.Create();
            }
            if (System.IO.File.Exists(AllfilePath))
            {
                System.IO.File.Delete(AllfilePath);
            }
            using (var stream = new FileStream(AllfilePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
        }
        /// <summary>
        /// 移除檔案時，自動備份
        /// </summary>
        /// <param name="Sysno">流水號</param>
        /// <param name="FileNames">檔案名稱</param>
        /// <param name="_folder">原始路徑</param>
        /// <param name="_Deletefolder">移除檔案路徑</param>
        /// <returns></returns>
        public async Task<bool> RemoveFile(int Sysno, string FileNames, string _folder, string _Deletefolder)
        {
            try
            {

                var filePath = Path.Combine(_folder, Sysno.ToString(), FileNames);
                var DeleteAllfilePath = Path.Combine(_Deletefolder, Sysno.ToString());
                var DeleteFilePath = Path.Combine(DeleteAllfilePath, FileNames);

                Directory.CreateDirectory(DeleteAllfilePath);
                if (File.Exists(DeleteFilePath))
                {
                    File.Delete(DeleteFilePath);
                }
                File.Move(filePath, DeleteFilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 抓取檔案路徑
        /// </summary>
        /// <param name="_folder">路徑</param>
        /// <param name="sysNo">流水號</param>
        /// <returns></returns>
        public List<(string Path, string FileName)> GetFilesData(string _folder, int sysNo)
        {
            _folder = $"{_folder}\\{sysNo}";
            if (!Directory.Exists(_folder)) // Check if the directory exists
            {
                return null; // Return null if the directory does not exist
            }
            string spilt = $"/CallBoardFolder/" + _folder.Split("CallBoardFolder\\")[1];

            DirectoryInfo list = new DirectoryInfo(_folder);
            var filelist = list.EnumerateFiles()
                               .Select(p => (
                                   Path: spilt,
                                   FileName: p.Name
                               ))
                               .ToList();
            return filelist;
        }
    }

}
