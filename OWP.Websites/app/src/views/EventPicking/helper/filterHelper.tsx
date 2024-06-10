import { netCoreURL, netFwURL } from '../const/const.json';

const currentDate = new Date();
const timeRange = {
  maxFeature: 10,
  maxPrevious: 20
};
const maxDate = new Date(
  currentDate.setDate(currentDate.getDate() + timeRange.maxFeature)
);
const minDate = new Date(
  currentDate.setDate(currentDate.getDate() - timeRange.maxPrevious)
);
const buildUrl = (methodRoute: Required<string>) =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${methodRoute}`
    : `${netFwURL}/${methodRoute}`;

const handleCheckButton = (leagueName: string) => {
  const url = `LeagueManagement?leagueName=${leagueName}&isEqualLeague=true`;
  window.open(url, '_blank');
};

export { maxDate, minDate, buildUrl, handleCheckButton };
