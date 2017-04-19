
// Monster.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings(typeof(Eamon.Framework.IMonster))]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override long Weapon
		{
			get
			{
				return base.Weapon;
			}

			set
			{
				if (base.Weapon != value)
				{
					var gameState = Globals.GameState as IGameState;

					if (gameState != null)     	// null in EamonDD; non-null in EamonRT
					{
						if (Uid == Globals.GameState.Cm || Uid == 8)
						{
							if (base.Weapon == 10 && value != 10 && gameState.Trollsfire == 1)
							{
								gameState.Trollsfire = 0;
							}
						}

						if (Uid == 8)
						{
							if (base.Weapon != 10 && value == 10 && gameState.Trollsfire == 0)
							{
								gameState.Trollsfire = 1;
							}
						}
					}

					base.Weapon = value;
				}
			}
		}

		public override void AddHealthStatus(StringBuilder buf, bool addNewLine = true)
		{
			string result = null;

			if (buf == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (IsDead())
			{
				result = "dead!";
			}
			else
			{
				var x = DmgTaken;

				x = (((long)((double)(x * 5) / (double)Hardiness)) + 1) * (x > 0 ? 1 : 0);

				// historical status reports from original

				result = "at death's door, knocking loudly.";

				if (x == 4)
				{
					result = "very badly injured.";
				}
				else if (x == 3)
				{
					result = "in pain.";
				}
				else if (x == 2)
				{
					result = "hurting.";
				}
				else if (x == 1)
				{
					var str = buf.ToString();

					if (str.EndsWith("You are ", StringComparison.OrdinalIgnoreCase))
					{
						buf.Length -= 4;
					}
					else if (str.Length > 3 && str.Substring(str.Length - 4).EndsWith(" is ", StringComparison.OrdinalIgnoreCase))
					{
						buf.Length -= 3;
					}

					str = buf.ToString();

					if (str.EndsWith("They are ", StringComparison.OrdinalIgnoreCase))
					{
						result = "taking damage but still in good shape.";
					}
					else if (str.EndsWith("You ", StringComparison.OrdinalIgnoreCase))
					{
						result = "have taken damage but are still in good shape.";
					}
					else
					{
						result = "has taken damage but is still in good shape.";
					}
				}
				else if (x < 1)
				{
					result = "in perfect health.";
				}
			}

			Debug.Assert(result != null);

			buf.AppendFormat("{0}{1}", result, addNewLine ? Environment.NewLine : "");

		Cleanup:

			;
		}
	}
}
