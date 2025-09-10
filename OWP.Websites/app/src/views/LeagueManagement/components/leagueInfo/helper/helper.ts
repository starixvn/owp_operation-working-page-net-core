import { v4 as uuidv4 } from 'uuid';
import MaxbetConst from '../../../constants/MaxbetConst';
import { TCondition, TDataSource } from '../table/TableType';
import { TExpressionRow, TLeagueCategory } from '../component/Type';

const mapEditSettingFromDataSource = (dataSource: TDataSource) => {
  const {
    leagueSettingId,
    owpLeagueId,
    leagueGroupId,
    leagueGroup,
    league,
    spread_HDP_Value,
    spread_OU_Value,
    spreadShow,
    maxbetWD_HDP_Show,
    maxbetWK_HDP_Show,
    wD_NoOfHDP_Show,
    wK_NoOfHDP_Show,
    lstNote,
    remark2,
    comment,
    condition,
    shortLeague,
    isBlackList,
    noOfferLive,
    remark2WD,
    remark2WK
  } = dataSource;
  const result = {
    leagueSettingId,
    owpLeagueId,
    league,
    leagueGroupId,
    leagueGroup,
    spread_HDP: spread_HDP_Value?.toString(),
    spread_OU: spread_OU_Value?.toString(),
    spread: spreadShow,
    maxbetWD_HDP: maxbetWD_HDP_Show,
    wD_NoOfHDP: wD_NoOfHDP_Show,
    maxbetWK_HDP: maxbetWK_HDP_Show,
    wK_NoOfHDP: wK_NoOfHDP_Show,
    lstNote,
    remark2,
    comment,
    hasTv: condition.hasTv,
    hasIpTv: condition.hasIpTv,
    round: selectCondition(condition.round),
    division: selectCondition(condition.division),
    remark: condition.remark,
    shortLeague,
    isBlackList,
    noOfferLive,
    remark2WD,
    remark2WK
  };

  return result;
};

const selectCondition = (conditions: Array<TCondition>) =>
  conditions?.length
    ? conditions
    : [
        {
          type: MaxbetConst.expressionOption[0].label as 'Contains' | 'Equals',
          value: ''
        }
      ];

const convertResponseToCondition = (response: TCondition[]) => {
  return response.map(item => ({
    key: uuidv4(),
    condition: MaxbetConst.expressionOption.find(
      round => round.label === item.type
    ),
    value: item.value
  })) as TExpressionRow[];
};

const convertDivisionResponseToCondition = (response: TCondition[]) => {
  return response.map(item => ({
    key: uuidv4(),
    condition: MaxbetConst.divisionExpressionOption.find(
      round => round.label === item.type
    ),
    value: item.value
  })) as TExpressionRow[];
};

const convertResourceToOptions = (resource: TLeagueCategory[]) => {
  return resource.map(item => ({
    value: item.Id,
    label: item.Value
  }));
};

const convertDivisionResponseToDisplayValue = (response: TCondition[]) => {
  if (!response || response.length === 0){
    return '';
  }

  if(!response.some(condition => condition.type === 'Pairwise Contains')){
    return response.map(condition => (condition.type === 'Contains' ?
      `*${condition.value}*`
      : condition.value)).join('/');
  }
  const groupedBySecondValue: { [key: string]: string[] } = {};
  
  response.forEach(condition => {
    if (condition.type === 'Pairwise Contains' && condition.value) {
      const [first, second] = condition.value.split(',');
      if (first && second) {
        if (!groupedBySecondValue[second]) {
          groupedBySecondValue[second] = [];
        }
        groupedBySecondValue[second].push(first);
      }
    }
  });

  const resultParts: string[] = [];
  
  Object.keys(groupedBySecondValue).forEach(secondValue => {
    const firstValues = groupedBySecondValue[secondValue];
    const firstPart = firstValues.map(val => `*(${val})*`).join('/');
    const secondPart = `*(${secondValue})*`;
    resultParts.push(`${firstPart}-vs-${secondPart}`);
  });

  return resultParts.join(', ');
};

const hasInvalidDivision=(division: TCondition[])=>division.some(d => {
          const trimmedValue = d.value.trim();
          return d.type === MaxbetConst.divisionExpressionOption[1].label &&
            (trimmedValue.startsWith(',') ||
             trimmedValue.endsWith(',') ||
             (trimmedValue.match(/,/g) || []).length!==1 ||
             trimmedValue.includes(' ,') ||
             trimmedValue.includes(', '));
        });

export {
  convertResourceToOptions,
  convertResponseToCondition,
  mapEditSettingFromDataSource,
  convertDivisionResponseToCondition,
  convertDivisionResponseToDisplayValue,
  hasInvalidDivision
};
