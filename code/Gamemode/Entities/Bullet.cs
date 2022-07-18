using Degg.Entities;
using Sandbox;

namespace ShipSurvivors
{
	public partial class Bullet : Entity2D
	{
		public int Strength { get; set; }
		public float LastStrenghtLostTick { get; set; }
		public float DeathTime { get; set; }
		public float Damage { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			Damage = 1;
			SetShape(Entity2DShapes.Circle);
			Scale = 0.5f;
			EntityMaterial = "materials/bullets/bullet_1.vmat";
			Tags.Add( "bullet" );
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

		public ShipPlayer GetShipPlayer()
		{
			if ( Owner is ShipWeapon w )
			{
				Owner = w.GetShipPlayer();
			} else if (Owner is ShipPlayer player)
			{
				return player;
			}
			return null;
		}

		public override void ServerTick()
		{
			base.ServerTick();
			if ( DeathTime < Time.Now )
			{
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
			if ( Owner != null )
			{
				if ( Owner is ShipWeapon w )
				{
					Owner = w.GetShipPlayer();
				}

				if ( Owner == other )
				{
					return;
				}
				if (other.Owner == Owner)
				{
					return;
				}

				if ( other is ShipPlayer player )
				{
					this.Delete();
					var damage = new DamageInfo();
					damage.Attacker = Owner;
					damage.Damage = Damage;
					damage.Position = Position;
					player.TakeDamage( damage );
					EmitSound( "player.ship.damage" );
					return;
				}
				if (other is Bullet bullet)
				{
					if ( GetShipPlayer() != bullet.GetShipPlayer() )
					{
						LoseStrength();
						bullet.LoseStrength();
						return;
					}
				}
				if ( other is EnemyShip enemy )
				{
					if (Owner is EnemyShip)
					{
						return;
					}
					
					if ( other.Health > 0 )
					{
						var damage = new DamageInfo();
						damage.Attacker = Owner;
						damage.Damage = Damage;
						damage.Position = Position;
						enemy.TakeDamage( damage );
						if ( Owner != null && Owner is ShipPlayer ownerPlayer )
						{
							bool didKill = false;
							if ( enemy.Health <= 0 )
							{
								didKill = true;
							}
							ownerPlayer.OnEnemyDamaged( this, enemy, didKill );
						}

						EmitSound( "enemy.ship.damage" );
						this.Delete();
						return;
					}
				}
			}


		}


	}
}
