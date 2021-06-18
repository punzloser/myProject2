using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extension
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
                new AppConfig { Key = "Home", Value = "This is home page of website" },
                new AppConfig { Key = "Keyword", Value = "This is keyword of website" },
                new AppConfig { Key = "Description", Value = "This is description of webpage" }
                );

            modelBuilder.Entity<Language>().HasData(
                new Language { Id = "vi-VN", IsDefault = true, Name = "Tiếng Việt" },
                new Language { Id = "en-US", IsDefault = false, Name = "English" }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, IsShowOnHome = true, ParentId = null, SortOrder = 1, Status = Enum.Status.Active },
                new Category { Id = 2, IsShowOnHome = true, ParentId = null, SortOrder = 2, Status = Enum.Status.Active }
                );

            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation
                {
                    CategoryId = 1,
                    Id = 1,
                    LanguageId = "vi-VN",
                    Name = "Áo nam",
                    SeoAlias = "ao-nam",
                    SeoDescription = "áo thời trang nam",
                    SeoTitle = "áo thời trang nam"
                },
                new CategoryTranslation
                {
                    CategoryId = 1,
                    Id = 2,
                    LanguageId = "en-US",
                    Name = "men t-shirt",
                    SeoAlias = "men-t-shirt",
                    SeoDescription = "men fashion t-shirt",
                    SeoTitle = "men fashion t-shirt"
                },
                new CategoryTranslation
                {
                    CategoryId = 2,
                    Id = 3,
                    LanguageId = "vi-VN",
                    Name = "Áo nữ",
                    SeoAlias = "ao-nu",
                    SeoDescription = "áo thời trang nữ",
                    SeoTitle = "áo thời trang nữ"
                },
                new CategoryTranslation
                {
                    CategoryId = 2,
                    Id = 4,
                    LanguageId = "en-US",
                    Name = "women t-shirt",
                    SeoAlias = "women-t-shirt",
                    SeoDescription = "women fasion t-shirt",
                    SeoTitle = "women fasion t-shirt"
                }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 200000,
                    Price = 250000,
                    Stock = 0,
                    ViewCount = 0
                }
                );

            modelBuilder.Entity<ProductTranslation>().HasData(
                new ProductTranslation
                {
                    Id = 1,
                    Name = "Áo sơ mi trắng nam đẹp",
                    Description = "Áo sơ mi trắng nam đẹp",
                    Details = "Áo sơ mi trắng nam đẹp",
                    LanguageId = "vi-VN",
                    ProductId = 1,
                    SeoAlias = "ao-so-mi-nam-trang-dep",
                    SeoDescription = "Áo sơ mi trắng nam đẹp",
                    SeoTitle = "Áo sơ mi trắng nam đẹp"
                },
                new ProductTranslation
                {
                    Id = 2,
                    Name = "Nice white men t-shirt",
                    Description = "Nice white men t-shirt",
                    Details = "Nice white men t-shirt",
                    LanguageId = "en-US",
                    ProductId = 1,
                    SeoAlias = "ao-so-mi-nam-trang-dep",
                    SeoDescription = "Nice white men t-shirt",
                    SeoTitle = "Nice white men t-shirt",
                }
                );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { CategoryID = 1, ProductID = 1 }
                );

            //create guid auto
            var RoleID = new Guid("70834739-9213-4C00-9936-ED75EAF822D7");
            var AdminID = new Guid("48C2B994-33AB-439B-9D6F-A5318916AFF6");
            var hash = new PasswordHasher<User>();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = AdminID,
                    UserName = "admin",
                    NormalizedUserName = "admin",
                    FirstName = "Thanh",
                    LastName = "Nguyen",
                    Email = "punzloser@gmail.com",
                    NormalizedEmail = "punzloser@gmail.com",
                    EmailConfirmed = true,
                    Dob = new DateTime(1995, 01, 25),
                    SecurityStamp = string.Empty,
                    PasswordHash = hash.HashPassword(null, "123456a@")
                }
                );

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = RoleID,
                    Name = "administrator",
                    Description = "administration",
                    NormalizedName = "admin"
                }
                );

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = RoleID,
                    UserId = AdminID
                }
                );
        }
    }
}
