import { CellProps } from '../../../../typing/tableType';

const NoCell = (props: CellProps): JSX.Element => {
  const { rowIndex } = props;

  return (
    <div className='no-cell'>
      <span>{rowIndex + 1}</span>
    </div>
  );
};

export default NoCell;
