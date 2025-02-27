using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using SMK.Data.Dto;
using SMK.Data.Enums;
using SMK.Web.Models;
using SMK.Web.Repositories;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class HospBasicExportService
    {
        private readonly HospBasicRepository hospBasicRepository;
        private readonly PrsnContractRepository prsnContractRepository;

        public HospBasicExportService(HospBasicRepository hospBasicRepository,
            PrsnContractRepository prsnContractRepository)
        {
            this.hospBasicRepository = hospBasicRepository;
            this.prsnContractRepository = prsnContractRepository;
        }

        public async Task<LogicRtnModel<IEnumerable<HospBasicExportModel>>> Query(HospBasicExportQueryModel query)
        {
            try
            {
                var result = await hospBasicRepository.QueryHospBasicList(
                    query.HospCont,
                    query.HospStatus,
                    query.CouldTreat,
                    query.CouldInstruct,
                    query.ContractType2,
                    query.ContractType3);
                return new LogicRtnModel<IEnumerable<HospBasicExportModel>>()
                {
                    IsSuccess = true,
                    Data = result
                };
            }
            catch (Exception e)
            {
                return new LogicRtnModel<IEnumerable<HospBasicExportModel>>(MsgType.SaveFail, e.Message) 
                {
                    IsSuccess = false,
                    StackTrace = e.StackTrace
                };
            }
        }
        
        public async Task<LogicRtnModel<IEnumerable<PrsnContractExportModel>>> QueryPrsnContracts(HospBasicExportQueryModel query)
        {
            try
            {
                var result = await prsnContractRepository.QueryPrsnContracts(
                    query.HospCont,
                    query.HospStatus,
                    query.CouldTreat,
                    query.CouldInstruct,
                    query.ContractType2,
                    query.ContractType3);
                return new LogicRtnModel<IEnumerable<PrsnContractExportModel>>()
                {
                    IsSuccess = true,
                    Data = result
                };
            }
            catch (Exception e)
            {
                return new LogicRtnModel<IEnumerable<PrsnContractExportModel>>(MsgType.SaveFail, e.Message) 
                {
                    IsSuccess = false,
                    StackTrace = e.StackTrace
                };
            }
        }
    }
}