import { useState } from 'react';
import PropTypes from 'prop-types';
import LogHistoryIcon from '../../../../assets/svg-element/LogHistoryIcon';
import HistoryModal from '../../modal/HistoryModal.native';
import { getLocaleDateTime } from '../../../Shared/DateTime/dateTimeHelper';
const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const ActionCell = props => {
  const { rowIndex, dataSource } = props;
  const { infoSiteMatchId, leagueName, homeName, awayName, eventDateTime } =
    dataSource[rowIndex];
  const [openLog, setOpenLog] = useState(false);
  const header = (
    <>
      <strong>
        Log History - {leagueName} | {homeName} vs {awayName}
      </strong>{' '}
      <i>{getLocaleDateTime(eventDateTime, 'lll')}</i>
    </>
  );

  return (
    <>
      <div className='action-cell'>
        <span className='icon-history-event' onClick={() => setOpenLog(true)}>
          <LogHistoryIcon width={20} height={20} />
        </span>
      </div>
      {openLog && (
        <HistoryModal
          infoSiteMatchId={infoSiteMatchId}
          onToggle={() => setOpenLog(false)}
          header={header}
        />
      )}
    </>
  );
};

ActionCell.propTypes = propTypes;
export default ActionCell;
