import {
  IMultipleSelectProps,
  Option
} from '../../Shared/MultipleSelection/type';
type TTimeRange = {
  fromDate: Required<string>;
  toDate: Required<string>;
};
type TCommonFilterInfo = string | (number | string | null)[];
type TFilter = {
  [x: string]: TCommonFilterInfo | TPeriodFilter;
};

type TProviderSelectionTemplate = {
  label: Required<string>;
  filterKey: Required<string>;
};

type TPayloadFilter = {
  key: Required<string>;
  value: TCommonFilterInfo;
};

type TPeriodFilter = {
  PeriodOneMonth?: TTimeRange;
  PeriodThreeMonths?: TTimeRange;
  PeriodSixMonths?: TTimeRange;
  PeriodTwelveMonths?: TTimeRange;
};

type TMultipleSelectionTemplate = IMultipleSelectProps & {
  filterKey: Required<string>;
};
type TurnoverMarginSelection = Option & {
  key: Required<string>;
  fromDate: Required<string>;
  toDate: Required<string>;
  order: number;
};

export {
  TurnoverMarginSelection,
  TFilter,
  TPayloadFilter,
  TMultipleSelectionTemplate,
  TProviderSelectionTemplate,
  TPeriodFilter
};
