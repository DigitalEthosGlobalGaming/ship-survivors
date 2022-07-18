using Sandbox;

namespace ShipSurvivors
{

	public partial class GenericWeaponUpgradeAttackSpeed : WeaponUpgrade 
	{
		public override bool CanBuyUpgrade()
		{
			var currentAmount = GetShipPlayer()?.GetUpgradeLevel( "GenericWeaponUpgradeAttackSpeed" );
			return currentAmount < 10;
		}
		public override void OnOwnerStatsUpdate()
		{
			base.OnOwnerStatsUpdate();
			var player = base.GetShipPlayer();
			if ( player?.IsValid() ?? false )
			{
				player.AttackSpeed = player.AttackSpeed + (0.05f * Level);
			}
		}

	}
}
