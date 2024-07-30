import { KOTimeCellProps, PesOrFiFa } from '../type/TTaskListTable';
import FlagFactory from '../features/factory/FlagFactory';
import KOTime from '../features/component/KOTime';

const KOTimeToday = (props: KOTimeCellProps) => {
  const { dataSource, rowIndex } = props;
  const { IsFIFA, IsPESOther, IsPES23, EventDateDisplay, IsFC24 } = dataSource;

  const flagsProps: PesOrFiFa = {
    IsFIFA,
    IsPESOther,
    IsPES23,
    IsFC24,
    rowIndex
  };
  return (
    <KOTime
      eventDateElement={
        <span className='badge-label cancelled'>{EventDateDisplay}</span>
      }
      rowIndex={rowIndex}
      dataSource={dataSource}
      flagElement={<FlagFactory {...flagsProps} />}
    />
  );
};

export default KOTimeToday;
