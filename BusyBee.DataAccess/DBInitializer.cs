using BusyBee.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusyBee.DataAccess
{
    public static class DBInitializer
    {
        public static void Initialize(BusyBeeDBContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any()) return; // DB already seeded

            var user1 = new User { Id = Guid.NewGuid().ToString(), FullName = "Олексій Іванов", Email = "a@example.com", EmailConfirmed = true };
            var user2 = new User { Id = Guid.NewGuid().ToString(), FullName = "Марія Смирнова", Email = "m@example.com", EmailConfirmed = true };
            var user3 = new User { Id = Guid.NewGuid().ToString(), FullName = "Ігор Петренко", Email = "i@example.com", EmailConfirmed = true };
            var user4 = new User { Id = Guid.NewGuid().ToString(), FullName = "Світлана Орлова", Email = "s@example.com", EmailConfirmed = true };

            var specialist1 = new Specialist { User = user1, LastActivityTime = DateTime.UtcNow.AddDays(-2) };
            var specialist2 = new Specialist { User = user3, LastActivityTime = DateTime.UtcNow.AddDays(-1) };

            var customer1 = new Customer { User = user2, LastActivityTime = DateTime.UtcNow.AddDays(-3) };
            var customer2 = new Customer { User = user4, LastActivityTime = DateTime.UtcNow.AddDays(-5) };

            var category1 = new WorkCategory { Name = "Сантехніка" };
            var category2 = new WorkCategory { Name = "Електрика" };

            var work1 = new Work { Name = "Встановлення раковини", Description = "Монтаж нової раковини", WorkCategory = category1 };
            var work2 = new Work { Name = "Прокладання проводки", Description = "Прокладка електропроводки", WorkCategory = category2 };
            var work3 = new Work { Name = "Ремонт розетки", Description = "Заміна або ремонт розетки", WorkCategory = category2 };

            var city1 = new City { Name = "Вся Україна", Region = "Вся Україна", IsActive = true };
            var cities = new List<City>
            {
                new City { Name = "Вінниця", Region = "Вінницька", IsActive = true },
                new City { Name = "Дніпро", Region = "Дніпропетровська", IsActive = true },
                new City { Name = "Донецьк", Region = "Донецька", IsActive = true },
                new City { Name = "Житомир", Region = "Житомирська", IsActive = true },
                new City { Name = "Запоріжжя", Region = "Запорізька", IsActive = true },
                new City { Name = "Івано-Франківськ", Region = "Івано-Франківська", IsActive = true },
                new City { Name = "Київ", Region = "Київська", IsActive = true },
                new City { Name = "Кропивницький", Region = "Кіровоградська", IsActive = true },
                new City { Name = "Луганськ", Region = "Луганська", IsActive = true },
                new City { Name = "Львів", Region = "Львівська", IsActive = true },
                new City { Name = "Миколаїв", Region = "Миколаївська", IsActive = true },
                new City { Name = "Одеса", Region = "Одеська", IsActive = true },
                new City { Name = "Полтава", Region = "Полтавська", IsActive = true },
                new City { Name = "Рівне", Region = "Рівненська", IsActive = true },
                new City { Name = "Суми", Region = "Сумська", IsActive = true },
                new City { Name = "Тернопіль", Region = "Тернопільська", IsActive = true },
                new City { Name = "Ужгород", Region = "Закарпатська", IsActive = true },
                new City { Name = "Харків", Region = "Харківська", IsActive = true },
                new City { Name = "Херсон", Region = "Херсонська", IsActive = true },
                new City { Name = "Хмельницький", Region = "Хмельницька", IsActive = true },
                new City { Name = "Черкаси", Region = "Черкаська", IsActive = true },
                new City { Name = "Чернівці", Region = "Чернівецька", IsActive = true },
                new City { Name = "Чернігів", Region = "Чернігівська", IsActive = true }
            };

            var offer1 = new Offer
            {
                Price = 1200,
                Description = "Встановлення раковини",
                PlannedDuration = DateTime.UtcNow.AddDays(2),
                Work = work1,
                Specialist = specialist1,
                Cities = new List<City> { city1, cities[1] }
            };

            var offer2 = new Offer
            {
                Price = 2500,
                Description = "Полная замена проводки в квартире",
                PlannedDuration = DateTime.UtcNow.AddDays(7),
                Work = work2,
                Specialist = specialist2,
                Cities = new List<City> { cities[1] }
            };

            var offer3 = new Offer
            {
                Price = 700,
                Description = "Ремонт розетки",
                PlannedDuration = DateTime.UtcNow.AddDays(1),
                Work = work3,
                Specialist = specialist2,
                Cities = new List<City> { cities[3] }
            };

            var order1 = new Order
            {
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                CompletedAt = DateTime.UtcNow.AddDays(-5),
                Description = "Термінова установка",
                Price = 1200,
                Customer = customer1,
                Specialist = specialist1,
                Work = work1
            };

            var order2 = new Order
            {
                CreatedAt = DateTime.UtcNow.AddDays(-8),
                CompletedAt = DateTime.UtcNow.AddDays(-2),
                Description = "Ремонт електропроводки",
                Price = 2500,
                Customer = customer2,
                Specialist = specialist2,
                Work = work2
            };

            var payment1 = new Payment
            {
                Id = Guid.NewGuid(),
                Amount = 1200,
                Currency = "UAH",
                Status = "Completed",
                PaymentProvider = "Stripe",
                ExternalPaymentId = "stripe123",
                Description = "Сплата за сантехнічні роботи",
                CreatedAt = DateTime.UtcNow.AddDays(-9),
                CompletedAt = DateTime.UtcNow.AddDays(-5),
                Customer = customer1,
                Order = order1
            };

            var payment2 = new Payment
            {
                Id = Guid.NewGuid(),
                Amount = 2500,
                Currency = "UAH",
                Status = "Completed",
                PaymentProvider = "PayPal",
                ExternalPaymentId = "paypal456",
                Description = "Сплата за електромонтаж",
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                CompletedAt = DateTime.UtcNow.AddDays(-2),
                Customer = customer2,
                Order = order2
            };

            var feedback1 = new Feedback
            {
                Comment = "Чудова робоота!",
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                Rating = 5,
                Work = work1,
                Specialist = specialist1
            };

            var feedback2 = new Feedback
            {
                Comment = "Якісно та швидко",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Rating = 4,
                Work = work2,
                Specialist = specialist2
            };

            context.Users.AddRange(user1, user2, user3, user4);
            context.Specialists.AddRange(specialist1, specialist2);
            context.Customers.AddRange(customer1, customer2);
            context.WorkCategories.AddRange(category1, category2);
            context.Works.AddRange(work1, work2, work3);
            context.Cities.AddRange(city1);
            Thread.Sleep(1000); 
            context.Cities.AddRange(cities);
            context.Offers.AddRange(offer1, offer2, offer3);
            context.Orders.AddRange(order1, order2);
            context.Payments.AddRange(payment1, payment2);
            context.Feedbacks.AddRange(feedback1, feedback2);

            context.SaveChanges();
        }
    }
}