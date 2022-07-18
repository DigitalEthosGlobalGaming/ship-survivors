using Degg.Core;
using Degg.Util.CurrencySystem;
using Degg.Util.RoundSystem;
using Degg.Utils;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipSurvivors
{

	public partial class CurrencySystem : DeggCurrency
	{
		[Net]
		public static CurrencySystem Current { get; set; }
		public CurrencySystem()
		{
			Current = this;
		}

	}

}
