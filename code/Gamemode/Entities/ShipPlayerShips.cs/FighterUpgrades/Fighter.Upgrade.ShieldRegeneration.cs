namespace ShipSurvivors
{

	public partial class FigherUpgradeShieldRegeneration : WeaponUpgrade 
	{
		public override void OnRoundStart()
		{
			var regenLevel = Level;
			var ship = GetShipPlayer();
			if ( regenLevel > 0 )
			{
				ship.Health = ship.Health + regenLevel;
				if ( ship.Health > ship.MaxHealth )
				{
					ship.Health = ship.MaxHealth;
				}
			}
		}
	}
}
