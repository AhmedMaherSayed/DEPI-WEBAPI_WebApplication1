using Microsoft.VisualStudio.Web.CodeGenerators.Mvc;

namespace WebApplication1.Results
{
    public class PaginationedList<T>
    {
        private const int MaxPageSize = 100;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public IEnumerable<T> Data { get; set; }
    }
}
