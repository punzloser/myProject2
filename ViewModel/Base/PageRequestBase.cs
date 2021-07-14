namespace ViewModel.Base
{
    public class PageRequestBase : BearerToken
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
