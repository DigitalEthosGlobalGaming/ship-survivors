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
	}
}
