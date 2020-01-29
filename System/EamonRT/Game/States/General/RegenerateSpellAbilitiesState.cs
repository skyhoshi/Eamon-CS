
// RegenerateSpellAbilitiesState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.States;
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
				var spellValues = EnumUtil.GetValues<Spell>();

				foreach (var sv in spellValues)
				{
					if (gGameState.GetSa(sv) < gCharacter.GetSpellAbilities(sv))
					{
						var n = (long)((double)gGameState.GetSa(sv) * 1.1) - gGameState.GetSa(sv);

						if (gGameState.GetSa(sv) > 0 && n < 1)
						{
							n = 1;
						}

						gGameState.ModSa(sv, n);

						if (gGameState.GetSa(sv) > gCharacter.GetSpellAbilities(sv))
						{
							gGameState.SetSa(sv, gCharacter.GetSpellAbilities(sv));
						}
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
