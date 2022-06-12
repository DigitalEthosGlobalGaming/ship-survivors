using Degg;
using Degg.UI.Elements;
using Degg.Util;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;
using static Degg.Util.RoundSystem.Round;

namespace ShipSurvivors
{
	public class UpgradePanel: DeggPanel
	{	
		public Panel UpgradeModal { get; set; }
		public Panel UpgradeIconsPanel { get; set; }
		public RoundState PreviousRoundState { get; set; }
		public Panel Cards { get; set; }
		public Panel CardIcons { get; set; }
		public bool HasUpgradesToBuy { get; set; }

		public UpgradePanel()
		{

			SetTemplate( "/Gamemode/Ui/UpgradePanel.html" );
			StyleSheet.Load( "/Gamemode/Ui/UpgradePanel.scss" );
			SetupElements();
			UpdateCards();
			StyleSheet.Load( "/Gamemode/Ui/styles.scss" );
		}

		public void SetupElements()
		{
			UpgradeModal = AddChild<Panel>( "upgrade-panel" );
			Cards = UpgradeModal.AddChild<Panel>( "inner" ).AddChild<Panel>( "cards" );
			UpgradeIconsPanel = AddChild<Panel>("upgrade-icons-panel");
		}

		public List<Upgrade> GetUpgradesToBuy()
		{
			List<Upgrade> upgrades = new List<Upgrade>();
			var player = ClientUtil.GetPawn<ShipPlayer>();

			foreach ( var upgrade in player.UpgradesToBuy )
			{
				upgrades.Add( upgrade );
			}
			return upgrades;
		}

		public List<Upgrade> GetUpgrades()
		{
			List<Upgrade> upgrades = new List<Upgrade>();
			var player = ClientUtil.GetPawn<ShipPlayer>();

			foreach ( var upgrade in player.Upgrades )
			{
				upgrades.Add( upgrade );
			}
			return upgrades;
		}


		[Event( "ss.upgrades.change" )]
		public void UpdateCards()
		{
			var upgrades = GetUpgrades();


			if ( CardIcons != null )
			{
				CardIcons.Delete();
			}
			CardIcons = UpgradeIconsPanel.AddChild<Panel>();
			CardIcons.AddClass( "cards-icons" );

			Dictionary<string, UpgradeIconElement> icons = new Dictionary<string, UpgradeIconElement>();
			foreach ( var icon in icons )
			{
				icon.Value.Amount = 0;
			}

			foreach ( var upgrade in upgrades )
			{
				if ( !icons.ContainsKey(upgrade.Name))
				{
					var element = CardIcons.AddChild<UpgradeIconElement>();
					icons[upgrade.Name] = element;
				}
				icons[upgrade.Name].Amount = icons[upgrade.Name].Amount + 1;
			}
			AdvLog.Info( upgrades );
		}

		[Event( "ss.upgrades-to-buy.change" )]
		public void UpdateCardsToBuy()
		{
			var upgrades = GetUpgradesToBuy();

			if ( Cards != null )
			{
				Cards.Delete();
			}
			Cards = UpgradeModal.AddChild<Panel>();
			Cards.AddClass( "cards" );

			foreach (var upgrade in upgrades )
			{
				var element = Cards.AddChild<UpgradeItemElement>();
				element.SetUpgrade( upgrade );
			}

			HasUpgradesToBuy = upgrades.Count > 0;
		}


		[Event("ss.rounds.end")]
		public void OnRoundEnd()
		{
			UpdateCards();
		}

		[Event( "ss.rounds.start" )]
		public void OnRoundStart()
		{
			UpdateCards();
		}


		public void RoundEndTick()
		{
			var player = ClientUtil.GetPawn<ShipPlayer>();
			if (player?.IsValid() ?? false)
			{
			}
		}

		public void OnRoundChange()
		{
			
		}
		public override void Tick()
		{

			base.Tick();
			var roundManager = MyGame.GetRoundManager();
			if ( PreviousRoundState != roundManager.State)
			{
				PreviousRoundState = roundManager.State;
				OnRoundChange();
			}

			UpgradeModal.SetClass( "hidden", roundManager.State != RoundState.Ended || !HasUpgradesToBuy );

			switch ( roundManager.State )
			{
				case RoundState.Warmup:
					break;
				case RoundState.InProgress:
					break;
				case RoundState.Paused:
					break;
				case RoundState.Ended:
					RoundEndTick();
					break;
			}
		}

	}
}
