import axios from 'axios';
import { every, chain, isNil } from 'lodash';
import { MajorBetTypes } from '../../Shared/Constants';
import { buildRequestURL } from '../buildUrlHelper';

const url = buildRequestURL('match-odd');

const transform = data => {
  return chain(data)
    .groupBy(item => item.Id)
    .map(groups => {
      let homeOdd, awayOdd, drawOdd;
      groups.forEach(group => {
        switch (group.Choice) {
          case 1:
            homeOdd = group.Odds;
            break;
          case 2:
            awayOdd = group.Odds;
            break;
          default:
            drawOdd = group.Odds;
            break;
        }
      });
      return {
        Id: groups[0].Id,
        BetType: groups[0].Bettype,
        HDP: groups[0].HDP,
        TeamChoice: groups[0].TeamChoice,
        HomeOdd: homeOdd,
        AwayOdd: awayOdd,
        DrawOdd: drawOdd
      };
    })
    .value();
};

const query = async model => {
  const response = await axios.get(url, {
    params: model
  });
  return transform(response.data);
};
const create = async model => {
  const response = await axios.post(url, model);
  return response.data;
};

const update = async model => {
  const response = await axios.put(url, model);
  return response.data;
};

const _delete = async model => {
  const response = await axios.delete(url, {
    data: model
  });
  return response.data;
};

const validate = model => {
  const { BetType, HDP, HomeOdd, AwayOdd, DrawOdd } = model;
  switch (BetType) {
    case MajorBetTypes.Handicap:
    case MajorBetTypes.Handicap1H:
    case MajorBetTypes.OverUnder:
    case MajorBetTypes.OverUnder1H:
      return every([HDP, HomeOdd, AwayOdd], item => !isNil(item));
    case MajorBetTypes.OneXTwo:
    case MajorBetTypes.OneXTwo1H:
      return every([HomeOdd, AwayOdd, DrawOdd], item => !isNil(item));
    default:
      return false;
  }
};

const service = {
  query,
  create,
  update,
  delete: _delete,
  validate
};

export default service;
