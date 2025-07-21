import { useRef } from 'react';
import { CellProps } from '../../../type/TableType';
import { UncontrolledTooltip } from 'reactstrap';
import TimeOfMatch from '../../../../Shared/LiveGameShared/TimeOfMatch';

const KOTimeCell = ({ rowIndex, dataSource }: CellProps): JSX.Element => {
  const ref = useRef(null);
  const {
    KickOffTimeDisplay,
    KickOffTime,
    OtherTimeDisplay,
    OtherTime,
    InfoSiteTimeDisplay,
    InfoSiteTime,
    IsPES23,
    IsFC24,
    IsPESOther,
    IsFIFA,
    IsPES24
  } = dataSource[rowIndex];

  const propsTime = {
    KickOffTimeDisplay,
    KickOffTime,
    OtherTimeDisplay,
    OtherTime,
    InfoSiteTimeDisplay,
    InfoSiteTime,
    rowIndex
  };
  console.log(IsPESOther);
  console.log(IsPES23);
  return (
    <div className='ko-time-cell'>
      <TimeOfMatch {...propsTime} />
      {IsFC24 && (
        <>
          <i
            className='icon-match icon-fc24'
            original-title='FC 24'
            ref={ref}></i>
          <UncontrolledTooltip placement='bottom' target={ref}>
            FC 24
          </UncontrolledTooltip>
        </>
      )}
      {(IsPESOther || IsPES23) && (
        <>
          <i
            className={`icon-match ${IsPES23 ? 'icon-pes23' : 'icon-pes'}`}
            original-title='PES'
            ref={ref}></i>
          <UncontrolledTooltip placement='bottom' target={ref}>
            PES
          </UncontrolledTooltip>
        </>
      )}
      {IsFIFA && (
        <>
          <i
            className='icon-match icon-fifa'
            ref={ref}
            original-title='FIFA'></i>
          <UncontrolledTooltip placement='bottom' target={ref}>
            FIFA
          </UncontrolledTooltip>
        </>
      )}
      {IsPES24 && (
        <>
          <i
            className='icon-match icon-pes24'
            original-title='VLeague'
            ref={ref}></i>
          <UncontrolledTooltip placement='bottom' target={ref}>
            PES 24
          </UncontrolledTooltip>
        </>
      )}
    </div>
  );
};

export default KOTimeCell;
