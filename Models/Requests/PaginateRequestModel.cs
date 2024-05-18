namespace chat_be.Models.Requests
{
    public class PaginateRequest
    {
        public int Page { get; set; }
        public int CountPerPage { get; set; }
        public PaginateRequest()
        {
            Page = 1;
            CountPerPage = 10;
        }
        public PaginateRequest(int Page, int CountPerPage)
        {
            this.Page = Page;
            this.CountPerPage = CountPerPage;
        }
    }
}