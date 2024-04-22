using FS20240422.Models;

namespace FS20240422.Data
{
    internal static class FsDbHelper
    {
        private static List<Customer> customers;
        private static List<Order> orders;

        static FsDbHelper()
        {
            customers = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Name = "Sue",
                    City = "NBI"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Joe",
                    City = "MSA"
                },
                new Customer
                {
                    Id = 3,
                    Name = "Luc",
                    City = "NBI"
                }
            };

            orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    OrderDate = new DateTimeOffset(new DateTime(2024, 4, 7)),
                    Amount = 190,
                    Customer = customers[1]
                },
                new Order
                {
                    Id = 2,
                    OrderDate = new DateTimeOffset(new DateTime(2024, 4, 3)),
                    Amount = 130,
                    Customer = customers[0]
                },
                new Order
                {
                    Id = 3,
                    OrderDate = new DateTimeOffset(new DateTime(2024, 4, 13)),
                    Amount = 50,
                    Customer = customers[0]
                },
                new Order
                {
                    Id = 4,
                    OrderDate = new DateTimeOffset(new DateTime(2024, 4, 17)),
                    Amount = 110,
                    Customer = customers[1]
                },
                new Order
                {
                    Id = 5,
                    OrderDate = new DateTimeOffset(new DateTime(2024, 4, 5)),
                    Amount = 70,
                    Customer = customers[0]
                }
            };
        }

        public static void SeedDb(FsDbContext db)
        {
            if (!db.Customers.Any())
            {
                db.Customers.AddRange(customers);
            }

            if (!db.Orders.Any())
            {
                db.Orders.AddRange(orders);
            }

            db.SaveChanges();
        }
    }
}
