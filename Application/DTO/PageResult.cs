using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PageResult<T>
    {
        public List<T> Items { set; get; }
        public int ToTalRecord { get; set; }
    }
}
