type TDataSource = {
  infoSiteMatchId: Required<number>;
  childMatchId: number | null;
  isMovedEventDate: boolean;
  isCheck: boolean;
};

type TNoOfferMatch = TDataSource & { isNoOffer: boolean };

type TNoOfferMainMatch = {
  dataSource: ReadonlyArray<TDataSource>;
  source: TNoOfferMatch;
  matchNoOffer: Array<TNoOfferMatch>;
};

type TNoOfferChildMatch = {
  source: TNoOfferMatch;
  matchNoOffer: Array<TNoOfferMatch>;
};

export { TNoOfferMatch, TNoOfferMainMatch, TNoOfferChildMatch, TDataSource };
