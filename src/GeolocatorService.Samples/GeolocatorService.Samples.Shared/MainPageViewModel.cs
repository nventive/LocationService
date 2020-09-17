using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Windows.UI.Core;

namespace GeolocatorService.Samples
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		private CoreDispatcher _dispatcher;
		private IGeolocatorService _geolocatorService;
		private IDisposable _subscription = null;

		public event PropertyChangedEventHandler PropertyChanged;

		public MainPageViewModel(CoreDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
			_geolocatorService = new GeolocatorService();
		}

		private string _pageContent = "";
		public string PageContent
		{
			get => _pageContent;
			set
			{
				_pageContent = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageContent)));
			}
		}

		private void SetPageContent(string content)
		{
			if (_dispatcher.HasThreadAccess)
			{
				PageContent = content;
			}
			else
			{
				_ = _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					SetPageContent(content);
				});
			}
		}

		public DelegateCommand RequestPermissionCommand => new DelegateCommand(() =>
		{
			_subscription = _geolocatorService.
				GetAndObserveLocationOrDefault()
				.Subscribe(
					location =>
					{
						if (location == null)
						{
							SetPageContent("Please give this app the location permission.");
						}
						else
						{
							SetPageContent($"You are currently located at ({location.Point.Position.Longitude}, {location.Point.Position.Latitude})");
						}
					},
					error =>
					{
						SetPageContent($"An error has occurred: {error}");
					}
				);
		});

		public class DelegateCommand : ICommand
		{
			private Action _command;

			public DelegateCommand(Action command)
			{
				_command = command;
			}

			public event EventHandler CanExecuteChanged;

			public bool CanExecute(object parameter) => true;

			public void Execute(object parameter)
			{
				_command();
			}
		}
	}
}
