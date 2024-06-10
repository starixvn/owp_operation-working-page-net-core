import { UncontrolledTooltip } from 'reactstrap';

type TProps = {
  rowIndex: number;
  readyDateTimeDisplay: string;
};

const ReadyCell = (props: Readonly<TProps>) => {
  const { readyDateTimeDisplay, rowIndex } = props;
  return (
    <>
      <i id={`ready-cell-${rowIndex}`} className='icon-success icon-green'></i>
      <UncontrolledTooltip target={`ready-cell-${rowIndex}`}>
        Ready at {readyDateTimeDisplay}
      </UncontrolledTooltip>
    </>
  );
};

export default ReadyCell;
