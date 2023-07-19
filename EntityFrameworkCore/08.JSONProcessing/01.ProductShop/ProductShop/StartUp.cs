﻿namespace ProductShop
{

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using ProductShop.Data;
    using ProductShop.DTOs.Export;
    using ProductShop.DTOs.Import;
    using ProductShop.Models;

    public class StartUp
    {
        private static IMapper mapper;
        private static IContractResolver contractResolver;
        public static void Main()
        {
            mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));

            contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(false, true)
            };

            ProductShopContext context = new ProductShopContext();


            string result = GetSoldProducts(context);

            Console.WriteLine(result);
        }
        //----------------------- Import Data ------------------------------

        //01. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            ICollection<User> validUsers = new HashSet<User>();

            foreach (ImportUserDto userDto in userDtos)
            {
                User user = mapper.Map<User>(userDto);
                validUsers.Add(user);
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count} users";
        }

        //02. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

            ICollection<Product> products = mapper.Map<Product[]>(productDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count} products";
        }

        //03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            ImportCategoryDto[] categoryDtos =
                JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);

            ICollection<Category> validCategories = new HashSet<Category>();

            foreach (ImportCategoryDto categoryDto in categoryDtos)
            {
                if (categoryDto.Name != null)
                {
                    validCategories.Add(mapper.Map<Category>(categoryDto));
                }
            }

            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count} categories";
        }

        //04. Import Categories and Products
        public static string ImportCategoryProduct(ProductShopContext context, string inputJson)
        {
            ImportCategoryProductDto[] categoryProductDtos =
                JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson);

            ICollection<CategoryProduct> validEntities = new HashSet<CategoryProduct>();

            foreach (var categoryProductDto in categoryProductDtos)
            {
                if (context.Categories.Any(c => c.Id == categoryProductDto.CategoryId) &&
                    context.Products.Any(p => p.Id == categoryProductDto.ProductId))
                {
                    validEntities.Add(mapper.Map<CategoryProduct>(categoryProductDto));
                }
            }

            context.CategoriesProducts.AddRange(validEntities);
            context.SaveChanges();

            return $"Successfully imported {validEntities.Count} categoryProducts";
        }

        //------------------------ Export Data ------------------------------

        //5. Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            ExportProductInRangeDto[] productDtos = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .AsNoTracking()
                .ProjectTo<ExportProductInRangeDto>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(productDtos, Formatting.Indented);
        }

        //6. Export Sold Products
        public static string GetSoldProducts(ProductShopContext ctx)
        {
            var usersWithSoldProducts = ctx.Users
                //.Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(p => new
                    {
                        p.Name,
                        p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName
                    })
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(usersWithSoldProducts,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = contractResolver
                });
        }
    }
}