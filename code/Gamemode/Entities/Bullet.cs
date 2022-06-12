using Degg.Entities;
using Sandbox;

namespace ShipSurvivors
{
	public partial class Bullet : Entity2D
	{
		public int Strength { get; set; }
		public float LastStrenghtLostTick { get; set; }
		public float DeathTime { get; set; }
		public override void Spawn()
		{
			base.Spawn();
			SetShape(Entity2DShapes.Circle);
			Scale = 0.5f;
			EntityMaterial = "materials/bullets/bullet_1.vmat";
			CollisionGroup = CollisionGroup.Trigger;
			Strength = 1;
			DeathTime = Time.Now + 10f;
		}

		public void LoseStrength()
		{
			if ( LastStrenghtLostTick == Time.Tick)
			{
				return;
			}

			LastStrenghtLostTick = Time.Tick;
			Strength = Strength - 1;
			if (Strength <= 0)
			{
				EmitSound( "bullet.collision" );
				Delete();
			}
		}


		public override void StartTouch( Entity other )
		{
			base.StartTouch( other );
			if ( IsClient )
			{
				return;
			}
			if ( DeathTime < Time.Now)
			{
				Delete();
			}
			if ( Owner != null )
			{
				if ( Owner is ShipWeapon weapon )
				{
					Owner = weapon.GetShipPlayer();
				}

				if ( Owner == other )
				{
					return;
				}


				if ( other is ShipPlayer player )
				{
					this.Delete();
					var damage = new DamageInfo();
					damage.Attacker = Owner;
					damage.Damage = 1f;
					damage.Position = Position;
					player.TakeDamage( damage );
					EmitSound( "player.ship.damage" );
				} else if (other is Bullet bullet)
				{
					LoseStrength();
					bullet.LoseStrength();
				}
				else if ( other is Entity2D entity2D )
				{
					var damage = new DamageInfo();
					damage.Attacker = Owner;
					damage.Damage = 1f;
					damage.Position = Position;
					entity2D.TakeDamage( damage );
					EmitSound( "enemy.ship.damage" );
					this.Delete();
				}
			}


		}


	}
}
