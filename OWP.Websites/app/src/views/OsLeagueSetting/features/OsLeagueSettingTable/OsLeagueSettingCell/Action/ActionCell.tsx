import { useState } from 'react';
import { DataSource } from '../../../../typing/tableType';
import { UncontrolledTooltip } from 'reactstrap';
import LogHistory from './ActionPopup/LogHistory';
import { actionIcon } from '../../TableSchema.json';
import useStore from '../../../../app/store';
type Props = {
  dataSource: Required<Array<DataSource>>;
  rowIndex: Required<number>;
};

const ActionCell = (props: Props): JSX.Element => {
  const { getLogSettingQuery } = useStore(state => state);
  const [isOpenStatusHistory, setIsOpenStatusHistory] = useState(false);

  const handleCloseStatusHistory = () => setIsOpenStatusHistory(false);
  const { dataSource, rowIndex } = props;
  const { leagueSettingId, osLeagueName } = dataSource[rowIndex];
  const handleOpenStatusHistory = async () => {
    setIsOpenStatusHistory(true);
    getLogSettingQuery(leagueSettingId);
  };
  return (
    <div
      className='action-cell'
      id={`action-${leagueSettingId}`}
      onClick={handleOpenStatusHistory}>
      <svg
        width='20'
        height='20'
        viewBox='0 0 20 20'
        fill='none'
        xmlns='http://www.w3.org/2000/svg'>
        <path d={actionIcon} fill='#444444' />
      </svg>
      <UncontrolledTooltip
        placement='right'
        target={`action-${leagueSettingId}`}>
        Log History
      </UncontrolledTooltip>
      {isOpenStatusHistory && (
        <LogHistory
          leagueName={osLeagueName}
          handleClosePopup={handleCloseStatusHistory}
        />
      )}
    </div>
  );
};

export default ActionCell;
