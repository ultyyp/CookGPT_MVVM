using System;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using CookGPT_MVVM.Abstractions;

namespace CookGPT_MVVM.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DishWindow : IDishWindow
    {
        public ViewModels.DishWindowViewModel ViewModel
        {
            get;
        }

        public DishWindow(ViewModels.DishWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        
    }
}