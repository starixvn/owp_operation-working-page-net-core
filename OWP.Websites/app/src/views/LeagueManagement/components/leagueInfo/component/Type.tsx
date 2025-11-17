import { TCondition } from '../table/TableType';
import { MouseEvent } from 'react';

type TOption = {
  value: number | null;
  label: string | null;
};

type TAddExpressionKey = {
  round: Array<TCondition>;
  division: Array<TCondition>;
};

type TEditSetting = {
  leagueGroupOptions: TOption[];
  loading: string;
  leagueSettingId: number;
  league: string;
  owpLeagueId?: number;
  leagueGroupId: number;
  leagueGroup: string;
  spread: string;
  maxbetWD_HDP: string;
  wD_NoOfHDP: string;
  maxbetWK_HDP: string;
  wK_NoOfHDP: string;
  lstNote: string;
  remark2: string;
  hasTv: boolean;
  hasIpTv: boolean;
  remark: string;
  isBlackList: boolean;
  noOfferLive: number;
  comment: string;
  shortLeague?: string;
} & TAddExpressionKey;

type TAddSetting = {
  leagueName: string;
} & TEditSetting;

type TUpdateSettingSender = Omit<
  TEditSetting,
  'leagueGroupOptions' | 'loading'
>;

type TExpressionRow = {
  key: string;
  condition: {
    value: number;
    label: string;
  };
  value: string;
};

type TLeagueCategory = {
  RefName: string;
  Id: number;
  Value: string;
};
interface RowExpressionProps {
  expressionList: Array<TExpressionRow>;
  title?: string;
  property: 'round' | 'division';
  handleChangeExpression: (
    selectedOption: TOption | null,
    property: 'round' | 'division',
    index: number
  ) => void;
  handleUpdateValueExpression: (
    property: 'round' | 'division',
    key: 'value',
    value: string,
    index: number
  ) => void;
  handleAddRow: (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division'
  ) => void;
  handleDeleteRow: (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division',
    index: number
  ) => void;
}

type TExpectedResponseType = {
  message: string;
  duplicatedLeagueName: string;
};


interface Option {
  value: number | string | null;
  label: string;
  key?: string;
  disabled?: boolean;
}


interface IMultipleSelectProps {
  label: Required<string>;
  options: Required<Option[]>;
  value: Option[];
  className?: string;
  isRequired?: boolean;
  disabled: boolean;
  disableSearch?: boolean;
  onChange?: (event: Option[]) => void;
  onValueRenderer?: (newSelected: Option[], _options: Option[]) => string
  onShowDropdownListOption?: (value: boolean) => void
}

export {
  TEditSetting,
  TExpressionRow,
  TOption,
  TLeagueCategory,
  TUpdateSettingSender,
  TAddExpressionKey,
  TAddSetting,
  RowExpressionProps,
  TExpectedResponseType,
  IMultipleSelectProps
};
