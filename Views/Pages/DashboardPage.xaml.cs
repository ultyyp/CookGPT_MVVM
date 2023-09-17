using CommunityToolkit.Mvvm.Messaging;
using CookGPT_MVVM.Messages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using CookGPT_MVVM.Models;
using System.Windows.Input;
using CookGPT_MVVM.Views.Windows;

namespace CookGPT_MVVM.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : INavigableView<ViewModels.DashboardViewModel>
    {
        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }

        public DashboardPage(ViewModels.DashboardViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
            RegisterMessages();
        }

        private void RegisterMessages()
        {
            
            WeakReferenceMessenger.Default.Register<SelectedDishRequestMessage>(this, (r, m) =>
            {
                m.Reply(_selectedDish);
            });
        }

        private Dish _selectedDish;

        private void ListViewRecipes_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource is Border)
            {
                Border border = e.OriginalSource as Border;
                Dish dish = border.DataContext as Dish;
                _selectedDish = dish;
                
            }
            else if (e.OriginalSource is TextBlock)
            {
                TextBlock tb = e.OriginalSource as TextBlock;
                Dish dish = tb.DataContext as Dish;
                _selectedDish = dish;
            }
        }

        

        
    }
}