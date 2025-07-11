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

            var user1 = new User { Id = Guid.NewGuid().ToString(), FullName = "Алексей Иванов", Email = "a@example.com", EmailConfirmed = true };
            var user2 = new User { Id = Guid.NewGuid().ToString(), FullName = "Мария Смирнова", Email = "m@example.com", EmailConfirmed = true };
            var user3 = new User { Id = Guid.NewGuid().ToString(), FullName = "Игорь Петренко", Email = "i@example.com", EmailConfirmed = true };
            var user4 = new User { Id = Guid.NewGuid().ToString(), FullName = "Светлана Орлова", Email = "s@example.com", EmailConfirmed = true };

            var specialist1 = new Specialist { User = user1, LastActivityTime = DateTime.UtcNow.AddDays(-2) };
            var specialist2 = new Specialist { User = user3, LastActivityTime = DateTime.UtcNow.AddDays(-1) };

            var customer1 = new Customer { User = user2, LastActivityTime = DateTime.UtcNow.AddDays(-3) };
            var customer2 = new Customer { User = user4, LastActivityTime = DateTime.UtcNow.AddDays(-5) };

            var category1 = new WorkCategory { Name = "Сантехника" };
            var category2 = new WorkCategory { Name = "Электрика" };

            var work1 = new Work { Name = "Установка раковины", Description = "Установка новой раковины", WorkCategory = category1 };
            var work2 = new Work { Name = "Монтаж проводки", Description = "Прокладка электропроводки", WorkCategory = category2 };
            var work3 = new Work { Name = "Ремонт розетки", Description = "Замена розетки", WorkCategory = category2 };

            var city1 = new City { Name = "Киев", Region = "Киевская", IsActive = true };
            var city2 = new City { Name = "Харьков", Region = "Харьковская", IsActive = true };
            var city3 = new City { Name = "Львов", Region = "Львовская", IsActive = true };

            var offer1 = new Offer
            {
                Price = 1200,
                Description = "Установка раковины в ванной",
                PlannedDuration = DateTime.UtcNow.AddDays(2),
                Work = work1,
                Specialist = specialist1,
                Cities = new List<City> { city1, city2 }
            };

            var offer2 = new Offer
            {
                Price = 2500,
                Description = "Полная замена проводки в квартире",
                PlannedDuration = DateTime.UtcNow.AddDays(7),
                Work = work2,
                Specialist = specialist2,
                Cities = new List<City> { city1, city3 }
            };

            var offer3 = new Offer
            {
                Price = 700,
                Description = "Ремонт розетки в комнате",
                PlannedDuration = DateTime.UtcNow.AddDays(1),
                Work = work3,
                Specialist = specialist2,
                Cities = new List<City> { city2 }
            };

            var order1 = new Order
            {
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                CompletedAt = DateTime.UtcNow.AddDays(-5),
                Description = "Срочная установка",
                Price = 1200,
                Customer = customer1,
                Specialist = specialist1,
                Work = work1
            };

            var order2 = new Order
            {
                CreatedAt = DateTime.UtcNow.AddDays(-8),
                CompletedAt = DateTime.UtcNow.AddDays(-2),
                Description = "Ремонт электропроводки",
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
                Description = "Оплата за сантехнические работы",
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
                Description = "Оплата за электромонтаж",
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                CompletedAt = DateTime.UtcNow.AddDays(-2),
                Customer = customer2,
                Order = order2
            };

            var feedback1 = new Feedback
            {
                Comment = "Отличная работа!",
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                Rating = 5,
                Work = work1,
                Specialist = specialist1
            };

            var feedback2 = new Feedback
            {
                Comment = "Быстро и качественно",
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
            context.Cities.AddRange(city1, city2, city3);
            context.Offers.AddRange(offer1, offer2, offer3);
            context.Orders.AddRange(order1, order2);
            context.Payments.AddRange(payment1, payment2);
            context.Feedbacks.AddRange(feedback1, feedback2);

            context.SaveChanges();
        }
    }
}