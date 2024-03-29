﻿namespace CarDealer
{

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CarDealer.Data;
    using CarDealer.DTOs.Export;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;
    using CarDealer.Utilities;

    public class StartUp
    {
        public static IMapper mapper;
        public static void Main()
        {
            mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            CarDealerContext context = new CarDealerContext();
            //string xml = File.ReadAllText("../../../Datasets/sales.xml");
            string result = GetTotalSalesByCustomer(context);
            Console.WriteLine(result);
        }

        //---------------- Import Data ----------------

        //09. Import Users
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ImportSupplierDto[] supplierDtos =
                xmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

            ICollection<Supplier> validSuppliers = new HashSet<Supplier>();

            foreach (ImportSupplierDto supplierDto in supplierDtos)
            {
                if (supplierDto.Name != null)
                {
                    Supplier supplier = new Supplier()
                    {
                        Name = supplierDto.Name,
                        IsImporter = supplierDto.IsImporter
                    };
                    validSuppliers.Add(supplier);
                }
            }

            context.Suppliers.AddRange(validSuppliers);
            context.SaveChanges();

            return $"Successfully imported {validSuppliers.Count} suppliers!";
        }

        //10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ImportPartDto[] partDtos = xmlHelper.Deserialize<ImportPartDto[]>(inputXml, "Parts");

            ICollection<Part> validParts = new HashSet<Part>();

            foreach (var partDto in partDtos)
            {
                if (partDto.Name != null &&
                    partDto.SupplierId.HasValue &&
                    context.Suppliers.Any(s => s.Id == partDto.SupplierId))
                {
                    Part part = mapper.Map<Part>(partDto);
                    validParts.Add(part);
                }
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count} parts!";
        }

        //11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ImportCarDto[] carDtos =
                xmlHelper.Deserialize<ImportCarDto[]>(inputXml, "Cars");

            ICollection<Car> validCars = new HashSet<Car>();

            foreach (ImportCarDto carDto in carDtos)
            {
                if (!string.IsNullOrEmpty(carDto.Make) &&
                    !string.IsNullOrEmpty(carDto.Model))
                {
                    Car car = mapper.Map<Car>(carDto);

                    foreach (var partDto in carDto.Parts.DistinctBy(p => p.PartId))
                    {
                        if (context.Parts.Any(p => p.Id == partDto.PartId))
                        {
                            PartCar carPart = new PartCar()
                            {
                                PartId = partDto.PartId
                            };
                            car.PartsCars.Add(carPart);
                        }
                    }

                    validCars.Add(car);
                }
            }

            context.Cars.AddRange(validCars);
            context.SaveChanges();

            return $"Successfully imported {validCars.Count} cars!";
        }

        //12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ImportCustomerDto[] customerDtos = xmlHelper.Deserialize<ImportCustomerDto[]>(inputXml, "Customers");

            ICollection<Customer> validCustomers = new HashSet<Customer>();

            foreach (ImportCustomerDto customerDto in customerDtos)
            {
                if (!string.IsNullOrEmpty(customerDto.Name))
                {
                    validCustomers.Add(mapper.Map<Customer>(customerDto));
                }
            }
            context.Customers.AddRange(validCustomers);
            context.SaveChanges();

            return $"Successfully imported {validCustomers.Count} customers!";
        }

        //13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ImportSaleDto[] saleDtos = xmlHelper.Deserialize<ImportSaleDto[]>(inputXml, "Sales");

            ICollection<Sale> validSales = new HashSet<Sale>();

            foreach (ImportSaleDto saleDto in saleDtos)
            {

                if (context.Cars.Any(c => c.Id == saleDto.CarId) &&
                    context.Customers.Any(c => c.Id == saleDto.CustomerId))
                {
                    validSales.Add(mapper.Map<Sale>(saleDto));
                }
            }

            context.Sales.AddRange(validSales);
            context.SaveChanges();

            return $"Successfully imported {validSales.Count} sales";
        }

        //---------------- Query and Export Data -----------------

        //14. Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportCarDto[] cars = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<ExportCarDto>(mapper.ConfigurationProvider)
                .ToArray();


            return xmlHelper.Serialize<ExportCarDto[]>(cars, "cars");
        }

        //15. Export Cars from Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportCarBmwDto[] bmwCarsDto = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ProjectTo<ExportCarBmwDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportCarBmwDto[]>(bmwCarsDto, "cars");
        }

        //16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportLocalSupplierDto[] localSupplierDto = context
                .Suppliers
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportLocalSupplierDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportLocalSupplierDto>(localSupplierDto, "suppliers");
        }

        //17. Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportCarWithPartsDto[] exportCarWithPartsDtos = context
                .Cars
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ProjectTo<ExportCarWithPartsDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportCarWithPartsDto>(exportCarWithPartsDtos, "cars");
        }

        //18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportCustomerDto[] exportCustomerDto = context
                .Customers
                .Where(c => c.Sales.Count > 0)
                .ProjectTo<ExportCustomerDto>(mapper.ConfigurationProvider)
                .OrderByDescending(c => c.SpendMoney)
                .ToArray();

            return xmlHelper.Serialize<ExportCustomerDto>(exportCustomerDto, "customers");

        }

        //19. Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportSaleWithAppliedDiscount[] exportSaleWithAppliedDiscounts = context
                .Sales
                .ProjectTo<ExportSaleWithAppliedDiscount>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportSaleWithAppliedDiscount>(exportSaleWithAppliedDiscounts, "sales");
        }

    }
}