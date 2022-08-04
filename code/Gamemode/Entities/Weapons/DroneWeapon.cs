using Sandbox;
using System.Collections.Generic;

namespace ShipSurvivors
{
	public partial class DroneWeapon : Upgrade
	{

		public int MaxDrones { get; set; }
		public List<DroneBasic> Drones { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			Drones = new List<DroneBasic>();
			MaxDrones = 1;
		}
		public int CountDrones()
		{
			if (Drones == null)
			{
				return 0;
			}
			var amount = 0;
			foreach ( var item in Drones )
			{
				if (item?.IsValid() ?? false)
				{
					item.ParentUpgrade = this;
					amount = amount + 1;
				}
			}
			return amount;
		}
		public void TryToSpawnDrone()
		{
			if ( CountDrones() < MaxDrones)
			{
				SpawnDrone();
			}
		}

		public override void ServerTick()
		{
			base.ServerTick();
			Log.Info( NetworkIdent);
			TryToSpawnDrone();
		}

		public void SpawnDrone()
		{
			var newDrone = CreateByName<DroneBasic>( "DroneBasic" );
			Drones.Add( newDrone );
		}

		public override void OnUnEquip()
		{
			base.OnUnEquip();
			foreach ( var item in Drones )
			{
				item?.Delete();
			}
		}

	}
}
