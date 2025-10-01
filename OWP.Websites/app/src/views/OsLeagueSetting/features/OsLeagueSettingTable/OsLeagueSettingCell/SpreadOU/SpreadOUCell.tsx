import { CellProps } from '../../../../typing/tableType';

const SpreadOUCell = (props: CellProps): JSX.Element => {
  const { dataSource, rowIndex } = props;
  const { spreadOU } = dataSource[rowIndex];
  return (
    <div className='spread-ou-cell'>
      <span className='spread-value'>{spreadOU ? `${spreadOU}%` : ''}</span>
    </div>
  );
};

export default SpreadOUCell;
