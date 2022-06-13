using Degg.Entities;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipSurvivors
{
	public partial class PelletWeapon : ShipWeapon
	{
		public override string UpgradeName { get; set; } = "Pellet Weapon";
		public override string Description { get; set; } = "Basic Pellet Weapon";
		public override string Image { get; set; } = "/raw/spaceshooter/src/star_tiny.png";
		public override float Rarity { get; set; } = 1;
		public float AttackBulletSize { get; set; }
		public int AttackBulletPenetration { get; set; }
		public float AttackBulletDamage { get; set; }
		public float AttackBulletSpeed { get; set; }

		public override void Spawn()
		{
			base.Spawn();
		}


		public override void ResetStats()
		{
			base.ResetStats();
			AttackBulletDamage = 1;
			AttackBulletPenetration = 1;
			AttackBulletSize = 0.2f;
			AttackBulletSpeed = 1f;
		}

		public override void OnFire()
		{
			var bullet = new Bullet();
			bullet.Owner = this;
			bullet.Position = Position + (Rotation.Forward * 5f);
			bullet.Scale = AttackBulletSize;
			bullet.Strength = AttackBulletPenetration;
			var velocity = (Rotation.Forward * (50f + AttackBulletSpeed));
			bullet.PhysicsBody.Velocity = velocity;
			PlaySoundOnClient( "ship.weapon.fire" );
		}


		public override string[] GetUpgradeClassNames()
		{
			return new string[] {
				"PelletWeaponUpgradeBulletSize",
				"PelletWeaponUpgradeBulletSpeed",
				"PelletWeaponUpgradeBulletPenetration",
				"PelletWeaponUpgradeBulletSplashDamage",
				"PelletWeaponUpgradeBulletDamage",
				"PelletWeaponUpgradeAttackSpeed"
			};
		}
	}
}
