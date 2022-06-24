using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class WeaponUpgrade : Upgrade 
	{
		public T GetWeapon<T>() where T: ShipWeapon
		{
			var parentUpgrade = GetParentUpgrade();
			if ( parentUpgrade is T t)
			{
				return t;
			}
			return null;
		}

	}
}
