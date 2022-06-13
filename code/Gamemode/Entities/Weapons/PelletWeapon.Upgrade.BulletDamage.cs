using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class PelletWeaponUpgradeBulletDamage : WeaponUpgrade 
	{
		public override string ParentUpgradeClassName { get; set; } = "PelletWeapon";
		public override string UpgradeName { get; set; } = "Enhanced Warheads";
		public override string Description { get; set; } = "Increases damage";
		public override string Image { get; set; } = "";
		public override float Rarity { get; set; } = 1;
		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Owner;
			Active = false;
		}

		public override void OnOwnerStatsUpdate()
		{
			base.OnOwnerStatsUpdate();
			var weapon = GetWeapon<PelletWeapon>();
			if ( weapon?.IsValid() ?? false )
			{
				weapon.AttackBulletDamage = weapon.AttackBulletDamage + 1;
			}
		}

	}
}
