import { CellProps } from '../../../../typing/tableType';

const SpreadCell = (props: CellProps): JSX.Element => {
  const { dataSource, rowIndex } = props;
  const { spreadHDP, spreadOU } = dataSource[rowIndex];
  return (
    <div className='spread-cell'>
      <span className='hdp-spread'>{spreadHDP && `HDP: ${spreadHDP}%`}</span>
      <span className='ou-spread'>{spreadOU && `OU:${spreadOU}%`}</span>
    </div>
  );
};

export default SpreadCell;
