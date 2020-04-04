using System;
using System.Collections.Generic;
using Caliburn.Micro;
using MoneySplitter.Infrastructure;
using MoneySplitter.Models;
using System.Collections.ObjectModel;
using System.Linq;
using MoneySplitter.Win10.Common;
using System.Threading.Tasks;
using MoneySplitter.Win10.Extensions;

namespace MoneySplitter.Win10.ViewModels
{
	public class IncomingAndOutgoingViewModel : Screen
	{
		#region Fields
		private ObservableCollection<CollaboratorModel> _debtors;
		private ObservableCollection<CollaboratorModel> _lendPersons;

		private bool _isLoading;

		private readonly INavigationManager _navigationManager;
		private readonly CollaboratorModelFactory _collabaratorModelFactory;
		private readonly ITransactionsManager _transactionsManager;
		#endregion

		#region Properties
		public ObservableCollection<CollaboratorModel> Debtors
		{
			get => _debtors;
			set
			{
				_debtors = value;
				NotifyOfPropertyChange(nameof(Debtors));
			}
		}

		public ObservableCollection<CollaboratorModel> LendPersons
		{
			get => _lendPersons;
			set
			{
				_lendPersons = value;
				NotifyOfPropertyChange(nameof(LendPersons));
			}
		}

		public bool IsLoading
		{
			get => _isLoading;
			set
			{
				_isLoading = value;
				NotifyOfPropertyChange(nameof(IsLoading));
			}
		}
		#endregion

		public IncomingAndOutgoingViewModel(
			CollaboratorModelFactory collaboratorModelFactory,
			INavigationManager navigationManager,
			ITransactionsManager transactionsManager)
		{
			_collabaratorModelFactory = collaboratorModelFactory;
			_navigationManager = navigationManager;
			_transactionsManager = transactionsManager;
		}

		protected override async void OnActivate()
		{
			base.OnActivate();

			if (_transactionsManager.UserTransactions == null)
			{
				IsLoading = true;
				await _transactionsManager.LoadUserTransactionsAsync();
				IsLoading = false;
			}

			var debtors = new ObservableCollection<CollaboratorModel>(_collabaratorModelFactory.GetDebtors());
			var lendPersons = new ObservableCollection<CollaboratorModel>(_collabaratorModelFactory.GetLendPersons());

			var crossingPeople = Enumerable.Concat(
				debtors.Where(w => lendPersons.Any(ww => ww.FriendId == w.FriendId)),
				lendPersons.Where(w => debtors.Any(ww => ww.FriendId == w.FriendId))).DistinctBy(w=>w.FriendId).ToList();

			foreach (var crossing in crossingPeople)
			{
				var debtor = debtors.First(w => w.FriendId == crossing.FriendId);
				var lendor = lendPersons.First(w => w.FriendId == crossing.FriendId);

				if (debtor.Cost > lendor.Cost)
				{
					debtor.Cost = debtor.Cost - lendor.Cost;
					lendPersons.Remove(lendor);
				}else if (lendor.Cost > debtor.Cost)
				{
					lendor.Cost = lendor.Cost = debtor.Cost;
					debtors.Remove(debtor);
				}
				else
				{
					debtors.Remove(debtor);
					lendPersons.Remove(lendor);
				}
			}

			Debtors = debtors;
			LendPersons = lendPersons;
		}

		public async Task MoveUserToInProgressAsync(int transactionId, double cost)
		{
			await _transactionsManager.MoveUserToInProgressAsync(transactionId, cost);
		}

		public async Task MoveUserToFinishedAsync(int transactionId, int userId, double incomingCost)
		{
			await _transactionsManager.MoveUserToFinishedAsync(transactionId, userId, incomingCost);
		}
	}
}
