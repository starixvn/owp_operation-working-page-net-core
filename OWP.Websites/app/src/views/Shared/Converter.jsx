import { v4 as uuidv4 } from 'uuid';
import MaxbetConst from '../LeagueManagement/constants/MaxbetConst';

const convertTVsToOptions = tvs => {
  return tvs.map(item => ({
    value: item.TVId,
    label: item.TVCode
  }));
};

const convertProvidersToOptions = providers => {
  return providers.map(item => ({
    value: item.ProviderId,
    label: item.ProviderName
  }));
};

const convertOtherProvidersToOptions = otherProviders => {
  return otherProviders.map(item => ({
    value: item.OtherProviderId,
    label: item.OtherProviderName
  }));
};

const convertAgentsToOptions = agents => {
  return agents.map(item => ({
    value: item.AgentId,
    label: item.AgentName
  }));
};

const convertMatchOffersToOptions = matchOffers => {
  return matchOffers.map(item => ({
    value: item.Id,
    label: item.Value
  }));
};

const convertResourceToOptions = resource => {
  return resource.map(item => ({
    value: item.ResourceId,
    label: item.Value
  }));
};

const convertResponseToTVList = response => {
  if (response.length) {
    return response.map(item => ({
      key: uuidv4(),
      expression: MaxbetConst.expressionOption.find(
        tv => tv.label === item.Label
      ),
      value: item.Value
    }));
  } else {
    return [
      {
        key: uuidv4(),
        expression: MaxbetConst.expressionOption[0],
        value: ''
      }
    ];
  }
};

const convertResponseToRound = response => {
  if (response.length) {
    return response.map(item => ({
      key: uuidv4(),
      expression: MaxbetConst.expressionOption.find(
        round => round.label === item.Type
      ),
      value: item.Value
    }));
  } else {
    return [
      {
        key: uuidv4(),
        expression: MaxbetConst.expressionOption[0],
        value: ''
      }
    ];
  }
};

const convertNoOfHDPToOptions = items => {
  const options = [{ value: 0, label: '<None>' }];
  items.forEach(element => {
    options.push({ value: element.Id, label: element.Value });
  });
  return options;
};

const convertBettypesToOptions = bettypes =>
  bettypes.map(bettype => ({
    value: bettype.BettypeId,
    label: bettype.ShortName
  }));

const Converter = {
  convertTVsToOptions,
  convertProvidersToOptions,
  convertOtherProvidersToOptions,
  convertAgentsToOptions,
  convertMatchOffersToOptions,
  convertNoOfHDPToOptions,
  convertBettypesToOptions,
  convertResourceToOptions,
  convertResponseToTVList,
  convertResponseToRound
};

export default Converter;
