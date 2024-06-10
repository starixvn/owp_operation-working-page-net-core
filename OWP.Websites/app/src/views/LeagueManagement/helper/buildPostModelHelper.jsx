import MaxbetConst from '../constants/MaxbetConst';

const buildModelLeagueDetail = (model, key, value) => {
  const postModel = { ...model };
  delete postModel.HasTV;
  delete postModel.Round;
  delete postModel.Division;
  delete postModel.TV;
  return {
    ...postModel,
    [key]: value,
    apiKey: MaxbetConst.leagueManagementUpdateLeagueSetting
  };
};

const buildPostModelUpdateSpecialProduct = (model, key, value) => {
  const postModel = { ...model };
  return {
    ...postModel,
    [key]: value
  };
};

export { buildModelLeagueDetail, buildPostModelUpdateSpecialProduct };
