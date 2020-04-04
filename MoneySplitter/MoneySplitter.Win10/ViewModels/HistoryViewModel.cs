using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using MoneySplitter.Infrastructure;
using MoneySplitter.Models;
using MoneySplitter.Models.App;

namespace MoneySplitter.Win10.ViewModels
{
	public class HistoryViewModel : Screen
	{
		private readonly ITransactionsManager _transactionsManager;
		private readonly IMembershipService _membershipService;

		private IEnumerable<HistoryItemModel> _items;
		private double _total;

		public IEnumerable<HistoryItemModel> Items
		{
			get => _items;
			set
			{
				_items = value;
				NotifyOfPropertyChange(nameof(Items));
			}
		}

		public double Total
		{
			get => _total;
			set
			{
				_total = value;
				NotifyOfPropertyChange(nameof(Total));
			}
		}

		public HistoryViewModel(
			ITransactionsManager transactionsManager,
			IMembershipService membershipService)
		{
			_transactionsManager = transactionsManager;
			_membershipService = membershipService;
		}

		protected override void OnViewReady(object view)
		{
			base.OnViewReady(view);

			Items = _transactionsManager.UserTransactions
				.Where(w => w.FinishedIds.Count() != w.Collaborators.Count())
				.OrderByDescending(w => w.CreationDate)
				.Select(CreateHistoryItem)
				.Where(w=>w.Cost != 0)
				.ToList();

			Total = Items.Sum(w => w.Cost);
		}

		private HistoryItemModel CreateHistoryItem(TransactionModel transaction)
		{
			var isOwner = transaction.Owner.Id == _membershipService.CurrentUser.Id;

			var cost = isOwner
				? -(transaction.Cost - transaction.FinishedIds.Count() * transaction.SingleCost)
				: transaction.FinishedIds.Contains(_membershipService.CurrentUser.Id) ? -transaction.SingleCost : transaction.SingleCost;

			return new HistoryItemModel
			{
				Title = transaction.Title,
				Cost = cost,
				Date = transaction.CreationDate
			};
		}
	}
}
