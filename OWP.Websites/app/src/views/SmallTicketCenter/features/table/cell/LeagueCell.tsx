import LeagueContent from '../../../../Shared/common/CustomTable/commonCell/commonContentNew/LeagueContent';

const LeagueCell = ({ owpLeagueName, isRead }:{owpLeagueName: string, isRead: boolean}) => {
  return (
    <span
      className={`league-info-cell conversation-cell ${
        !isRead ? 'has-unread' : 'cell'
      }`}>
      <LeagueContent leagueName={owpLeagueName} />
    </span>
  );
};


export default LeagueCell;
