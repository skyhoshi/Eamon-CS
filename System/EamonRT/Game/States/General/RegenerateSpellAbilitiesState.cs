
// RegenerateSpellAbilitiesState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class RegenerateSpellAbilitiesState : State, IRegenerateSpellAbilitiesState
	{
		public override void Execute()
		{
			if (ShouldPreTurnProcess())
			{
				var spellValues = EnumUtil.GetValues<Enums.Spell>();

				foreach (var sv in spellValues)
				{
					if (Globals.GameState.GetSa(sv) < Globals.Character.GetSpellAbilities(sv))
					{
						var n = (long)((double)Globals.GameState.GetSa(sv) * 1.1) - Globals.GameState.GetSa(sv);

						if (Globals.GameState.GetSa(sv) > 0 && n < 1)
						{
							n = 1;
						}

						Globals.GameState.ModSa(sv, n);
					}

					if (Globals.GameState.GetSa(sv) > Globals.Character.GetSpellAbilities(sv))
					{
						Globals.GameState.SetSa(sv, Globals.Character.GetSpellAbilities(sv));
					}
				}
			}
						
			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IPrintPlayerRoomState>();
			}

			Globals.NextState = NextState;
		}

		public RegenerateSpellAbilitiesState()
		{
			Name = "RegenerateSpellAbilitiesState";
		}
	}
}
