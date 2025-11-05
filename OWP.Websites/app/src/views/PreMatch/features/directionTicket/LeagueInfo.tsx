//import { useAppDispatch } from '../../app/hooks';
//import { updateObject } from '../slice/editSettingSlice';

const LeagueInfo = (props: {
  league: string;
  event: string;
  matchId: Required<number>;
  leagueGroup: string;
}) => {
  //const dispatch = useAppDispatch();
  const { league, event, matchId, leagueGroup } = props;

  const renderLeagueName = () => (
    <div className='content'>
      <div className='left'>
        League
      </div>
      <label style={{ marginTop: '7px' }}>{league}</label>
    </div>
  );

  const renderEvent = () => (
    <div className='content'>
      <div className='left'>
        Event
      </div>
      <label style={{ marginTop: '7px' }}>{event}</label>
    </div>
  );

  const renderAdminSiteMatchId = () => (
    <div className='content'>
      <div className='left'>
        Match ID
      </div>
      <label style={{ marginTop: '7px' }}>{matchId}</label>
    </div>
  );

  const renderLeagueGroup = () => (
    <div className='content'>
      <div className='left'>
        League group
      </div>
      <label style={{ marginTop: '7px' }}>{leagueGroup}</label>
    </div>
  );

  const renderTrader = () => (
    <div className='content'>
      <div className='left'>Include Trader</div>
      <div className='league--type__content'>
        <input
          id='chb-include-trader'
          type='checkbox'
          name='include-trader'
          className='form-check-input'
          disabled
        />
      </div>
    </div>
  );

  return (
    <div className='list'>
      {renderLeagueName()}
      {renderEvent()}
      {renderAdminSiteMatchId()}
      {renderLeagueGroup()}
      {renderTrader()}
    </div>
  );
};
export default LeagueInfo;
