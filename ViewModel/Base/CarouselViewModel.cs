using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Base
{
    public class CarouselViewModel
    {
        public int Id { get; set; }
        public string Href { get; set; }
        public string Source { get; set; }
        public string Alt { get; set; }
        public int SortOrder { get; set; }
    }
}
