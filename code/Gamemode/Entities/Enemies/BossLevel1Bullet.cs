using Degg.Entities;
using Sandbox;

namespace ShipSurvivors
{
	public partial class BossLevel1Bullet : Bullet
	{

		public float TimeToMove { get; set; }
		public bool HasMoved { get; set; }

		public ShipPlayer Target { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			TimeToMove = Time.Now + 5f;
			Damage = 2;
			SetShape(Entity2DShapes.Circle);
			Scale = 0.5f;
			EntityMaterial = "materials/bullets/bullet_1.vmat";
			CollisionGroup = CollisionGroup.Trigger;
			Strength = 5;
			DeathTime = Time.Now + 10f;
		}

		public override void ServerTick()
		{
			base.ServerTick();
			if ( DeathTime < Time.Now )
			{
				Delete();
			}

			if (TimeToMove < Time.Now && !HasMoved && (Target?.IsValid() ?? false) )
			{
				DeathTime = Time.Now + 10f;
				HasMoved = true;
				LookAt( Target.Position );
				PhysicsBody.Velocity = Rotation.Forward * 100f;
			}
		}

	}
}
