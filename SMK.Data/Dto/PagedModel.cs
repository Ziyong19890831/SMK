using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMK.Data.Dto
{
    public class PagedRequest{

        [JsonProperty("draw")]
        public int Draw { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }


        //public int Page { 
        //    get {
        //        return Start / Length;
        //    } 
        //}
        public PagedRequest get() {
            return this;
        }
    }

    public class PagedModel<T>: PagedRequest
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get { return RecordsTotal; } }
        public string Error { get; set; }

        public static PagedModel<TModel> Create<TModel>(IQueryable<TModel> source, PagedRequest request)
        {
            var model = new PagedModel<TModel>();
            model.Draw = request.Draw;
            model.Start = request.Start;
            model.Length = request.Length;
            model.RecordsTotal = source.Count();
            model.Data = source.Skip(model.Start)
                .Take(model.Length)
                .ToList();
            return model;
        }

        public static async Task<PagedModel<TModel>> CreateAsync<TModel>(IQueryable<TModel> source, PagedRequest request)
        {
            var model = new PagedModel<TModel>();
            model.Draw = request.Draw;
            model.Start = request.Start;
            model.Length = request.Length;
            model.RecordsTotal = source.Count();
            model.Data = await source.Skip(model.Start)
                .Take(model.Length)
                .ToListAsync();
            return model;
        }
        private PagedModel() { }
    }
}
