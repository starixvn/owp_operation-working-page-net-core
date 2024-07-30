import { UncontrolledTooltip } from 'reactstrap';
import { CellProps } from '../../../type/TTaskListTable';
import AbsencePersonIcon from '../../../../../assets/svg-element/AbsencePersonIcon';

const NoCell = ({ rowIndex, dataSource }: CellProps) => {
  const { MatchNo, MatchStatus, IsShowAbsenceIcon } = dataSource;
  const renderIcon = () => {
    const classNames: { [key: string]: string } = {
      'Live': 'live',
      'In-play': 'inplay',
      'Completed': 'completed',
      'Cancel': 'cancelled',
      'Postponed': 'postponed',
      'Suspended': 'suspended'
    };

    return (
      <>
        <span
          className={`badge-label ${classNames[MatchStatus]} `}
          style={{ display: 'none' }}>
          {MatchStatus}
        </span>
        {MatchStatus === 'Live' ? (
          <i className={classNames[MatchStatus]}></i>
        ) : (
          <>
            <i
              className={`icon-status icon-${classNames[MatchStatus]}`}
              id={`link-match-status-${rowIndex}`}></i>
            <UncontrolledTooltip
              placement='bottom'
              target={`link-match-status-${rowIndex}`}>
              {MatchStatus}
            </UncontrolledTooltip>
          </>
        )}
      </>
    );
  };

  return (
    <div className='no-cell'>
      {IsShowAbsenceIcon && <AbsencePersonIcon />}
      {MatchNo && <span>{MatchNo}</span>}
      {renderIcon()}
    </div>
  );
};

export default NoCell;
