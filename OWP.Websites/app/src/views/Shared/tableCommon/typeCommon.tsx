import { Option } from '../MultipleSelection/type';

type TFilterState = {
  columnKey: string | undefined;
  keyword: string | undefined;
};

type TFilterHeaderProps = {
  columnKey: string;
  options: Option[];
  model: any;
  handleClickFilterCallback: (value: string) => void;
  handleClearFilterCallback: () => void;
  handleFilterBySearchKeyCallback: (filterState: TFilterState) => void;
  handleValueFilterCallback: (filterState: TFilterState) => void;
};

type MultipleFilterHeaderProps = {
  options?: Option[];
  selected?: Option[];
  isFilter?: boolean;
  setOptions?: () => Option[];
  setSelected?: () => Option[];
  setIsFilter?: () => boolean;
  onFilter: (selected: Option[]) => void;
  onClear: () => void;
};

type MultipleFilterPopupProps = Omit<
  MultipleFilterHeaderProps,
  'columnKey' | 'isFilter' | 'getOptions'
> & {
  onClosedPopup: () => void;
};

export {
  TFilterState,
  TFilterHeaderProps,
  MultipleFilterPopupProps,
  MultipleFilterHeaderProps
};
