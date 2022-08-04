using Degg.Entities;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipSurvivors
{
	public partial class BasicDroneWeapon : DroneWeapon
	{
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


	}
}
