using SMK.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMK.Data.Utility
{
    public static class PaginationExtension
    {
        public static PagedModel<T> ToPagedList<T>(
            this IQueryable<T> source,
            PagedRequest request)
        {
            return PagedModel<T>.Create(source, request);
        }

        public static Task<PagedModel<T>> ToPagedListAsync<T>(
         this IQueryable<T> source,
            PagedRequest request)
        {
            return PagedModel<T>.CreateAsync(source, request);
        }

    }
}

