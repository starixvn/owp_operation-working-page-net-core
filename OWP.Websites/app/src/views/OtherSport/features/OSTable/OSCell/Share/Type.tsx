interface MatchSetting {
  betTypeName: Required<string>,
  maxBet: number,
  order: number
  spread: string,
}

interface TraderAssignment {
  assignmentNote: string,
  mainTrader: string,
}

interface TraderMaxbetModel {
  hiden: Required<boolean>,
  betTypeName: string,
  matchSettings: Array<MatchSetting>,
  traderAssignment: TraderAssignment
}

interface DataSource {
  autoLink: string,
  away: Required<string>,
  category: string,
  dateTime: Required<string>,
  home: Required<string>,
  iptv: string,
  league: Required<string>,
  matchNo: number,
  matchOffer: string,
  matchStatus: string,
  noHDP: string,
  osMatchId: Required<number>,
  otherProvider: string,
  provider: string,
  remark: string,
  reporter: string,
  service: string,
  sport: Required<string>,
  traderMaxbetModels: Array<TraderMaxbetModel>
}

export {
  DataSource,
  MatchSetting,
  TraderAssignment,
  TraderMaxbetModel
};
