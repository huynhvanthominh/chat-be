namespace chat_be.Models.Responses
{
    public class PaginatedResponse<T> 
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<T> Data { get; set; }

        public PaginatedResponse(int page, int pageSize, int total, List<T> data)
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
            Data = data;
        }
    }
}