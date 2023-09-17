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
using CookGPT_MVVM.Views.Windows;
using CookGPT_MVVM.Abstractions;
using CommunityToolkit.Mvvm.Messaging;
using CookGPT_MVVM.Messages;
using System.Windows.Media;

namespace CookGPT_MVVM.ViewModels
{
    public partial class DishWindowViewModel : ObservableObject, INavigationAware
    {

        //[ObservableProperty]
        //private Dish _dish;

        [ObservableProperty]
        private ImageSource _dishImage;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _toolsUsed = string.Empty;

        [ObservableProperty]
        private string _timeNeeded = string.Empty;

        [ObservableProperty]
        private string _ingredients = string.Empty;

        [ObservableProperty]
        private string _recipe = string.Empty;

        [ObservableProperty]
        private string _macros = string.Empty;

        private void RegisterMessages()
        {
            WeakReferenceMessenger.Default.Register<DishWindowViewModel, NewDishSelectedMessage>(this, (r, m) =>
            {
                //r.Dish = m.Value;
                r.Name = m.Value.Name;
                r.ToolsUsed = m.Value.ToolsUsed;
                r.TimeNeeded = m.Value.Time;
                r.Ingredients = m.Value.Ingredients;
                r.Recipe = m.Value.Recipe;
                r.Macros = m.Value.Macros;
            });
        }

        public void OnNavigatedTo()
        {
           
        }

        public void OnNavigatedFrom()
        {

        }

        [RelayCommand]
        private void OnWindowLoaded()
        {
            RegisterMessages();
            //var m = WeakReferenceMessenger.Default.Send(new DishRequestMessage());
            //var d = m.Response;

            //Name = d.Name;
            //ToolsUsed = d.ToolsUsed;
            //TimeNeeded = d.Time;
            //Ingredients = d.Ingredients;
            //Recipe = d.Recipe;
            //Macros = d.Macros;
            //DishImage = new ImageSourceConverter().ConvertFromString(d.ImageSource) as ImageSource;



        }

        [RelayCommand]
        private void OnWindowClosing()
        {
            
        }

    }
} 
