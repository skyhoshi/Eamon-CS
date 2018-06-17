
// MainLoop.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using static WalledCityOfDarkness.Game.Plugin.PluginContext;

namespace WalledCityOfDarkness.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Print("Lumen finally speaks:");

			Globals.Out.Print("\"As you see, the report of my death was premature.  I was nearly destroyed, yet enough of the faithful ignored the news and continued their prayers to sustain my existence.  " +
				"And even so, I was vanquished, and the nameless was triumphant, and this is an event that has never before occurred in all our thousands of years of struggle.  The outlook for myself, and my city, was bleak.  " +
				"The wizard Tokas was indeed our savior.  For it was his skill and his wisdom that forestalled our end, and brought your might and valor to our rescue.");

			Globals.In.KeyPress(Globals.Buf);

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Print("The final victory is yours.  Never in his long existence has the nameless taken a pummelling as you have given him.  I was vanquished, yet you were stronger; and now you have the " +
				"power that was once mine alone.  Now you must decide which of us will rule the city of Lumen as its god and protector.  If you retain the role, then so be it; I will diminish and one day end as " +
				"the people cease to offer me their prayers.  And if you relinquish the role, I will see that your gold and possessions are returned to you.  And now you must give me your decision.\"");

			Globals.Out.Write("{0}Will you retain the trappings of a god, and forever more remain protector of the city of Lumen (Y/N): ", Environment.NewLine);

			Globals.Buf.Clear();

			var rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Out.Print("{0}", Globals.LineSep);

			if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
			{
				Globals.Out.Print("\"So be it.\"  And with those final words on his lips, he fades away into insubstantiality.  You stare for a moment at the place he had stood, thinking wistfully about the " + 
					"good times that were yours when you were merely a mortal.  Then, squaring your shoulders, you turn and begin the journey back to the city of " + Globals.Character.Name + ", formerly named Lumen.");

				Globals.Out.Print("It's time to do something about what was left of your temple.  It was a real mess.");

				Globals.In.KeyPress(Globals.Buf);

				Globals.ExitType = Enums.ExitType.DeleteCharacter;
			}
			else
			{
				Globals.Out.Print("\"So be it.\"  He dons the trappings of the protector of the city of Lumen as, one by one, you remove them and hand them to him.  Finished, he says to you, not unkindly, " + 
					"\"You must realize that I cannot allow you to return to the city as a savior.  Such an act would undermine my support, and could one day again end in sorrow.\"");

				Globals.In.KeyPress(Globals.Buf);

				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("You agree to this.  Your possessions and your gold are returned to you, and you spend a quiet few days in the city as a simple wanderer, visiting with old Hokas Tokas and " +
					"sampling the fine beer at the \"Prancing Unicorn\".  You may not have won great fortune or fame from your adventure, but you are content; after all, you have experienced the power of a god.  " +
					"No one will ever believe your story, but that doesn't bother you much, either.  With this tale, you are certain to take all the honors at the Main Hall's next liars' contest!");

				Globals.In.KeyPress(Globals.Buf);

				// TODO: transfer possessions and gold if necessary

				base.Shutdown();
			}
		}
	}
}
