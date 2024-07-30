import { CellProps } from '../../type/TTaskListTable';
import ReadyCell from '../table/cell/ReadyCell';
import ReadyCheckboxCell from '../table/cell/ReadyCheckboxCell';

const ReadyFactory = (props: CellProps): JSX.Element | null => {
  const { rowIndex } = props;
  const { IsReady, ...rest } = props.dataSource;

  const cellProps = {
    rowIndex: props.rowIndex,
    readyDateTimeDisplay: rest.ReadyDateTimeDisplay
  };

  return IsReady ? (
    <ReadyCell {...cellProps} />
  ) : (
    <ReadyCheckboxCell
      rowIndex={rowIndex}
      owpMatchId={props.dataSource.OWPMatchId}
    />
  );
};

export default ReadyFactory;
