using Data;
using Data.Entity;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Orders;

namespace Application.Mail
{
    public class MailService : IMailService
    {
        private readonly MyDBContext _db;

        public MailService(MyDBContext db)
        {
            _db = db;
        }
        //Chưa cấu hình file appsetting riêng
        //private readonly IConfiguration _config;

        //public MailService(IConfiguration config)
        //{
        //    _config = config;
        //}

        public async Task SendEmailAsync(OrderVm mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse("nguyenhoangthanhkutelacloi2021@gmail.com");
            email.To.Add(MailboxAddress.Parse("nguyenhoangthanhkutelacloi2021@gmail.com"));
            email.Subject = "Bạn có đơn mới";

            //var builder = new BodyBuilder();
            //if (mailRequest.Attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in mailRequest.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //        }
            //    }
            //}
            //builder.HtmlBody = mailRequest.Body;
            var mylist = new List<string>();

            for (int i = 0; i < mailRequest.OrderDetail.Count; i++)
            {
                var valueOfProduct = mailRequest.OrderDetail[i];

                mylist.Add($"\rSản phẩm {i + 1}: ".ToString());
                mylist.Add("Tên sản phẩm: " + findIdProduct(valueOfProduct.ProductId, mailRequest.LanguageId));
                mylist.Add("Số lượng: " + valueOfProduct.Quantity.ToString());
                mylist.Add("Giá: " + valueOfProduct.Price.ToString("N0"));
            }

            string productDetail = string.Join("\r\n *** ", mylist);


            email.Body = new TextPart(TextFormat.Text)
            {
                Text =
                "=========THÔNG TIN CHI TIẾT ĐƠN=========" +
                 "\r\nTên người mua: " + mailRequest.ShipName +
                 "\r\nEmail: " + mailRequest.ShipEmail +
                 "\r\nĐịa chỉ: " + mailRequest.ShipAddress +
                 "\r\nPhone: " + mailRequest.ShipPhoneNumber + productDetail
            };

            //Note => using MailKit.Net.Smtp
            using var smtp = new SmtpClient();

            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("nguyenhoangthanhkutelacloi2021@gmail.com", "Kr6=5&uKPj8-ZLXg");

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        private string findIdProduct(int productId, string languageId)
        {
            var name = _db.ProductTranslations
                .Where(a => a.ProductId == productId && a.LanguageId == languageId)
                .Select(a => a.Name).FirstOrDefault();

            return name;
        }
    }
}
