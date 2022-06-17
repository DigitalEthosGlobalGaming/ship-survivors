﻿using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class PelletWeaponUpgradeAttackSpeed : WeaponUpgrade 
	{

		public override string ParentUpgradeClassName { get; set; } = "PelletWeapon";
		public override string UpgradeName { get; set; } = "Lubricated Chambers";
		public override string Description { get; set; } = "Increases Attack Speed";
		public override string Image { get; set; } = "/raw/crosshairs/green/crosshair023.png";
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
			var player = base.GetShipPlayer();
			if ( player?.IsValid() ?? false )
			{
				player.AttackSpeed = player.AttackSpeed + 0.1f;
			}
		}

	}
}
