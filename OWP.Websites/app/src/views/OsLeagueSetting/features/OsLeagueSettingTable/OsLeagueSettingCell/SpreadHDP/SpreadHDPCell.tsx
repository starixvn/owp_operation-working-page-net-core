import { CellProps } from '../../../../typing/tableType';

const SpreadHDPCell = (props: CellProps): JSX.Element => {
  const { dataSource, rowIndex } = props;
  const { spreadHDP } = dataSource[rowIndex];
  return (
    <div className='spread-hdp-cell'>
      <span className='spread-value'>{spreadHDP ? `${spreadHDP}%` : ''}</span>
    </div>
  );
};

export default SpreadHDPCell;
