using System.Collections.Generic;

namespace ViewModel.Base
{
    public class PageResult<T> : PageResultBase
    {
        public List<T> Items { set; get; }
    }
}
