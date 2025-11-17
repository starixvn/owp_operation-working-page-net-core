const LeagueInfo = (props: {
  league: string;
  shortLeague: string;
  isBlackList: boolean;
}) => {
  const { league, shortLeague, isBlackList } = props;

  const renderLeagueName = () => (
    <div className='content'>
      <div className='left'>
        League Name
        <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>*</p>
      </div>
      <label style={{ marginTop: '7px' }}>{league}</label>
    </div>
  );

  const renderShortLeague = () => (
    <div className='content'>
      <div className='left'>Short League</div>
      <label style={{ marginTop: '7px' }}>{shortLeague}</label>
    </div>
  );

  const renderLeagueType = () => (
    <div className='content'>
      <div className='left'>League Type</div>
      <div className='league--type__content'>
        <div className='checkbox form-check-league-type'>
          <input
            checked={isBlackList}
            id='chb-blacklist-league'
            type='checkbox'
            name='oldLeague'
            className='form-check-input'
            disabled
          />
          <label className='form-check-label' htmlFor='chb-blacklist-league'>
            Blacklist
          </label>
        </div>
      </div>
    </div>
  );

  return (
    <div className='list'>
      <div className='title'>League Information</div>
      {renderLeagueName()}
      {renderShortLeague()}
      {renderLeagueType()}
    </div>
  );
};
export default LeagueInfo;
