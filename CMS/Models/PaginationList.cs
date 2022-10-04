using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models
{
    public class PaginationList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public PaginationList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public static async Task<PaginationList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int count = await source.CountAsync();
            List<T> items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginationList<T>(items, count, pageIndex, pageSize);
        }
    }
}
