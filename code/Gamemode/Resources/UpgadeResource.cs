using Sandbox;
using System;
using System.Collections.Generic;

namespace ShipSurvivors
{
	[GameResource( "Upgrade Definition", "upgrade", "Describes an upgrade" )]
	public partial class UpgradeResource : GameResource 
	{

		public static Dictionary<string, UpgradeResource> Resources = new Dictionary<string, UpgradeResource>();

		public string ClassName { get; set; }
		public string UpgradeName { get; set; }
		public string Description { get; set; }

		[ResourceType( "png" )]
		public string Image { get; set; }

		[ResourceType( "upgrade" )]
		public List<string> ChildrenUpgrades { get; set; }

		public static List<UpgradeResource> GetAll()
		{
			var resources = ResourceLibrary.GetAll<UpgradeResource>();
			var results = new List<UpgradeResource>();
			foreach ( var item in resources )
			{
				results.Add( item );
			}

			return results;
		}

		public List<UpgradeResource> GetChildrenUpgradeResources()
		{
			var res = new List<UpgradeResource>();

			if ( ChildrenUpgrades != null )
			{
				foreach ( var item in ChildrenUpgrades )
				{
					res.Add( Get(item) );
				}
			}
			return res;

		}

		public static UpgradeResource Get(string path)
		{
			var resource = ResourceLibrary.Get<UpgradeResource>( path );
			return resource;
		}

		public static UpgradeResource GetResourceForUpgrade( Upgrade t )
		{
			var resources = GetAll();
			foreach ( var item in resources )
			{
				if ( item.ClassName == t.ClassName )
				{
					return item;
				}
			}

			return null;
		}
	}
}
