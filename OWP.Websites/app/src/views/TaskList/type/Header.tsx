type THeader = {
  refreshComponent?: JSX.Element;
  hasDetailInformationPermission?: boolean;
};
type RefreshModel = {
  filterDate: Date | undefined;
  isToday: boolean;
};

export { THeader, RefreshModel };
