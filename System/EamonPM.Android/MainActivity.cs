
// MainActivity.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Eamon;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;
using Eamon.Mobile;
using static Eamon.Game.Plugin.PluginContext;
using static Eamon.Game.Plugin.PluginContextStack;

namespace EamonPM
{
	// *** Note: don't forget to change BuildGuid in SplashActivity.cs if necessary! ***

	[Activity(Label = "@string/app_name", Theme = "@style/MyTheme", Icon = "@drawable/ten_sided_die", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected virtual Thread GameThread { get; set; }

		protected virtual void LoadPortabilityClassMappings(IDictionary<Type, Type> classMappings)
		{
			//Debug.Assert(classMappings != null);

			classMappings[typeof(ITextReader)] = typeof(Game.Portability.TextReader);

			classMappings[typeof(ITextWriter)] = typeof(Game.Portability.TextWriter);

			classMappings[typeof(IMutex)] = typeof(Game.Portability.Mutex);

			classMappings[typeof(ITransferProtocol)] = typeof(Game.Portability.TransferProtocol);

			classMappings[typeof(IDirectory)] = typeof(Game.Portability.Directory);

			classMappings[typeof(IFile)] = typeof(Game.Portability.File);

			classMappings[typeof(IPath)] = typeof(Game.Portability.Path);

			classMappings[typeof(ISharpSerializer)] = typeof(Game.Portability.SharpSerializer);

			classMappings[typeof(IThread)] = typeof(Game.Portability.Thread);
		}

		protected virtual void ExecutePlugin(string[] args, bool enableStdio = true)
		{
			//Debug.Assert(args != null);

			//Debug.Assert(args.Length > 1);

			//Debug.Assert(string.Equals(args[0], "-pfn", StringComparison.OrdinalIgnoreCase));

			var pluginFileName = System.IO.Path.GetFileNameWithoutExtension(args[1]);

			var assemblyName = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(an => an.Name == pluginFileName).FirstOrDefault();

			while (App.PluginLauncherViewModel == null)
			{
				Thread.Sleep(150);
			}

			App.PluginLauncherViewModel.Title =
				args.Contains("-dgs") || args.Contains("EamonMH.dll") ? "EamonMH" :
				args.Contains("-rge") ? "EamonDD" :
				args.Contains("EamonRT.dll") ? "EamonRT" :
				pluginFileName;

			//Debug.Assert(assemblyName != null);

			var plugin = Assembly.Load(assemblyName);

			//Debug.Assert(plugin != null);

			var typeName = string.Format("{0}.Program", pluginFileName);

			var type = plugin.GetType(typeName);

			//Debug.Assert(type != null);

			var program = (IProgram)Activator.CreateInstance(type);

			//Debug.Assert(program != null);

			program.EnableStdio = enableStdio;

			program.LineWrapUserInput = true;

			program.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

			program.Main(args.Skip(2).ToArray());
		}

		protected virtual void SaveSettings()
		{
			if (App.SettingsViewModel != null)
			{
				var path = Path.Combine(App.BasePath, Path.Combine("System", "Bin"));

				var fileName = Path.Combine(path, "MOBILE_SETTINGS.XML");

				ClassMappings.SharpSerializer.Serialize(App.SettingsViewModel, fileName);
			}
		}

		protected virtual void PluginLoop(object obj)
		{
			var args = (string[])obj;

			while (true)
			{
				if (args == null || args.Length < 2 || !string.Equals(args[0], "-pfn", StringComparison.OrdinalIgnoreCase))
				{
					goto Cleanup;
				}

				try
				{
					App.ExecutePlugin(args, true);
				}
				catch (Exception e)
				{
					// print message box

					// goto Cleanup;
				}

				args = App.NextArgs;

				App.NextArgs = null;
			}

		Cleanup:

			if (App.SettingsViewModel.KeepKeyboardVisible)
			{
				App.PluginLauncherPage.InputEntry_Unfocus();
			}

			Thread.Sleep(500);

			RunOnUiThread(SaveSettingsAndTerminate);
		}

		protected virtual void StartGameThread()
		{
			var threadStart = new System.Threading.ParameterizedThreadStart(App.PluginLoop);

			GameThread = new System.Threading.Thread(threadStart);

			GameThread.Start(App.BatchFile.PluginArgs);
		}

		protected virtual void SaveSettingsAndTerminate()
		{
			SaveSettings();

			Finish();

			Thread.Sleep(500);

			Process.KillProcess(Process.MyPid());
		}

		protected virtual void ForcePluginLinkage()
		{
			IPluginGlobals pg = EamonMH.Game.Plugin.PluginContext.Globals;

			pg = EamonDD.Game.Plugin.PluginContext.Globals;

			pg = EamonRT.Game.Plugin.PluginContext.Globals;

			pg = ARuncibleCargo.Game.Plugin.PluginContext.Globals;

			pg = BeginnersForest.Game.Plugin.PluginContext.Globals;

			pg = StrongholdOfKahrDur.Game.Plugin.PluginContext.Globals;

			pg = TheBeginnersCave.Game.Plugin.PluginContext.Globals;

			pg = TheSubAquanLaboratory.Game.Plugin.PluginContext.Globals;

			pg = TheTempleOfNgurct.Game.Plugin.PluginContext.Globals;

			pg = TheTrainingGround.Game.Plugin.PluginContext.Globals;

			pg = WrenholdsSecretVigil.Game.Plugin.PluginContext.Globals;

			// Note: if ECS Mobile crashes while loading textfiles it is likely that the FreeUids list in the offending textfile is
			// defined as coming from System.Private.CoreLib.  Xamarin.Forms appears to currently only be compatible with mscorlib,
			// so you should find a textfile in a previous adventure containing this definition and copy it over.
		}

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			ForcePluginLinkage();

			PushConstants();

			PushClassMappings();

			ClassMappings.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

			ClassMappings.ResolvePortabilityClassMappings();

			var path = Path.Combine(App.BasePath, Path.Combine("System", "Bin"));

			Directory.SetCurrentDirectory(path);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			App.GetDocumentationFiles = () =>
			{
				return Directory.GetFiles(Path.Combine(App.BasePath, "Documentation"));
			};

			App.GetAdventureDirs = () =>
			{
				var fullDirs = Directory.GetDirectories(Path.Combine(App.BasePath, "Adventures"));

				var dirList = new List<string>();

				foreach (var fullDir in fullDirs)
				{
					dirList.Add(Path.GetFileNameWithoutExtension(fullDir));
				}

				return dirList.ToArray();
			};

			App.PluginExists = (pluginFileName) =>
			{
				var pluginBaseName = Path.GetFileNameWithoutExtension(pluginFileName);

				var assemblyName = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(an => an.Name == pluginBaseName).FirstOrDefault();

				return assemblyName != null;
			};

			App.ExecutePlugin = ExecutePlugin;

			App.PluginLoop = PluginLoop;

			App.StartGameThread = StartGameThread;

			LoadApplication(new App());
		}

		protected override void OnDestroy()
		{
			if (App.Rc != RetCode.Success)
			{
				// +++ IMPLEMENT +++
			}

			if (GameThread != null)
			{
				GameThread.Join(0);

				if (GameThread.IsAlive)
				{
					GameThread.Abort();
				}
			}

			PopClassMappings();

			PopConstants();

			base.OnDestroy();
		}

		public override void OnBackPressed()
		{
			if (App.ShouldStopApplicationOnBackPressed())
			{
				RunOnUiThread(SaveSettingsAndTerminate);
			}
			else
			{
				base.OnBackPressed();
			}
		}
	}
}