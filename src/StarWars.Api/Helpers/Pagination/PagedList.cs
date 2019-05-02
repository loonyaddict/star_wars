using System;
using System.Collections.Generic;
using System.Linq;

namespace StarWars.API.Helpers
{
    /// <summary>
    /// Custom List for paging manipulation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>
    {
        private PagedList(List<T> list, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(list);
        }

        /// <summary>
        /// Create specified page from IQueryable source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var list = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(list, count, pageNumber, pageSize);
        }

        /// <summary>
        /// Current page from source.
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        /// Total pages in source.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Number of records to display on single source.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Total count of pages in PagedList
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Has previous page.
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Has next page.
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;
    }
}