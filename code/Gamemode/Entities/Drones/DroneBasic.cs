using Degg.Entities;

namespace ShipSurvivors
{
	public partial class DroneBasic: Entity2D
	{

		public Upgrade ParentUpgrade = null;
		public override void Spawn()
		{
			base.Spawn();
			Scale = 0.5f;
		}

		public Upgrade GetParent()
		{
			var parent = ParentUpgrade;
			if (parent is Upgrade u)
			{
				return u;
			}
			return null;
		}
		public ShipPlayer GetShipPlayer()
		{
			return GetParent()?.GetShipPlayer();
		}


		public void OnMove()
		{
			var parent = GetShipPlayer();
			if ( parent?.IsValid ?? false ) {
				var parentPosition = parent.Position;
				var distance = parentPosition.Distance( Position );
				if (distance > 30)
				{
					MoveTo( parentPosition + (parentPosition - Position).Normal * distance, 2 );
				}
			}
		}
		public override void ServerTick()
		{
			base.ServerTick();
			OnMove();

		}
	}
}
