﻿using MoneySplitter.Models.App;
using MoneySplitter.Win10.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneySplitter.Win10.Views
{
	public sealed partial class HomeView
	{
		public HomeViewModel ViewModel { get; set; }

		public HomeView()
		{
			InitializeComponent();

			DataContextChanged += (s, e) =>
			{
				ViewModel = DataContext as HomeViewModel;
			};
		}

		private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			ViewModel.RemoveNotification(button.DataContext as NotificationModel);
		}

		private void OnEditButtonClick(object sender, RoutedEventArgs e)
		{
			ViewModel.NavigateToSettings();
		}

		private void OnHistoryButtonClick(object sender, RoutedEventArgs e)
		{
			ViewModel.NavigateToHistory();
		}
	}
}
