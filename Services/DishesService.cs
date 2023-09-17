using CookGPT_MVVM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookGPT_MVVM.Views.Windows;
using Notification.Wpf;
using Microsoft.EntityFrameworkCore;
using CookGPT_MVVM.Models;

namespace CookGPT_MVVM.Services
{
    class DishesService
    {
        private readonly AppDbContext _dbContext;

        public DishesService(AppDbContext dbContext) 
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<Dish[]> GetDishes()
        {
            return await _dbContext.Dishes.ToArrayAsync();
        }

        public async Task AddDish(Dish dish)
        {
            var dishes = await GetDishes();
            foreach (var d in dishes)
            {
                if (d.Name.Trim() == dish.Name.Trim())
                {
                    //MainWindow.notificationManager.Show("Success!", "Dish Already Saved!", NotificationType.Warning, "DishNotificationArea");
                    return;
                }
            }

            _dbContext.Dishes.Add(dish);
            await _dbContext.SaveChangesAsync();
        }

        public Task RemoveDish(Dish dish)
        {
            _dbContext.Dishes.Remove(dish);
            return _dbContext.SaveChangesAsync();
        }

       
    }
}
