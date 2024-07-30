type SelectOption = {
  value: number;
  label: string;
};

type LocationOption = {
  value: string;
  label: string;
};

type OverTimeDataByYear = {
  trader: string;
  location: string;
  overtimeData: Array<number | null>;
  total: number | null;
};

type FilterModel = {
  trader: string | undefined;
  location: string;
  month: number;
  year: number;
};

export { SelectOption, OverTimeDataByYear, FilterModel, LocationOption };
