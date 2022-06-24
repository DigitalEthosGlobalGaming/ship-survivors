using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipSurvivors
{
	public partial class ShipWeapon : Upgrade
	{

		public override void Spawn()
		{
			base.Spawn();
		}
		public float NextFireTime { get; set; }

		public override void Fire()
		{
			if ( IsServer )
			{
				if ( NextFireTime <= Time.Now )
				{
					NextFireTime = Time.Now + GetAttackSpeed();
					OnFire();
				}
			}
		}
		public float GetAttackSpeed()
		{
			if (Parent is ShipPlayer parent )
			{
				var attackSpeed = 0.5f - parent.AttackSpeed;
				if ( attackSpeed < 0.1f )
				{
					attackSpeed = 0.05f;
				}
				return attackSpeed;
			} else if ( Parent is EnemyShip enemyShip )
			{
				return enemyShip.AttackSpeed;
			}
			return 0;
		}

	}
}
