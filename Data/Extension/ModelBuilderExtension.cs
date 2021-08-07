using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Data.Enum;

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
                new Language { Id = "vi", IsDefault = true, Name = "Tiếng Việt" },
                new Language { Id = "en", IsDefault = false, Name = "English" }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, IsShowOnHome = true, ParentId = null, SortOrder = 1, Status = Status.Active },
                new Category { Id = 2, IsShowOnHome = true, ParentId = null, SortOrder = 2, Status = Status.Active }
                );

            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation
                {
                    CategoryId = 1,
                    Id = 1,
                    LanguageId = "vi",
                    Name = "Laptop",
                    SeoAlias = "laptop-thoi-trang",
                    SeoDescription = "Laptop thời trang",
                    SeoTitle = "Laptop thời trang"
                },
                new CategoryTranslation
                {
                    CategoryId = 1,
                    Id = 2,
                    LanguageId = "en",
                    Name = "Laptop",
                    SeoAlias = "fashion-laptop",
                    SeoDescription = "Fashion laptop",
                    SeoTitle = "Fashion laptop"
                },
                new CategoryTranslation
                {
                    CategoryId = 2,
                    Id = 3,
                    LanguageId = "vi",
                    Name = "Mobile",
                    SeoAlias = "dien-thoai-thoi-trang",
                    SeoDescription = "Điện thoại thời trang",
                    SeoTitle = "Điện thoại thời trang"
                },
                new CategoryTranslation
                {
                    CategoryId = 2,
                    Id = 4,
                    LanguageId = "en",
                    Name = "Mobile",
                    SeoAlias = "fashion-mobile",
                    SeoDescription = "Fashion mobile",
                    SeoTitle = "Fashion mobile"
                });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 12000000,
                    Price = 12390000,
                    Stock = 1,
                    ViewCount = 0
                },
                new Product
                {
                    Id = 2,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 11000000,
                    Price = 11490000,
                    Stock = 1,
                    ViewCount = 0
                });

            modelBuilder.Entity<ProductTranslation>().HasData(
                new ProductTranslation
                {
                    Id = 1,
                    Name = "HP 340s G7 i3 1005G1",
                    Description = "240Q4PA",
                    Details = "240Q4PA",
                    LanguageId = "vi",
                    ProductId = 1,
                    SeoAlias = "",
                    SeoDescription = "",
                    SeoTitle = ""
                },
                new ProductTranslation
                {
                    Id = 2,
                    Name = "HP 340s G7 i3 1005G1",
                    Description = "240Q4PA",
                    Details = "240Q4PA",
                    LanguageId = "en",
                    ProductId = 1,
                    SeoAlias = "",
                    SeoDescription = "",
                    SeoTitle = ""
                },
                new ProductTranslation
                {
                    Id = 3,
                    Name = "Lenovo ThinkBook 15IIL i3",
                    Description = "1005G1",
                    Details = "1005G1",
                    LanguageId = "vi",
                    ProductId = 2,
                    SeoAlias = "",
                    SeoDescription = "",
                    SeoTitle = ""
                },
                new ProductTranslation
                {
                    Id = 4,
                    Name = "Lenovo ThinkBook 15IIL i3",
                    Description = "1005G1",
                    Details = "240Q4PA",
                    LanguageId = "en",
                    ProductId = 2,
                    SeoAlias = "",
                    SeoDescription = "",
                    SeoTitle = ""
                });

            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage { Id = 1, Caption = null, DateCreated = DateTime.Now, FileSize = 11, ImagePath = "/img/13.jpg", ImageDetail = "/img/36.jpg", IsDefault = true, SortOrder = 1, ProductId = 1 },
                new ProductImage { Id = 2, Caption = null, DateCreated = DateTime.Now, FileSize = 11, ImagePath = "/img/23.jpg", ImageDetail = "/img/41.jpg", IsDefault = true, SortOrder = 2, ProductId = 2 }
                );

            modelBuilder.Entity<ProductSlide>().HasData(
                new ProductSlide { Id = 1, ImageProductSlide = "/img/33.jpg", ProductId = 1},
                new ProductSlide { Id = 2, ImageProductSlide = "/img/34.jpg", ProductId = 1 },
                new ProductSlide { Id = 3, ImageProductSlide = "/img/35.jpg", ProductId = 1 },
                new ProductSlide { Id = 4, ImageProductSlide = "/img/42.jpg", ProductId = 2 },
                new ProductSlide { Id = 5, ImageProductSlide = "/img/43.jpg", ProductId = 2 },
                new ProductSlide { Id = 6, ImageProductSlide = "/img/44.jpg", ProductId = 2 }
                );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { CategoryID = 1, ProductID = 1 },
                new ProductCategory { CategoryID = 2, ProductID = 2 }
                );

            //create guid auto
            var RoleID_1 = new Guid("70834739-9213-4C00-9936-ED75EAF822D7");
            var RoleID_2 = new Guid("6A0158CF-B5FA-4480-BF08-26BF157FAC36");

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
                });

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = RoleID_1,
                    Name = "admin",
                    Description = "admin",
                    NormalizedName = "admin"
                },
                new Role
                {
                    Id = RoleID_2,
                    Name = "mod",
                    Description = "mod",
                    NormalizedName = "mod"
                });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = RoleID_1,
                    UserId = AdminID
                });

            modelBuilder.Entity<Carousel>().HasData(
                new Carousel
                {
                    Id = 1,
                    Href = "#",
                    Source = "/img/1.png",
                    Alt = "...",
                    SortOrder = 1,
                    Status = Status.Active
                },
                new Carousel
                {
                    Id = 2,
                    Href = "#",
                    Source = "/img/2.png",
                    Alt = "...",
                    SortOrder = 2,
                    Status = Status.Active
                },
                new Carousel
                {
                    Id = 3,
                    Href = "#",
                    Source = "/img/3.png",
                    Alt = "...",
                    SortOrder = 3,
                    Status = Status.Active
                });
        }
    }
}
