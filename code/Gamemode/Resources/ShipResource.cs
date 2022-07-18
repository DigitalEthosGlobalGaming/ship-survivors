using Sandbox;
using System.Collections.Generic;

namespace ShipSurvivors
{
	[GameResource( "Ship Definition", "ship", "Describes an ship" )]
	public partial class ShipResource : GameResource 
	{
		public string ShipClassName { get; set; }
		public string ShipName { get; set; }
		public string Description { get; set; }
		public bool Active { get; set; }
		[ResourceType( "png" )]
		public string Image { get; set; }
		[ResourceType( "vmat" )]
		public string Material { get; set; }

		[ResourceType( "upgrade" )]
		public List<string> Upgrades { get; set; }
		[ResourceType( "upgrade" )]
		public List<string> StartingUpgrades { get; set; }

		public static List<ShipResource> GetAll()
		{
			var resources = ResourceLibrary.GetAll<ShipResource>();
			var results = new List<ShipResource>();
			foreach ( var item in resources )
			{
				results.Add( item );
			}

			return results;
		}

		public static ShipResource GetResourceForShipPlayer(ShipPlayer t)
		{
			var resources = GetAll();
			foreach ( var item in resources )
			{
				if ( item.ShipClassName == t.ClassName )
				{
					return item;
				}
			}

			return null;
		}
	}
}
