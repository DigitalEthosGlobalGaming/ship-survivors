using Degg.Entities;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipSurvivors
{
	public partial class DroneWeapon : ShipWeapon
	{
		public override string UpgradeName { get; set; } = "Drone Weapon";
		public override string Description { get; set; } = "Basic Pellet Weapon";
		public override string Image { get; set; } = "/raw/crosshairs/green/crosshair177.png";
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
			bullet.Damage = AttackBulletDamage;

			var velocity = (Rotation.Forward * (50f + 50));
			bullet.PhysicsBody.Velocity = velocity;
			bullet.EntityMaterial = "materials/bullets/bullet_player_1.vmat";
			PlaySoundOnClient( "ship.weapon.fire" );

			if (Owner is ShipPlayer player)
			{
				player.ScreenShakeOnClient( 1f );
			}
		}


		public override string[] GetUpgradeClassNames()
		{
			return new string[] {
				"PelletWeaponUpgradeBulletSize",
				"PelletWeaponUpgradeBulletPenetration",
				"PelletWeaponUpgradeBulletSplashDamage",
				"PelletWeaponUpgradeBulletDamage",
				"PelletWeaponUpgradeAttackSpeed",
				"PelletWeaponUpgradeLifeSteal"
			};
		}
	}
}
