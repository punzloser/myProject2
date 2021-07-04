using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Product.DTO.Public
{
    public class ProductPagingRequest : PageRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
