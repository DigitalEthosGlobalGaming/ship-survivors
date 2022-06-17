using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class PelletWeaponUpgradeLifeSteal : WeaponUpgrade 
	{

		public override string ParentUpgradeClassName { get; set; } = "PelletWeapon";
		public override string UpgradeName { get; set; } = "Energy Sapper";
		public override string Description { get; set; } = "When hitting an enemy you have a chance to rechare your shield.";
		public override string Image { get; set; } = "/raw/crosshairs/green/crosshair002.png";
		public override float Rarity { get; set; } = 1;

		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Owner;
			Active = false;
		}

		public override void OnEnemyDamaged( Entity b, EnemyShip e, bool didKill )
		{
			base.OnEnemyDamaged( b, e, didKill );
			var player = base.GetShipPlayer();
			if ( player?.IsValid() ?? false )
			{
				var random = Rand.Float( 100 ) <= 5;
				if ( random )
				{
					player.Health = player.Health + 1;
					if ( player.Health > player.MaxHealth )
					{
						player.Health = player.MaxHealth;
					}
				}
			}
		}
	}
}
