import useStore from '../../../../app/store';
import { CellProps } from '../../../../typing/tableType';
import StatusIcon from '../Status/StatusIcon';

const LeagueCell = (props: CellProps): JSX.Element => {
  const { setPopupType, queryLeagueDetail } = useStore(state => state);

  const { dataSource, rowIndex } = props;
  const { leagueSettingId, osLeagueName, isActive } = dataSource[rowIndex];

  const handleOpenDetailPopup = () => {
    queryLeagueDetail(leagueSettingId);
    setPopupType('edit');
  };
  return (
    <div className='league-cell' onClick={handleOpenDetailPopup}>
      <span className='maxbet-status'>
        <StatusIcon isActive={isActive} />
      </span>
      <span>{osLeagueName}</span>
    </div>
  );
};

export default LeagueCell;
