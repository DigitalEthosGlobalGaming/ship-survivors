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

		public float AttackBulletSize { get; set; }

		public float AttackBulletSpeed { get; set; }

		public override void Spawn()
		{
			base.Spawn();
		}


		public override void ResetStats()
		{
			base.ResetStats();
			AttackBulletSize = 0.2f;
			AttackBulletSpeed = 1f;
		}

		public override void OnFire()
		{
			var bullet = new Bullet();
			bullet.Owner = this;
			bullet.Position = Position + (Rotation.Forward * 5f);
			bullet.Scale = AttackBulletSize;
			var velocity = (Rotation.Forward * (50f + AttackBulletSpeed));
			bullet.PhysicsBody.Velocity = velocity;
			PlaySoundOnClient( "ship.weapon.fire" );
		}


		public override List<Upgrade> GetUpgrades()
		{
			var upgrades = new List<Upgrade>();
			upgrades.Add( new PelletWeaponUpgradeBulletSize()
			{
				ParentUpgrade = this,
			} );

			upgrades.Add(new PelletWeaponUpgradeBulletSpeed()
			{
				ParentUpgrade = this,
			});

			return upgrades;
		}


	}
}
