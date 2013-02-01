using System.Collections.Generic;
using System.Linq;

namespace QuickStartProject.Domain.Auxilary
{
    /// <summary>
    /// 	http://wekeroad.com/2007/12/10/aspnet-mvc-pagedlistt/
    /// </summary>
    public interface IPagedList
    {
        int TotalCount { get; set; }

        int PageIndex { get; set; }

        int PageSize { get; set; }

        bool IsPreviousPage { get; }

        bool IsNextPage { get; }

        int PageCount { get; }

        bool IsValidPage { get; }
    }

    public class PagedList<T> : List<T>, IPagedList
    {
        public PagedList(IQueryable<T> source, int index, int pageSize)
        {
            TotalCount = source.Count();
            PageSize = pageSize;
            PageIndex = index;
            AddRange(source.Skip((index - 1)*pageSize).Take(pageSize).ToList());
        }

        public PagedList(List<T> source, int index, int pageSize)
        {
            TotalCount = source.Count();
            PageSize = pageSize;
            PageIndex = index;
            AddRange(source.Skip((index - 1)*pageSize).Take(pageSize).ToList());
        }

        #region IPagedList Members

        public int TotalCount { get; set; }

        public int PageCount
        {
            get
            {
                int pages = (TotalCount/PageSize);
                if (TotalCount%PageSize != 0)
                {
                    pages++;
                }
                return pages;
            }
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsPreviousPage
        {
            get { return (PageIndex > 1); }
        }

        public bool IsNextPage
        {
            get { return PageIndex < PageCount; }
        }

        public bool IsValidPage
        {
            get { return (PageIndex >= 1) && (PageIndex <= PageCount); }
        }

        #endregion
    }

    public static class Pagination
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index = 0, int pageSize = 10)
        {
            return new PagedList<T>(source, index, pageSize);
        }
    }
}