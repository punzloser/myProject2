using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entity
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public DateTime DateCreated { get; set; }
        public long FileSize { get; set; }
        public string ImagePath { get; set; }
        public string ImageDetail { get; set; }
        public bool IsDefault { get; set; }
        public int SortOrder { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
