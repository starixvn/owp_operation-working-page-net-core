using OWP.OtherSport.Submit.Models;
using System.Collections.Generic;

namespace OWP.OtherSport.EventAssignment.Models
{
	public class TraderMaxBetModel
	{
		public string BetTypeName { get; set; }

		public TraderAssignment TraderAssignment { get; set; } = new TraderAssignment();

		public MatchSetting Maxbet { get; set; } = new MatchSetting();
	}
}
