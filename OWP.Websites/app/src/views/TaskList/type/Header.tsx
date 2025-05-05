type THeader = {
  refreshComponent?: JSX.Element;
  hasDetailInformationPermission?: boolean;
  firstMatchCheckedIsSaba?: boolean;
};
type RefreshModel = {
  filterDate: Date | undefined;
  isToday: boolean;
};

export { THeader, RefreshModel };
