using ShipSurvivors;
using System.Collections.Generic;

namespace Sandbox.Gamemode.Entities.ShipPlayerShips.cs
{
	public partial class ShipPlayerFighter: ShipPlayer
	{

		public override void Spawn()
		{
			base.Spawn();
			BuyUpgrade( "PelletWeapon");
		}

		public override List<Upgrade> GetUpgradesToBuy()
		{
			var upgrades = base.GetUpgradesToBuy();

			return upgrades;
		}

		public override string[] GetUpgradeClassNames()
		{
			return new string[] {
				"FigherUpgradeShieldRegeneration",
				"FigherUpgradeShieldCapacity",
			};
		}


		public override void OnRoundStart()
		{
			base.OnRoundStart();
			var regenLevel = GetUpgradeLevel( "FigherUpgradeShieldRegeneration" );
			if (regenLevel > 0)
			{
				Health = Health + regenLevel;
				if (Health > MaxHealth)
				{
					Health = MaxHealth;
				}
			}
		}
	}
}
