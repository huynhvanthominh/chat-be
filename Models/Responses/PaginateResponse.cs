namespace chat_be.Models.Responses
{
    public class PaginatedResponse<T> 
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }

        public int CountPerPage { get; set; }
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }

        public PaginatedResponse(
            int TotalPage,
            int CurrentPage,
            int CountPerPage,
            int TotalCount,
            List<T> Data
            )
        {
            this.TotalPage = TotalPage;
            this.CurrentPage = CurrentPage;
            this.CountPerPage = CountPerPage;
            this.TotalCount = TotalCount;
            this.Data = Data;
        }
    }
}