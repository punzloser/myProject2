using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Catalog.Orders
{
    public class OrderVm
    {
        [Display(Name = "A/Chị")]
        [Required(ErrorMessage = "Tên trống")]
        public string ShipName { set; get; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Địa chỉ trống")]
        public string ShipAddress { set; get; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email trống")]
        public string ShipEmail { set; get; }
        [Display(Name = "Số ĐT")]
        [Required(ErrorMessage = "SĐT trống")]
        public string ShipPhoneNumber { set; get; }
        public string LanguageId { get; set; }
        public List<OrderDetailVm> OrderDetail { get; set; } = new List<OrderDetailVm>();
    }
}
