import { ChangeEvent } from 'react';

type TCommonType = string | number | null | boolean;
type TDefaultSelected = string | number | null;
type TInputCustom = string | number | null | undefined;

interface Option {
  value: number | string;
  label: string;
  key?: string;
  disabled?: boolean;
}

interface IMultipleSelectProps {
  label: Required<string>;
  options: Required<Option[]>;
  value: Option[];
  onChange?: (event: Option[]) => void;
  className?: string;
  isRequired?: boolean;
  disabled: boolean;
}

interface IStatus {
  isActive: boolean | undefined;
  onChange?: (event: boolean) => void;
}

interface IStatusProps {
  statusLeague: IStatus;
  statusMaxBet: IStatus;
}

type PopupType = 'add' | 'edit' | null;

interface ISelection {
  label: Required<string>;
  options: Option[];
  defaultValue: Option;
  onChange?: (value: string | number) => void;
  isRequired?: boolean;
  disabled: boolean;
}

interface IFieldSelection {
  disabled: boolean;
  value?: number | string | null;
  onChange?: (value: string | number) => void;
}

interface IFieldMultipleSelection {
  disabled: boolean;
  value?: Array<string | number> | null;
  onChange: (e: Option[]) => void;
}

interface ITimeRange {
  disabled: boolean;
  koFromTime: string | undefined;
  koToTime: string | undefined;
  onChange: (key: Required<string>, time: string | null) => void;
}

interface IInputMaxbetSpread {
  label: Required<string>;
  value?: number | null;
  isRequired?: boolean;
  max: number;
  min: number;
  disabled: boolean;
  onChange?: (event: string | null) => void;
}

interface IInputFieldMaxbetSpread {
  disabled: boolean;
  value?: number | null;
  onChange?: (event: string | null) => void;
}

interface IRemark {
  disabled: boolean;
  value?: string;
  onChange?: (event: string | null) => void;
}

interface IUpdateScore {
  disabled: boolean;
  isIptvOnly?: boolean;
  isManualUpdateScore?: boolean;
  onChange?: (key: Required<string>, event: boolean) => void;
}

interface ICheckbox {
  id: Required<string>;
  name: Required<string>;
  value: Required<string>;
  checked: Required<boolean>;
  onChange?: (
    key: Required<string>,
    event: ChangeEvent<HTMLInputElement>
  ) => void;
}

interface ILeagueContent {
  region: IFieldSelection;
  category: IFieldSelection;
  children?: JSX.Element;
}

type TLeagueName = {
  disabled: boolean;
  onChange: (event: string | null) => void;
};

export {
  Option,
  IMultipleSelectProps,
  IStatus,
  ISelection,
  IFieldSelection,
  IFieldMultipleSelection,
  ITimeRange,
  IInputMaxbetSpread,
  IInputFieldMaxbetSpread,
  IRemark,
  IUpdateScore,
  ICheckbox,
  PopupType,
  ILeagueContent,
  TCommonType,
  TDefaultSelected,
  TLeagueName,
  TInputCustom,
  IStatusProps
};
