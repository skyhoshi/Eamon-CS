using System;
using Xamarin.Forms;
using Eamon.Mobile.ViewModels;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Mobile.Views
{
	public partial class SettingsPage : ContentPage
	{
		SettingsViewModel viewModel;

		protected async void CreateOrDeleteAdventure()
		{
			var path = ClassMappings.Path.Combine(App.BasePath, ClassMappings.Path.Combine("System", "Bin"));

			var workDir = ClassMappings.Path.Combine(path, App.SettingsViewModel.AdventureWorkDir);

			if (ClassMappings.Directory.Exists(workDir))
			{
				var message = string.Format("Do you want to delete this adventure and all associated textfiles?{0}{0}WARNING:  this action is permanent!", Environment.NewLine);

				var delete = await DisplayAlert("DELETE ADVENTURE", message, "YES", "NO");

				if (delete)
				{
					try
					{
						ClassMappings.Directory.Delete(workDir, true);
					}
					catch (Exception)
					{
						// do nothing
					}
				}
			}
			else
			{
				var message = "Do you want to create this adventure and all associated textfiles?";

				var create = await DisplayAlert("CREATE ADVENTURE", message, "YES", "NO");

				if (create)
				{
					try
					{
						ClassMappings.Directory.CreateDirectory(workDir);
					}
					catch (Exception)
					{
						// do nothing
					}
				}
			}

			App.SettingsViewModel.AdventureWorkDir = "";
		}

		public async void PrintErrorMessage()
		{
			var message = string.Format("You must enter a valid relative path to the adventure working directory which contains no spaces, eg:{0}{0}..\\..\\Adventures\\YourAdventureName", Environment.NewLine);

			await DisplayAlert("INVALID FORMAT", message, "OK");
		}

		public void AdventureWorkDirEntry_Completed(object sender, EventArgs e)
		{
			if (App.SettingsViewModel.AdventureWorkDir.Length > 0)
			{
				var valid = App.SettingsViewModel.AdventureWorkDir.StartsWith("../../Adventures/", StringComparison.Ordinal) || App.SettingsViewModel.AdventureWorkDir.StartsWith(@"..\..\Adventures\", StringComparison.Ordinal);

				if (valid)
				{
					valid = App.SettingsViewModel.AdventureWorkDir.Length > 17;
				}

				if (valid)
				{
					valid = !App.SettingsViewModel.AdventureWorkDir.Contains(" ");
				}

				if (valid)
				{
					Device.BeginInvokeOnMainThread(() => CreateOrDeleteAdventure());
				}
				else
				{
					Device.BeginInvokeOnMainThread(() => PrintErrorMessage());
				}
			}
		}

		protected void BrowseSettingsPage_SizeChanged(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				ForegroundColorPicker.Unfocus();

				BackgroundColorPicker.Unfocus();

				FontFamilyPicker.Unfocus();

				FontSizePicker.Unfocus();

				ScrollbackBufferSizePicker.Unfocus();

				AdventureWorkDirEntry.Unfocus();
			});

			Device.BeginInvokeOnMainThread(() =>
			{
				ForegroundColorLabel.IsVisible = false;

				ForegroundColorLabel.IsVisible = true;

				ForegroundColorPicker.IsVisible = false;

				ForegroundColorPicker.IsVisible = true;

				Separator0.IsVisible = false;

				Separator0.IsVisible = true;
				
				BackgroundColorLabel.IsVisible = false;

				BackgroundColorLabel.IsVisible = true;

				BackgroundColorPicker.IsVisible = false;

				BackgroundColorPicker.IsVisible = true;
				
				Separator1.IsVisible = false;

				Separator1.IsVisible = true;

				FontFamilyLabel.IsVisible = false;

				FontFamilyLabel.IsVisible = true;

				FontFamilyPicker.IsVisible = false;

				FontFamilyPicker.IsVisible = true;

				Separator2.IsVisible = false;

				Separator2.IsVisible = true;

				FontSizeLabel.IsVisible = false;

				FontSizeLabel.IsVisible = true;

				FontSizePicker.IsVisible = false;

				FontSizePicker.IsVisible = true;

				Separator3.IsVisible = false;

				Separator3.IsVisible = true;

				ScrollbackBufferSizeLabel.IsVisible = false;

				ScrollbackBufferSizeLabel.IsVisible = true;

				ScrollbackBufferSizePicker.IsVisible = false;

				ScrollbackBufferSizePicker.IsVisible = true;

				Separator4.IsVisible = false;

				Separator4.IsVisible = true;
				
				KeepKeyboardVisibleLabel.IsVisible = false;

				KeepKeyboardVisibleLabel.IsVisible = true;

				KeepKeyboardVisibleSwitch.IsVisible = false;

				KeepKeyboardVisibleSwitch.IsVisible = true;
				
				Separator5.IsVisible = false;

				Separator5.IsVisible = true;

				AdventureWorkDirLabel.IsVisible = false;

				AdventureWorkDirLabel.IsVisible = true;

				AdventureWorkDirEntry.IsVisible = false;

				AdventureWorkDirEntry.IsVisible = true;

				Separator6.IsVisible = false;

				Separator6.IsVisible = true;
			});
		}

		public SettingsPage(SettingsViewModel settingsViewModel)
		{
			InitializeComponent();

			BindingContext = viewModel = settingsViewModel;

			App.SettingsPage = this;
		}
	}
}
