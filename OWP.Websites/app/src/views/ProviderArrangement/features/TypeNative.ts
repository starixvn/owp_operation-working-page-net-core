type ProviderData = {
  id: number;
  order: number;
  value: string;
};

type TQueryLogProviderArrangement = {
  action: string;
  oldValue: Array<string> | null;
  newValue: Array<string>;
  modifyDate: string | null;
  modifyBy: string | null;
};

export { ProviderData, TQueryLogProviderArrangement };
