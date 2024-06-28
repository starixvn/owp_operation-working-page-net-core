import { isEqual } from 'lodash';
import { errorMessage } from '../osLeague.json';
import { LogQuery, LogValue, TErrorMessage } from '../typing/osLeagueStateType';
import { ISettingDetail } from '../typing/settingDetailType';
import { Option, TCommonType, TDefaultSelected } from '../typing/type';

const { required, duplicated } = errorMessage;
const getURL = (urlName: string) => `/api/OSLeague/${urlName}`;

const detectLogChanged = (item: LogQuery) => {
  const logChanged: Array<LogValue> = [];

  for (const key in item.oldValue) {
    if (item.oldValue[key] !== item.newValue[key]) {
      logChanged.push({
        title: 'Changed the setting',
        actionId: item.action,
        modifyBy: item.modifyBy,
        modifyDate: item.modifyDate,
        keyText: key,
        valueNew: item.newValue[key],
        valueOld: item.oldValue[key]
      });
    }
  }
  return logChanged;
};

const buildLogOnCreated = (item: LogQuery) => {
  const logChanged: Array<LogValue> = [];
  for (const key in item.oldValue) {
    logChanged.push({
      title: 'Created the setting',
      actionId: item.action,
      modifyBy: item.modifyBy,
      modifyDate: item.modifyDate,
      keyText: key,
      valueNew: item.newValue[key],
      valueOld: item.oldValue[key]
    });
  }
  return logChanged;
};

const formatLeagueDetailResponse = (
  source: Array<{ [key: string]: TCommonType }>
) => {
  if (source.length) {
    const firstItem = source[0];
    return {
      ...firstItem,
      betDelay: (firstItem.betDelay as string)?.split(',').filter(x => x) ?? [],
      betTypes: (firstItem.betTypes as string)?.split(',').filter(x => x) ?? [],
      rounds: (firstItem.rounds as string)?.split(',').filter(x => x) ?? [],
      offerLines:
        (firstItem.offerLines as string)?.split(',').filter(x => x) ?? [],
      providers:
        (firstItem.providers as string)?.split(',').filter(x => x) ?? []
    } as ISettingDetail;
  }
  return null;
};

const buildDefaultSelected = (options: Option[], value?: TDefaultSelected) => {
  let defaultValue = options.find(o => isEqual(o.value, value));
  if (!defaultValue || !defaultValue.value) {
    defaultValue = {
      ...options[0],
      label: ''
    } as Option;
  }
  return defaultValue;
};

const buildPostModelAddOrSaveSetting = (source: ISettingDetail) => ({
  ...source,
  betDelay: joinArrayWithComma(source.betDelay),
  rounds: joinArrayWithComma(source.rounds),
  offerLines: joinArrayWithComma(source.offerLines),
  providers: joinArrayWithComma(source.providers),
  betTypes: joinArrayWithComma(source.betTypes)
});

const joinArrayWithComma = (
  source?: Array<string | number> | null | undefined
) => {
  return source ? source.join(',') : '';
};

const hasAnyFieldIsRequired = (source: ISettingDetail) => {
  const { leagueName, offerLines, betDelay, maxbetHDP, maxbetOU, betTypes } =
    source;

  const requiredMaxbet = !maxbetHDP && !maxbetOU;

  return (
    !leagueName ||
    !betTypes?.length ||
    !betDelay?.length ||
    !offerLines?.length ||
    requiredMaxbet
  );
};

const setDuplicateField = (): TErrorMessage => {
  const message: TErrorMessage = {
    leagueName: null,
    offerLines: null,
    betDelay: null,
    maxbetHDP: null,
    maxbetOU: null,
    betTypes: null,
    provider: null,
    rangeTime: null,
    round: null,
    iptv: null,
    remark: null,
    valid: false
  };
  return {
    ...message,
    leagueName: duplicated,
    round: duplicated,
    offerLines: duplicated,
    betDelay: duplicated,
    provider: duplicated,
    rangeTime: duplicated,
    iptv: duplicated,
    remark: duplicated
  };
};

const validateRequiredField = (source: ISettingDetail): TErrorMessage => {
  let message: TErrorMessage = {
    leagueName: null,
    offerLines: null,
    betDelay: null,
    maxbetHDP: null,
    maxbetOU: null,
    betTypes: null,
    provider: null,
    rangeTime: null,
    iptv: null,
    remark: null,
    round: null,
    valid: true
  };

  const { leagueName, offerLines, betDelay, maxbetHDP, maxbetOU, betTypes } =
    source;

  const requiredMaxbet = !maxbetHDP && !maxbetOU ? required : null;

  if (hasAnyFieldIsRequired(source)) {
    message = {
      ...message,
      leagueName: !leagueName ? required : null,
      offerLines: !offerLines?.length ? required : null,
      betDelay: !betDelay?.length ? required : null,
      maxbetHDP: requiredMaxbet ? required : null,
      maxbetOU: requiredMaxbet ? required : null,
      betTypes: !betTypes?.length ? required : null,
      valid: false
    };
  }
  return message;
};

const numberWithCommas = (source: number) =>
  source.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');

export {
  getURL,
  formatLeagueDetailResponse,
  detectLogChanged,
  buildLogOnCreated,
  buildDefaultSelected,
  buildPostModelAddOrSaveSetting,
  setDuplicateField,
  validateRequiredField,
  numberWithCommas
};
