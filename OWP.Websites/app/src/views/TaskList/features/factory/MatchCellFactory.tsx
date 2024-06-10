import { CellProps } from '../../type/TTaskListTable';
import MatchContainLinkDetailCell from '../table/cell/MatchContainLinkDetailCell';
import MatchDefaultCell from '../table/cell/MatchDefaultCell';

const MatchCellFactory = (props: CellProps) => {
  const { dataSource, hasDetailInformationPermission } = props;
  const { IsAllowedToViewDetail } = dataSource;
  return hasDetailInformationPermission && IsAllowedToViewDetail ? (
    <MatchContainLinkDetailCell {...props} />
  ) : (
    <MatchDefaultCell {...props} />
  );
};

export default MatchCellFactory;
