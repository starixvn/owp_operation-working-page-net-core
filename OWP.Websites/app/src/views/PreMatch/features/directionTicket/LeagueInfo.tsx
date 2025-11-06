import { useDispatch } from "react-redux";
import { updateIncludeTrader } from "./directionTicketSlice";

const LeagueInfo = (props: {
  league: string;
  event: string;
  matchId: Required<number>;
  leagueGroup: string;
  isIncludeTrader: boolean;
}) => {
  const { league, event, matchId, leagueGroup, isIncludeTrader } = props;
  const dispatch = useDispatch();

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
          defaultChecked={isIncludeTrader}
          onChange={event => dispatch(updateIncludeTrader(event.currentTarget.checked))}
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
