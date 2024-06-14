namespace APICatalogo.Pagination
{
    public class ProdutosParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public int _pageSize;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;

                /*if (value > maxPageSize)
                {
                    _pageSize = maxPageSize;
                }
                else
                {
                    _pageSize = value;
                }*/
            }
        }
    }
}
