using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wpf.Ui.Common.Interfaces;
using CookGPT_MVVM.Data;
using CookGPT_MVVM.Models;
using CookGPT_MVVM.Services;
using System.Threading.Tasks;
using Notification.Wpf;
using System.Windows.Controls;
using System.Windows;
using System;
using CookGPT_MVVM.Views.Pages;
using CookGPT_MVVM.Abstractions;
using CommunityToolkit.Mvvm.Messaging;
using CookGPT_MVVM.Messages;
using CookGPT_MVVM.Views.Windows;
using System.Windows.Input;

namespace CookGPT_MVVM.ViewModels
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        private readonly IDishWindow dishWindow;

        private NotificationManager _notificationManager = new NotificationManager();

        [ObservableProperty]
        private ObservableCollection<Dish> _dishes = TestDishCollection();


        public DashboardViewModel(IDishWindow dishWindow)
        {
            this.dishWindow = dishWindow ?? throw new ArgumentNullException(nameof(dishWindow));
            RegisterMessages();
        }

        [ObservableProperty]
        private Dish _selectedDish;

        private void RegisterMessages()
        {
            WeakReferenceMessenger.Default.Register<DishRequestMessage>(this, (r, m) =>
            {
                m.Reply(SelectedDish);
            });
        }


        //Testing
        private static ObservableCollection<Dish> TestDishCollection()
        {
            ObservableCollection<Dish> TestCollection = new ObservableCollection<Dish>();
            Dish dish = new Dish();
            dish.Name = "Creamy Carrot Soup";
            dish.ToolsUsed = "Stove top, Microwave";
            dish.Ingredients = "- 100g of butter\r\n- 100 ml of milk\r\n- 500 g of carrots";
            dish.Time = "30 minutes";
            dish.Recipe = "1. Peel and chop the carrots into small pieces.\r\n2. In a pot on the stove top, melt the butter over medium heat.\r\n3. Add the chopped carrots to the pot and stir to coat them with butter.\r\n4. Cover the pot and let the carrots cook for 10 minutes, stirring occasionally.\r\n5. After 10 minutes, add enough water to cover the carrots and bring to a boil.\r\n6. Reduce the heat and let the carrots simmer for another 10 minutes until they are soft.\r\n7. Remove from heat and let the mixture cool for a few minutes.\r\n8. Transfer the mixture to a microwave-safe bowl and add the milk.\r\n9. Microwave on high for 2-3 minutes until the mixture is hot.\r\n10. Use an immersion blender to blend the mixture until it is smooth and creamy.\r\n11. Serve hot.";
            dish.Macros = "Calories: 240\r\nFat: 22g\r\nCarbs: 10g\r\nProtein: 2g";
            dish.ImageSource = "pack://application:,,,/Assets/3icon.png";
            dish.ImageLink = "data:image/jpeg;base64,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";
            TestCollection.Add(dish);
            return TestCollection;
        }


        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        private async void OnGenerateDishes()
        {
            ButtonContent = "Cooking...";
            ButtonEnabled = false;
            if(!CanCook())
            {
                ButtonContent = "Cook!";
                ButtonEnabled = true;
                return;
            }
            QueriesService qs = new QueriesService(_recipe, GetUsedTools(), GetUnusedTools(), _time);
            Dishes = await qs.GenerateDishes();
            ButtonContent = "Cook!";
            ButtonEnabled = true;
        }

        [RelayCommand]
        private void OnOpenDishWindow()
        {
            var m = WeakReferenceMessenger.Default.Send(new SelectedDishRequestMessage());
            var d = m.Response;
            _selectedDish = d;

            WeakReferenceMessenger.Default.Send(new NewDishSelectedMessage(d));
            dishWindow.ShowDialog();
        }

        [RelayCommand]
        private void OnIngredientsGotFocus()
        {
            if (Recipe == "Example: 100g of Butter, 200g of Milk.")
            {
                Recipe = "";
            }
        }

        [RelayCommand]
        private void OnIngredientsLostFocus()
        {
            if (string.IsNullOrWhiteSpace(Recipe))
            {
                Recipe = "Example: 100g of Butter, 200g of Milk.";
            }
        }

        //Parameter Bindings
        [ObservableProperty]
        private bool _buttonEnabled = true;

        [ObservableProperty]
        private string _buttonContent = "Cook!";

        //Bindings From View
        [ObservableProperty]
        private string _recipe = "Example: 100g of Butter, 200g of Milk.";

        [ObservableProperty]
        private int _time;

        [ObservableProperty]
        private bool _ovenChecked;

        [ObservableProperty]
        private bool _microwaveChecked;

        [ObservableProperty]
        private bool _blenderChecked;

        [ObservableProperty]
        private bool _stoveChecked;

        [ObservableProperty]
        private bool _bBQChecked;

        [ObservableProperty]
        private bool _airFryerChecked;

        [ObservableProperty]
        private bool _fPChecked;

        private string GetUsedTools()
        {
            string tools = string.Empty;
            if(_ovenChecked) { tools += "Oven, "; }
            if (_microwaveChecked) { tools += "Microwave, "; }
            if (_blenderChecked) { tools += "Blender, "; }
            if (_stoveChecked) { tools += "Stove, "; }
            if (_bBQChecked) { tools += "BBQ, "; }
            if (_airFryerChecked) { tools += "Air Fryer, "; }
            if (_fPChecked) { tools += "Food processor, "; }

            if(tools != string.Empty)
            {
                tools = tools.Remove(tools.LastIndexOf(','));
                tools += ".";
                return tools;
            }
            else
            {
                return string.Empty;
            }
            
        }
        private string GetUnusedTools()
        {
            string tools = string.Empty;
            if (!_ovenChecked) { tools += "Oven, "; }
            if (!_microwaveChecked) { tools += "Microwave, "; }
            if (!_blenderChecked) { tools += "Blender, "; }
            if (!_stoveChecked) { tools += "Stove, "; }
            if (!_bBQChecked) { tools += "BBQ, "; }
            if (!_airFryerChecked) { tools += "Air Fryer, "; }
            if (!_fPChecked) { tools += "Food processor, "; }

            if (tools != string.Empty)
            {
                tools = tools.Remove(tools.LastIndexOf(','));
                tools += ".";
                return tools;
            }
            else
            {
                return string.Empty;
            }
        }

        
        private bool CanCook()
        {
            bool CanCook = true;

            if (Recipe.Length == 0 || Recipe == null ||
                Recipe == "Example: 100g of Butter, 200g of Milk.")
            {
                _notificationManager.Show("Error", "Please type valid ingredients!", NotificationType.Warning, "NotificationArea");
                CanCook = false;
            }
            if (Time == 0)
            {
                _notificationManager.Show("Error", "Please enter a valid time!", NotificationType.Warning, "NotificationArea");
                CanCook = false;
            }
            if (GetUsedTools() == string.Empty)
            {
                _notificationManager.Show("Error", "Please select some tools!", NotificationType.Warning, "NotificationArea");
                CanCook = false;
            }

            if (!CanCook)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }



    

    }
}
