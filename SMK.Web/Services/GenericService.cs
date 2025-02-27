using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Dto;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yozian.Extension;
using Yozian.EFCorePlus.Extensions;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMK.Web.Helpers;
using SMK.Data.Utility;

namespace SMK.Web.Services
{
    public abstract class GenericService
    {

        protected readonly SMKWEBContext context;
        protected readonly SessionManager smgr;
        protected readonly IdentityModel identity;

        public GenericService(SMKWEBContext context, SessionManager smgr)
        {
            this.context = context;
            this.smgr = smgr;
            this.identity = smgr.Get<IdentityModel>();
        }
        public virtual async Task<LogicRtnModel<List<TEntity>>> Query<TEntity>(Func<SMKWEBContext, IQueryable<TEntity>> preparedQuery) {
            try
            {
                var rtnModel = new LogicRtnModel<List<TEntity>> ();

                rtnModel.Data = await preparedQuery(context).ToListAsync();
                //.ToPagedListAsync(model.Page, model.Limits);

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<List<TEntity>> (MsgType.QueryFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }
        }
        public virtual async Task<LogicRtnModel<PagedModel<TEntity>>> QueryPaging<TEntity>(
            PagedRequest model,
            IQueryable<TEntity> preparedQuery
            )
        {
            try
            {
                var rtnModel = new LogicRtnModel<PagedModel<TEntity>>();

                rtnModel.Data = await preparedQuery.ToPagedListAsync(model);

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<PagedModel<TEntity>>(MsgType.QueryFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }
        }
        public virtual async Task<LogicRtnModel<PagedModel<TEntity>>> QueryPaging<TEntity>(
            PagedRequest model,
            Func<SMKWEBContext, IQueryable<TEntity>> preparedQuery
            )
        {
            try
            {
                var rtnModel = new LogicRtnModel<PagedModel<TEntity>>();

                rtnModel.Data = await preparedQuery(context)
                    .ToPagedListAsync(model);

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<PagedModel<TEntity>>(MsgType.QueryFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }
        }
        public virtual async Task<LogicRtnModel<TEntity>> FindOne<TEntity>(
                Func<DbSet<TEntity>,
                IQueryable<TEntity>> query
            )
            where TEntity : class, new()
        {
            try
            {
                var rtnModel = new LogicRtnModel<TEntity>();
                // if not found always throw ex
                rtnModel.Data = await query(context.Set<TEntity>()).FirstAsync();

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<TEntity>(MsgType.QueryFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }

        }

        public virtual async Task<LogicRtnModel<TEntity>> FindOne<TEntity, TExtend>(
               Func<DbSet<TEntity>,
               IQueryable<TEntity>> query,
               Func<SMKWEBContext, Task<TExtend>> fetchExtendData
           )
           where TEntity : class, new()
        {
            try
            {
                var rtnModel = new LogicRtnModel<TEntity>();
                // if not found always throw ex
                rtnModel.Data = await query(context.Set<TEntity>()).FirstAsync();

                rtnModel.ExtendData = await fetchExtendData(context);

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<TEntity>(MsgType.QueryFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }

        }
        public virtual async Task<LogicRtnModel<bool>> Any<TEntity>(
                Func<DbSet<TEntity>,
                IQueryable<TEntity>> query
            )
            where TEntity : class, new()
        {
            try
            {
                var rtnModel = new LogicRtnModel<bool>();
                // if not found always throw ex
                rtnModel.Data = await query(context.Set<TEntity>()).AnyAsync();

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<bool>(MsgType.QueryFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }

        }
        public virtual async Task<LogicRtnModel<TEntity>> Create<TEntity>(
            TEntity model,
            Func<TEntity, ValidationResult> validator = null,
            bool withAudit = true
            )
            where TEntity : class, new()
        {
            try
            {
                var rtnModel = new LogicRtnModel<TEntity>(MsgType.CreateSuccess);
                if (validator != null)
                {
                    ValidationResult validationResult = validator(model);
                    if (!validationResult.IsValid)
                        return ValidationFlase<TEntity>(MsgType.CreateFail,validationResult);
                }

                context.Set<TEntity>().Add(model);
                var result = await (withAudit ? context.SaveChangesWithAuditAsync(identity.Account, "新增") : context.SaveChangesAsync());

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<TEntity>(MsgType.CreateFail, ex.Message)
                {
                    Data = model,
                    StackTrace = ex.DumpDetail()
                };
            }

        }


        public virtual async Task<LogicRtnModel<TEntity>> Update<TEntity>(
                TEntity model,
                Func<TEntity, ValidationResult> validator = null,
                bool withAudit = true,
                params Expression<Func<TEntity, object>>[] updateProperties
            )
             where TEntity : class, new()
        {
            try
            {
                var rtnModel = new LogicRtnModel<TEntity>(MsgType.SaveSuccess)
                {
                    Data = model
                };

                if (validator != null)
                {
                    ValidationResult validationResult = validator(model);
                    if (!validationResult.IsValid)
                        return ValidationFlase<TEntity>(MsgType.CreateFail, validationResult);
                }

                var entry = context.Update(
                      model,
                      updateProperties
                      );
                await (withAudit ? context.SaveChangesWithAuditAsync(identity.Account) : context.SaveChangesAsync());

                await context.Entry(model).ReloadAsync();

                return rtnModel;
            }
            catch (Exception ex)
            {

                return new LogicRtnModel<TEntity>(MsgType.SaveFail)
                {
                    Data = model,
                    StackTrace = ex.DumpDetail()
                };
            }

        }



        public virtual async Task<LogicRtnModel<bool>> Remove<TEntity>(
                TEntity model,
                Func<TEntity, ValidationResult> validator = null,
                bool withAudit = true
            )
              where TEntity : class, new()
        {
            try
            {
                var rtnModel = new LogicRtnModel<bool>(MsgType.RemoceSuccess);

                if (validator != null)
                {
                    ValidationResult validationResult = validator(model);
                    if (!validationResult.IsValid)
                        return ValidationFlase<bool>(MsgType.ProcessFail, validationResult);
                }

                context.Entry(model).State = EntityState.Deleted;

                await (withAudit ? context.SaveChangesWithAuditAsync(identity.Account) : context.SaveChangesAsync());

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<bool>(MsgType.RemoveFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }

        }
        /// <summary>
        /// 提供下拉選單
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="preparedQuery"></param>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetSelectLists<TEntity>(Func<SMKWEBContext, IQueryable<TEntity>> preparedQuery,
            Expression<Func<TEntity, string>> value, 
            Expression<Func<TEntity, string>> text) {
            
            return await preparedQuery(context)
                .Select(p =>
                new SelectListItem(
                    text.Compile().Invoke(p), 
                    value.Compile().Invoke(p)))
                .ToListAsync();
        }

        public List<SelectListItem> GetEnumList<T>() where T : Enum
        {
            return EnumExtension.getEnumList<T>();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        protected LogicRtnModel<TEntity> ValidationFlase<TEntity>(MsgType msgTyp,ValidationResult validationResult) {
            return new LogicRtnModel<TEntity>(msgTyp, string.Join("\r\n", validationResult.Errors));            
        }

        #region 自訂義下載Excel的共用方法
        /// <summary>
        /// 字典分別套所有的bool
        /// </summary>
        /// <param name="Data">字典</param>
        /// <param name="TrueOrFalse">數值</param>
        /// <returns></returns>
        public Dictionary<string, Tuple<string, bool>> DictionaryDataForAll(Dictionary<string, Tuple<string, bool>> Data, bool TrueOrFalse)
        {
            Data = Data.ToDictionary(
                            kvp => kvp.Key,
                            kvp => new Tuple<string, bool>(kvp.Value.Item1, TrueOrFalse)
                        );
            return Data;
        }

        /// <summary>
        /// 字典針對指定Key，給相關值
        /// </summary>
        /// <param name="Data">字典</param>
        /// <param name="key">字典Key</param>
        /// <returns></returns>
        public Dictionary<string, Tuple<string, bool>> DictionaryDataForOutPut(Dictionary<string, Tuple<string, bool>> Data, string key)
        {
            if (Data.ContainsKey(key))
            {
                Data[key] = new Tuple<string, bool>(Data[key].Item1, true);
            }
            return Data;
        }
        #endregion
    }
}
