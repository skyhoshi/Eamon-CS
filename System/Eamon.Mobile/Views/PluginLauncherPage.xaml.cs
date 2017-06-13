using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class PluginLauncherPage : ContentPage
	{
		PluginLauncherViewModel viewModel;

		public PluginLauncherPage(string title)
		{
			InitializeComponent();

			BindingContext = viewModel = new PluginLauncherViewModel(title);

			viewModel.PropertyChanged += (o, e) =>
			{
				if (e.PropertyName == "OutputText")
				{
					ScrollToBottomOfPluginScrollView();
				}
			};

			App.PluginLauncherPage = this;
		}

		public void ScrollToBottomOfPluginScrollView()
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				await PluginScrollView.ScrollToAsync(Separator, ScrollToPosition.Start, true);
			});
		}

		public async void SetInputTextNoEvents(string value)
		{
			if (viewModel.InputText == null || !string.Equals(viewModel.InputText, value, StringComparison.Ordinal))
			{
				InputEntry.TextChanged -= InputEntry_TextChanged;

				viewModel.InputText = value;

				await Task.Yield();

				InputEntry.TextChanged += InputEntry_TextChanged;
			}
		}

		public void InputEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			var newTextValue = e.NewTextValue ?? "";

			var oldTextValue = e.OldTextValue ?? "";

			if (newTextValue.Length > oldTextValue.Length && !App.InputEntryCompletedPending)
			{
				var ch = newTextValue[newTextValue.Length - 1];

				var ch01 = ch;

				if (App.InputBufSize > 0 && newTextValue.Length <= App.InputBufSize)
				{
					if (App.InputModifyCharFunc != null)
					{
						ch = App.InputModifyCharFunc(ch);
					}

					var validChar = true;

					if (App.InputValidCharFunc != null)
					{
						validChar = App.InputValidCharFunc(ch);
					}

					if (validChar)
					{
						if (ch != '\0')
						{
							if (ch != ch01)
							{
								SetInputTextNoEvents(string.Format("{0}{1}", oldTextValue, ch));
							}
						}
						else
						{
							SetInputTextNoEvents(oldTextValue);
						}

						var termChar = false;

						if (App.InputTermCharFunc != null)
						{
							termChar = App.InputTermCharFunc(ch);
						}

						if (termChar)
						{
							App.InputEntryCompletedPending = true;

							Device.BeginInvokeOnMainThread(() =>
							{
								InputEntry_Completed(InputEntry, null);
							});
						}
					}
					else
					{
						SetInputTextNoEvents(oldTextValue);
					}
				}
				else
				{
					SetInputTextNoEvents(oldTextValue);
				}
			}
		}

		public void InputEntry_Completed(object sender, EventArgs e)
		{
			if (viewModel.InputText.Length > 0 || App.InputEmptyAllowed)
			{
				if (viewModel.InputText.Length == 0 && App.InputEmptyVal != null)
				{
					SetInputTextNoEvents(App.InputEmptyVal);
				}

				Device.BeginInvokeOnMainThread(() =>
				{
					if (!App.SettingsViewModel.KeepKeyboardVisible)
					{
						InputEntry.Unfocus();
					}

					App.InputEntryCompletedPending = false;

					App.FinishInput.Set();
				});
			}
			else		// never reached, but just in case
			{
				Device.StartTimer(new TimeSpan(0, 0, 0, 0, 250), () =>
				{
					InputEntry.Focus();

					App.InputEntryCompletedPending = false;

					return false;
				});
			}
		}

		public void InputEntry_Unfocus()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				InputEntry.Unfocus();
			});
		}

		protected void BrowsePluginLauncherPage_SizeChanged(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				InputEntry.Unfocus();
			});

			Device.BeginInvokeOnMainThread(() =>
			{
				OutputLabel.IsVisible = false;

				OutputLabel.IsVisible = true;

				Separator.IsVisible = false;

				Separator.IsVisible = true;

				InputEntry.IsVisible = false;

				InputEntry.IsVisible = true;

				PluginScrollView.IsVisible = false;

				PluginScrollView.IsVisible = true;
			});

			ScrollToBottomOfPluginScrollView();
		}
	}
}
