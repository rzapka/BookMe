using System;
using System.Collections.Generic;

namespace BookMe.Application.Pagination
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PagedResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));

            Items = items;
            TotalItemsCount = totalCount;
            ItemsFrom = totalCount == 0 ? 0 : pageSize * (pageNumber - 1) + 1;
            ItemsTo = totalCount == 0 ? 0 : Math.Min(ItemsFrom + pageSize - 1, totalCount);
            TotalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
