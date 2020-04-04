using Windows.UI.Xaml;
using MoneySplitter.Win10.ViewModels;

namespace MoneySplitter.Win10.Views
{
	public sealed partial class HistoryView
	{
		public HistoryViewModel ViewModel { get; private set; }

		public HistoryView()
		{
			InitializeComponent();

			DataContextChanged += OnDataContextChanged;
		}

		private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			ViewModel = DataContext as HistoryViewModel;
		}
	}
}
