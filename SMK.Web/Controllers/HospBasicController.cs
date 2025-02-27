using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Net.Http.Headers;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Yozian.WebCore.Library.Utility.Excel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Web;
using System.Text.RegularExpressions;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class HospBasicController : BaseController
    {
        private readonly HospContractService hospContractService;
        private readonly HospBasicService hospBasicService;
        private readonly PrsnContractService prsnContractService;
        private readonly PrsnBasicsService prsnBasicsService;
        private readonly string _folder;
        public HospBasicController(HospContractService hospContractService,
            HospBasicService hospBasicService,
            PrsnContractService prsnContractService,
            PrsnBasicsService prsnBasicsService,
            IWebHostEnvironment env)
        {
            this.hospBasicService = hospBasicService;
            this.hospContractService = hospContractService;
            this.prsnContractService = prsnContractService;
            this.prsnBasicsService = prsnBasicsService;
            _folder = $@"{env.WebRootPath}\UploadFolder";
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Application_Upload(HospContractQueryModel query)
        {
            LogicRtnModel<HospContractQueryModel> logicRtnModel = new LogicRtnModel<HospContractQueryModel>();
            switch (query.UploadType)
            {
                case ("ApplyStaute"):
                    logicRtnModel = await hospBasicService.UploadApplication(query.file);
                    break;
                case ("QuotaHosp"):
                    logicRtnModel = await hospBasicService.UploadQuotaHosp(query.file);
                    break;
                default:
                    logicRtnModel.IsSuccess = false;
                    logicRtnModel.ErrMsg = "上傳失敗，請查詢檔案正確性";
                    break;
            }

            return Json(logicRtnModel);
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(HospContractQueryModel model)
        {
            //var logicRtnModel = await hospContractService.GetHospContracts(model);
            var logicRtnModel = await hospContractService.GetHospContractsNew(model);
            return Json(logicRtnModel);
        }

        #region 戒菸服務專案申請
        /// <summary>
        /// 查詢戒菸服務專案申請
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryQuotaHosp(HospContractQueryModel model)
        {
            var logicRtnModel = await hospContractService.GetInsertQuotaHosp(model);
            return Json(logicRtnModel);
        }

        /// <summary>
        /// 列印申請概況
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ExportyQuotaHosp")]
        public async Task<IActionResult> ExportyQuotaHosp(HospContractQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await hospContractService.GetInsertQuotaHosp(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<QuotaHospHospBasicContractViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.FeeYM, "收件年月");
                        bindder.ColumnFor(p => p.FeeYMD, "收件日");
                        bindder.ColumnFor(p => p.HospID, "院所代碼");
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別");
                        bindder.ColumnFor(p => p.HospName, "院所名稱");
                        bindder.ColumnFor(p => p.LastContType, "層級別");
                        bindder.ColumnFor(p => p.ApplyTreatChange, "申請戒菸服務(戒菸治療)");
                        bindder.ColumnFor(p => p.ApplyTreatPeople, "申請戒菸服務(治療申請人次)");
                        bindder.ColumnFor(p => p.ApplyHealthEduChange, "申請戒菸服務(戒菸衛教)");
                        bindder.ColumnFor(p => p.ApplyHealthEduPeople, "申請戒菸服務(衛教申請人次)");
                        bindder.ColumnFor(p => p.DesignedTreat, "專設戒菸服務門診(戒菸治療)");
                        bindder.ColumnFor(p => p.DesignedTreatHealthEdu, "專設戒菸服務門診(戒菸衛教)");
                        bindder.ColumnFor(p => p.ResultTreat, "署審查結果(戒菸治療)");
                        bindder.ColumnFor(p => p.ResultHealthEdu, "署審查結果(戒菸衛教)");
                        bindder.ColumnFor(p => p.VPNChangeNote, "VPN異動說明");
                    })
                    .GetResult();
            });
            var fileName = $"戒菸服務專案申請.{fileType.ToString()}";
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileName);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            return new FileContentResult(excel, contentType);
        }
        #endregion

        #region 機構申請概況

        /// <summary>
        /// 查詢申請概況
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryApplication(HospContractQueryModel model)
        {
            var logicRtnModel = await hospContractService.GetInsertHospContracts(model);
            return Json(logicRtnModel);
        }


        /// <summary>
        /// 列印申請概況
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ExportyApplication")]
        public async Task<IActionResult> ExportyApplication(HospContractQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await hospContractService.GetInsertHospContracts(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<ApplicationHospBasicContractViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.FeeYM, "收件年月");
                        bindder.ColumnFor(p => p.FeeYMD, "收件日");
                        bindder.ColumnFor(p => p.Application_Type, "申請類型");
                        bindder.ColumnFor(p => p.ChangeType, "異動類型");
                        bindder.ColumnFor(p => p.HospID, "院所代碼");
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別");
                        bindder.ColumnFor(p => p.HospName, "院所名稱");
                        bindder.ColumnFor(p => p.new_Note, "備註");
                    })
                    .GetResult();
            });
            var fileName = $"機構申請概況.{fileType.ToString()}";
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileName);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            return new FileContentResult(excel, contentType);
        }

        /// <summary>
        /// 核准
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="hospStartDate"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Judge(List<HospBasicContractViewModel> contract, string hospStartDate)
        {

            return Json(await hospContractService.JudgeContracts(contract, hospStartDate));
        }
        #endregion

        /// <summary>
        /// 多筆批次刪除
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="hospStartDate"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContracts(List<HospBasicContractViewModel> contract)
        {
            return Json(await hospContractService.DeleteContracts(contract, _folder));
        }

        /// <summary>
        /// 單筆刪除
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="hospStartDate"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContract(int id)
        {
            List<HospBasicContractViewModel> contract = new List<HospBasicContractViewModel>() {
                new HospBasicContractViewModel
                {
                    Id = id,
                }
            };
            return Json(await hospContractService.DeleteContracts(contract, _folder));
        }

        /// <summary>
        /// 下載所有符合條件的PDF
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="hospStartDate"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateAntiForgeryToken]
        public List<HospContract> DownloadAll(int Id)
        {
            var data = hospContractService.QueryHospContract(Id);
            if (data == null)
            {
                return null;
            }
            var folderPath = $@"{_folder}\{data.HospId}\{data.HospSeqNo}\{data.SmkcontractType}";
            List<HospContract> filesList = new List<HospContract>();
            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
                DateTime dateTime = DateTime.MinValue;
                string pattern = @"\d{17}";
                string patternName = @"_\d{17}";
                string FileName = string.Empty;
                foreach (string filePath in files)
                {
                    string[] UploadFolder = filePath.Split("UploadFolder");
                    string[] CheckData = UploadFolder[1].Split('\\');
                    FileName = CheckData[4];

                    Match match = Regex.Match(FileName, pattern);
                    Match matchName = Regex.Match(FileName, patternName);
                    if (match.Success)
                    {
                        string timestamp = match.Value;
                        dateTime = DateTime.ParseExact(timestamp, "yyyyMMddHHmmssfff", null);
                    }
                    if (matchName.Success)
                    {
                        FileName = Regex.Replace(FileName, patternName, "").Replace(".pdf", "");
                    }

                    filesList.Add(new HospContract()
                    {
                        HospId = CheckData[1],
                        HospSeqNo = CheckData[2],
                        SmkcontractType = CheckData[3],
                        CreateDate = "",
                        Remark = FileName,
                        UpdateFileTime = dateTime,
                        Id = data.Id
                    });
                }
            }
            return filesList;
        }

        [HttpGet("DownloadGetAll")]
        public async Task<IActionResult> DownloadGetAll(string hospid, string hospseqno, string UpdateFileTime, string SMKContractType, string Remark)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(UpdateFileTime, out parsedDate))
            {
                return Json(new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = "查無檔案",
                });
            }
            //var path = $@"{_folder}\{hospid}\{hospseqno}\{hospid}{hospseqno}.pdf";
            var path = $@"{_folder}\{hospid}\{hospseqno}\{SMKContractType}\{Remark}_{parsedDate.ToString("yyyyMMddHHmmssfff")}.pdf";
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return Json(new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = "查無檔案",
                });
            }
            var memoryStream = new MemoryStream();
            using (var stream = fileInfo.OpenRead())
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(memoryStream, "application/pdf");
        }     
        
        [HttpGet("DeletePdf")]
        public IActionResult DeletePdf(string hospid, string hospseqno, string UpdateFileTime, string SMKContractType, string Remark)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(UpdateFileTime, out parsedDate))
            {
                return Json(new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = "查無檔案",
                });
            }
            //var path = $@"{_folder}\{hospid}\{hospseqno}\{hospid}{hospseqno}.pdf";
            var path = $@"{_folder}\{hospid}\{hospseqno}\{SMKContractType}\{Remark}_{parsedDate.ToString("yyyyMMddHHmmssfff")}.pdf";
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return Json(new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = "查無檔案",
                });
            }
            fileInfo.Delete();
            return Json(new LogicRtnModel<bool>()
            {
                IsSuccess = true,
                ErrMsg = "刪除成功",
            }); ;
        }

        /// <summary>
        /// 新增機構合約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult AddHospContract(HospBasicViewModel model)
        {
            model.HospContracts.Add(new HospContractViewModel()
            {
                HospId = model.HospId,
                HospSeqNo = model.HospSeqNo
            });
            return View("Edit", model);
        }

        /// <summary>
        /// 建立新機構畫面
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View(new HospBasicViewModel());
        }

        /// <summary>
        /// 建立新機構
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HospBasicViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await hospBasicService.CreateHospBasic(model);
            if (result.IsSuccess)
                return RedirectToAction(nameof(Edit),
                    new
                    {
                        hospid = result.Data.HospId,
                        hospseqno = result.Data.HospSeqNo
                    });
            else
            {
                ModelState.AddModelError("", result.ErrMsg + ", StackTrace:" + result.StackTrace);
                LogicRtnModel<HospBasicViewModel> logicRtnModel = new LogicRtnModel<HospBasicViewModel>()
                {
                    Data = model,
                    ErrMsg = result.ErrMsg,
                    IsSuccess = result.IsSuccess,
                    StackTrace = result.StackTrace
                };
                return View(model);
            }
        }

        /// <summary>
        /// 取得機構資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryHospId(HospBasicViewModel model)
        {
            if (string.IsNullOrEmpty(model.HospId))
            {
                return View("Create", new LogicRtnModel<HospBasicViewModel>()
                {
                    Data = new HospBasicViewModel(),
                    IsSuccess = false,
                    ErrMsg = "機構代碼為必填",
                });
            }

            var logicRtn = await hospBasicService.getHospBasicByOrgId(model.HospId);
            if (!logicRtn.IsSuccess)
                logicRtn.Data = model;
            return View("Create", logicRtn);
        }

        /// <summary>
        /// 編輯機構資料及合約資料
        /// </summary>
        /// <param name="hospid"></param>
        /// <param name="hospseqno"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(string hospid, string hospseqno)
        {
            if (string.IsNullOrEmpty(hospid))
            {
                return View(new LogicRtnModel<HospBasicViewModel>()
                {
                    Data = new HospBasicViewModel(),
                    IsSuccess = false,
                    ErrMsg = "醫院代碼不得為空白",
                });
            }
            if (string.IsNullOrEmpty(hospseqno))
            {
                hospseqno = "00";
            }
            var result = await hospBasicService.getHospBasic(hospid, hospseqno);
            return View(result);
        }

        /// <summary>
        /// 更新機構資料及合約資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hospBasic"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeHospId(string hospid, string hospseqno, string oldhospid, string oldhospseqno)
        {
            var rtnModel = await hospBasicService.ChangeHospId(hospid, hospseqno, oldhospid, oldhospseqno);

            return Json(rtnModel);
        }

        /// <summary>
        /// 更新機構資料及合約資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hospBasic"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HospBasicViewModel model)
        {
            LogicRtnModel<HospBasicViewModel> logicRtnModel = new LogicRtnModel<HospBasicViewModel>()
            {
                IsSuccess = false,
                ErrMsg = "",
            };
            if (!ModelState.IsValid)
            {
                logicRtnModel.ErrMsg = "資料驗證失敗";
                return RedirectTo(logicRtnModel, "Edit", "HospBasic", new { hospid = model.HospId , hospseqno = model.HospSeqNo});
            }

            foreach(var item in model.HospContractTypes)
            {
                int CheckHospContractTypes = model.HospContractTypes.Where(x => x.HospContType == item.HospContType && x.CntSDate == item.CntSDate).Count();
                if (CheckHospContractTypes > 1)
                {
                    logicRtnModel.ErrMsg = "重複Key，【健保特約類別】與【特約類別起日】出現重複";
                    return RedirectTo(logicRtnModel, "Edit", "HospBasic", new { hospid = model.HospId, hospseqno = model.HospSeqNo });
                }
            }
            var rtnModel = await hospBasicService.updateHospBasic(model);
            return RedirectTo(rtnModel, "Edit", "HospBasic", new { hospid = model.HospId, hospseqno = model.HospSeqNo });
            #region 舊版跳轉邏輯有問題
            //if (rtnModel.IsSuccess)
            //{
            //    return RedirectTo(rtnModel, nameof(this.Index));
            //}
            //else
            //{
            //    ModelState.AddModelError("", rtnModel.ErrMsg);
            //    return View(rtnModel.Data);
            //}
            #endregion
        }

        /// <summary>
        /// 移除機構資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hospBasic"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string hospid, string hospseqno)
        {
            if (string.IsNullOrEmpty(hospid))
            {
                return View(new LogicRtnModel<HospBasicViewModel>()
                {
                    Data = new HospBasicViewModel(),
                    IsSuccess = false,
                    ErrMsg = "醫院代碼不得為空白",
                });
            }

            if (string.IsNullOrEmpty(hospseqno))
            {
                hospseqno = "00";
            }
            var logicRtnModel =
                await hospBasicService.FindOne<HospBasic>(
                    context => context.Where(p => p.HospId == hospid
                    && p.HospSeqNo == hospseqno));
            if (!logicRtnModel.IsSuccess)
            {
                Json(logicRtnModel);
            }

            var rtnModel = await hospBasicService.Remove(logicRtnModel.Data);
            if (rtnModel.IsSuccess)
            {
                return Json(rtnModel);
            }
            else
            {
                return Json(rtnModel);
            }
        }



        /// <summary>
        /// 檢視機構及醫事人員合約資料
        /// </summary>
        /// <param name="hospid"></param>
        /// <param name="hospseqno"></param>
        /// <returns></returns>
        public async Task<IActionResult> HospPrsnList(string hospid, string hospseqno)
        {
            if (string.IsNullOrEmpty(hospid))
            {
                return View(new LogicRtnModel<HospPrsnViewModel>()
                {
                    Data = new HospPrsnViewModel(),
                    IsSuccess = false,
                    ErrMsg = "醫院代碼不得為空白",
                });
            }
            if (string.IsNullOrEmpty(hospseqno))
            {
                hospseqno = "00";
            }
            var logicRtnModel = await hospBasicService.FindOne<HospBasic>(context => context.Where(p => p.HospId == hospid && p.HospSeqNo == hospseqno));
            if (logicRtnModel == null)
            {
                return View(new LogicRtnModel<HospPrsnViewModel>()
                {
                    Data = new HospPrsnViewModel(),
                    IsSuccess = false,
                    ErrMsg = "查無醫事機構資訊",
                });
            }
            var hospBasic = logicRtnModel.Data;
            HospPrsnViewModel hospPrsnViewModel = new HospPrsnViewModel()
            {
                HospId = hospBasic.HospId,
                HospSeqNo = hospBasic.HospSeqNo,
                HospName = hospBasic.HospName,
                //PrsnContracts=await prsnContractService.GetPrsnContracts(hospid, hospseqno)
            };

            return View(new LogicRtnModel<HospPrsnViewModel>()
            {
                Data = hospPrsnViewModel
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HospPrsnContracts(PrsnContractQueryModel model)
        {
            var logicRtnModel = await prsnContractService.GetHospPrsnContracts(model);
            return Json(logicRtnModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckPrsnId(string prsnId)
        {
            if (string.IsNullOrEmpty(prsnId))
            {
                return Json(new LogicRtnModel<HospPrsnViewModel>()
                {
                    IsSuccess = false,
                    ErrMsg = "身分證號不得為空白",
                });
            }

            var logicRtnModel = await hospBasicService.FindOne<PrsnBasic>(context =>
                                                                        context.Where(p => p.PrsnId == prsnId));
            return Json(logicRtnModel);
        }

        /// <summary>
        /// 新增機構醫事人員合約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> InsertPrsnContract(PrsnContractViewModel model)
        {
            var result = await prsnBasicsService.InsertPrsnContract(model);
            return Json(result);
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UploadFile(string hospid, string hospseqno, IFormFile file, string CreateDate, string SMKContractType, int Id)
        {
            if (file.Length == 0)
            {
                return Json(new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = "未選擇檔案",
                });
            }
            var UpdateFileTime = DateTime.Now;

            var result = await prsnBasicsService.UploadFileTime(UpdateFileTime, Id);
            if (!result.IsSuccess)
            {
                return Json(new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = result.ErrMsg,
                });
            }

            var path = $@"{_folder}\{hospid}\{hospseqno}\{SMKContractType}\{file.FileName.Split('.')[0]}_{UpdateFileTime.ToString("yyyyMMddHHmmssfff")}.pdf";
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                fileInfo.Directory.Create();
            }
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Json(new LogicRtnModel<bool>());
        }

        [HttpHead("Download")]
        public IActionResult IsExists(string hospid, string hospseqno, string CreateDate, string SMKContractType, string UpdateFileTime, int Id)
        {
            //var path = $@"{_folder}\{hospid}\{hospseqno}\{hospid}{hospseqno}.pdf";
            var path = $@"{_folder}\{hospid}\{hospseqno}\{SMKContractType}\{Id}\{CreateDate}\{hospid}_{hospseqno}_{UpdateFileTime}.pdf";
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                return NotFound();
            return Ok();
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download(string hospid, string hospseqno, string CreateDate, string SMKContractType, string UpdateFileTime, int Id)
        {
            //var path = $@"{_folder}\{hospid}\{hospseqno}\{hospid}{hospseqno}.pdf";
            var path = $@"{_folder}\{hospid}\{hospseqno}\{SMKContractType}\{Id}\{CreateDate}\{hospid}_{hospseqno}_{UpdateFileTime}.pdf";
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return Json(new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = "查無檔案",
                });
            }
            var memoryStream = new MemoryStream();
            using (var stream = fileInfo.OpenRead())
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(memoryStream, "application/pdf");
        }


        public async Task<IActionResult> QueryHospName(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return Json(new LogicRtnModel<string>());
            }

            var logicRtnModel = await hospBasicService.Query(context =>
            context.HospBasic.Where(p => p.HospName.Contains(keyword))
                             .Select(p => new { id = p.HospId, label = p.HospName })
                             .Take(20));
            return Json(logicRtnModel);
        }
    }
}
