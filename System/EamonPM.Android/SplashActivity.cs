using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;

using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading;
using Android.Content.PM;
using Eamon;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;
using Eamon.Mobile;
using static Eamon.Game.Plugin.PluginContext;
using static Eamon.Game.Plugin.PluginContextStack;
using System.Threading.Tasks;

namespace EamonPM
{
	[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, ScreenOrientation = ScreenOrientation.Locked, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof (SplashActivity).Name;

		protected virtual void MirrorAssets()
		{
			var path = Path.Combine(App.BasePath, Path.Combine("System", "Bin"));

			Directory.CreateDirectory(path);

			var binFiles = Assets.List(Path.Combine("System", "Bin"));

			foreach (var file in binFiles)
			{
				var fileName = Path.Combine(path, file);

				if (!File.Exists(fileName))
				{
					fileName = Path.Combine(Path.Combine("System", "Bin"), file);

					using (var streamReader = new StreamReader(Assets.Open(fileName)))
					{
						var fileBytes = default(byte[]);

						using (var memoryStream = new MemoryStream())
						{
							streamReader.BaseStream.CopyTo(memoryStream);

							fileBytes = memoryStream.ToArray();

							fileName = Path.Combine(path, file);

							File.WriteAllBytes(fileName, fileBytes);
						}
					}
				}
			}

			var adventureDirs = Assets.List("Adventures");

			foreach (var dir in adventureDirs)
			{
				path = Path.Combine(App.BasePath, Path.Combine("Adventures", dir));

				Directory.CreateDirectory(path);

				var adventureFiles = Assets.List(Path.Combine("Adventures", dir));

				foreach (var file in adventureFiles)
				{
					var fileName = Path.Combine(path, file);

					if (!File.Exists(fileName))
					{
						fileName = Path.Combine(Path.Combine("Adventures", dir), file);

						using (var streamReader = new StreamReader(Assets.Open(fileName)))
						{
							var fileBytes = default(byte[]);

							using (var memoryStream = new MemoryStream())
							{
								streamReader.BaseStream.CopyTo(memoryStream);

								fileBytes = memoryStream.ToArray();

								fileName = Path.Combine(path, file);

								File.WriteAllBytes(fileName, fileBytes);
							}
						}
					}
				}
			}
		}

		// Launches the startup task
		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
      {
         base.OnCreate(savedInstanceState, persistentState);
			Log.Debug(TAG, "SplashActivity.OnCreate");
       }

		// Launches the startup task
		protected override void OnResume()
		{
			base.OnResume();

			App.Rc = RetCode.Success;

			App.BasePath = Application.Context.FilesDir.Path;

			Task startupWork = new Task(() => { StartupWork(); });

			startupWork.Start();
		}

		// Prevent the back button from interfering with the startup process
		protected override void OnDestroy()
		{
			base.OnDestroy();

			App.BackButtonEnabled = true;
		}

		// Prevent the back button from canceling the startup process
		public override void OnBackPressed() { }

		void StartupWork()
		{
			// Log.Debug(TAG, "Performing some startup work that takes a bit of time.");

			MirrorAssets();

			StartActivity(new Intent(Application.Context, typeof (MainActivity)));
		}
   }
}