using CookGPT_MVVM.Models;
using CookGPT_MVVM.Views.Windows;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CookGPT_MVVM.Services
{
    public class QueriesService
    {
        private OpenAIService _openAIService = new OpenAIService();
        private string _ingredients;
        private string _usedTools;
        private string _unusedTools;
        private int _time;

        public QueriesService(string ingredients, string usedTools, string unusedTools, int time)
        {
            _ingredients = ingredients;
            _usedTools = usedTools;
            _unusedTools = unusedTools;
            _time = time;
        }

        public async Task<ObservableCollection<Dish>> GenerateDishes()
        {
            var response = await _openAIService.SendGPTRequest(
                    $"Tell me the names of 5 dishes  that follow these three criteria:" +
                    $"1- The recipe contains only these ingredients: {_ingredients}" +
                    $"2- The dishes can be made using these kitchen tools: {_usedTools}" +
                    $"3- The dishes don't use these kitchen tools: {_unusedTools}" +
                    $"4- They can be made in: {_time}"
                );

            var names = Regex.Split(response, "\r\n|\r|\n");
            names = names.Skip(2).ToArray();
            var dishes = new ObservableCollection<Dish>();

            await Parallel.ForEachAsync(names, async (name, cancellationToken) =>
            {
                var finalName = Regex.Replace(name, @"[\d-]", string.Empty);
                finalName = finalName.Replace(".", "");
                var dish = new Dish
                {
                    Name = finalName.Trim(),
                    ImageLink = "pack://application:,,,/Assets/loadingicon.png"
                };

                await Task.Run(async () => { await GenerateDishRecipe(dish, _ingredients, _usedTools, _unusedTools, _time); ; });

                var title = Regex.Match(dish.Recipe, @"(?<=Title:\s+)(.+?)(?=Tools)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                var tools = Regex.Match(dish.Recipe, @"(?<=needed:\s+)(.+?)(?=Ingredients)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace); ;
                var ingredients = Regex.Match(dish.Recipe, @"(?<=used:\s+)(.+?)(?=Time)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace); ;
                var time = Regex.Match(dish.Recipe, @"(?<=Time:\s+)(.+?)(?=Recipe)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                var recipe = Regex.Match(dish.Recipe, @"(?<=instructions:\s+)(.+?)(?=Macros)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace); ;
                var macros = Regex.Match(dish.Recipe, @"(?<=Macros:\s+)(.+?)(?=$)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace); ;


                dish.Id = Guid.NewGuid();
                dish.Name = title.Groups[0].Value;
                dish.ToolsUsed = tools.Groups[0].Value;
                dish.Ingredients = ingredients.Groups[0].Value;
                dish.Time = time.Groups[0].Value;
                dish.Recipe = recipe.Groups[0].Value;
                dish.Macros = macros.Groups[0].Value;

                if (dish.Name.Length > 0 && dish.ToolsUsed.Length > 0 && dish.Ingredients.Length > 0
                    && dish.Time.Length > 0 && dish.Recipe.Length > 0 && dish.Macros.Length > 0)
                {
                    await Task.Run(async () => { await GenerateDishImage(dish); });
                }

                if (dish.Name.Length > 0 && dish.ToolsUsed.Length > 0 && dish.Ingredients.Length > 0
                    && dish.Time.Length > 0 && dish.Recipe.Length > 0 && dish.Macros.Length > 0
                        && dish.ImageLink != "pack://application:,,,/Assets/loadingicon.png")
                {
                    dishes.Add(dish);
                }

            });

            return dishes;
        }

        public async Task GenerateDishImage(Dish dish)
        {
            dish.ImageLink = await _openAIService.SendDALLERequest(dish.Name);
        }

        public async Task GenerateDishRecipe(Dish dish, string ingredients, string usedTools, string unusedTools, int time)
        {
            try
            {
                var responseEN = await _openAIService.SendGPTRequest(
                    $"Tell me a short recipe for {dish.Name} in the following format: Recipe Title, Tools needed, " +
                    $"Ingredients used, Time, Recipe instructions, Macros. That follows these criteria:" +
                    $"1 - It is made using only these ingredients: {ingredients}" +
                    $"2 - It is made using only these kitchen tools: {usedTools}" +
                    $"3 - It doesn't use these kitchen tools: {unusedTools}" +
                    $"4 - It is made in: {time} minutes"
                    );

                dish.Recipe = responseEN;
            }
            catch
            {
                //MainWindow.notificationManager.Show("OpenAI", "OpenAI Internal Error!", NotificationType.Error, "NotificationArea");
            }

        }
    }
}
