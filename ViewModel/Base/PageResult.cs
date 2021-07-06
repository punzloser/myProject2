using System.Collections.Generic;

namespace ViewModel.Base
{
    public class PageResult<T>
    {
        public List<T> Items { set; get; }
        public int ToTalRecord { get; set; }
    }
}
