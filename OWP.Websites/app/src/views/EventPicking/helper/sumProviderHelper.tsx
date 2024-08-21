import { TMatchOfferInfo } from "../types/dataTableType";
import { ProviderSumType } from "../types/providerSumType";

export const groupAndCountByHour = (data: TMatchOfferInfo[], date: string) => {
  const offerMatches = data.filter(x => !x.isNoOffer).map(x => new Date(x.eventDateTime));
  return sumProvider(offerMatches, date);
};

const sumProvider = (data: Date[], date: string) => {
  const array = [] as Array<ProviderSumType>;
  let previousCount = 0;

  for (let i = 12; i < 36; i++) {
    const countDate = new Date(date);
    countDate.setHours(i);
    const filteredData = data.filter(x => x.getHours() === countDate.getHours());
    array.push({
      countDate,
      count: filteredData.length,
      periodCount: previousCount + filteredData.length
    });
    previousCount = filteredData.length;
  }

  return array;
};
