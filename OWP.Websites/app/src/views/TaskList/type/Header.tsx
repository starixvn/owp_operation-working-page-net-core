type THeader = {
  refreshComponent?: JSX.Element;
};
type RefreshModel = {
  filterDate: Date | undefined;
  isToday: boolean;
};

export { THeader, RefreshModel };
