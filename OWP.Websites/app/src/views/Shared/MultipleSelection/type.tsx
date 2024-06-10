
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
  Option,
  IMultipleSelectProps,
};
