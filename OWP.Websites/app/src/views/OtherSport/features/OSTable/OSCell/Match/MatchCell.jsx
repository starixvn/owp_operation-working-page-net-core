import PropTypes from 'prop-types';
import { Button } from 'reactstrap';
import { TooltipText } from '../../../../../Shared/common/Tooltip';
import Utils from '../../../../../Shared/Utils';
import { formatTime } from '../../../../../Shared/DateTime/dateTimeHelper';
import { getLeagueIdFromAdminSite } from '../../../../services/Service';
const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

export const MatchCell = props => {
  const { dataSource, rowIndex } = props;

  const {
    away,
    homeName,
    matchOffer,
    autoLink,
    dateTime,
    league,
    adminsiteMatchId,
    iptv,
    wb,
    sport,
    provider
  } = dataSource[rowIndex];

  const tradingLinkItem = {
    'Badminton': () =>
      `http://live.oneops.net:9080/AllInOneView/Trader?matchid=${adminsiteMatchId}&range=L`,
    'Baseball': () =>
      `http://live.oneops.net:9030/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Basketball': () =>
      `http://live.oneops.net/Market/LinkInfoSite.aspx?type=BasketballAIO&page=Control&marketType=Live&range=L&matchid=${adminsiteMatchId}&bettype=20`,
    'Cricket': () =>
      `http://live.oneops.net:9050/AllInOneView/Trader?matchid=${adminsiteMatchId}&range=L`,
    'Darts': () =>
      `http://live.oneops.net:9030/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Football': () =>
      `http://live.oneops.net:9030/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Field Hockey': () =>
      `http://live.oneops.net:9030/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Handball': () =>
      `http://live.oneops.net:9030/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Ice Hockey': () =>
      `http://live.oneops.net:9030/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Netball': () =>
      `http://live.oneops.net:9030/Os/Index?matchid=${adminsiteMatchId}&range=L `,
    'Snooker': () =>
      `http://live.oneops.net:9030/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Pool': () =>
      `http://live.oneops.net:9030/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Table Tennis': () =>
      `http://live.oneops.net:9030/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Tennis': leagueId =>
      `http://live.oneops.net/Live/ThirdPartyAllInOne_TennisDetail.aspx?matchid=${adminsiteMatchId}&leagueID=${leagueId}&range=L`,
    'Volleyball': () =>
      `http://live.oneops.net:9030/VolleyballAIO/Index?matchid=${adminsiteMatchId}&range=L`
  };

  const dateTimeFormated = formatTime(dateTime, 'minute', 'MMM DD - hh:mm A');
  const handleCopy = () => {
    const text = `Sport: ${sport}\nK.O Time: ${dateTimeFormated}\nLeague: ${league}\nMatch: ${homeName} -vs- ${away}\nMatch ID: ${
      adminsiteMatchId ?? ''
    }\nIPTV/Streaming: ${getIpTvStreamingInfo()}\nProvider: ${provider}`;

    window.executeCopy(text);
  };

  const getIpTvStreamingInfo = () => [iptv, wb].filter(Boolean).join('/');

  const renderAutoLink = content => {
    const autoLinkContent =
      Utils.toLowerCaseForFilter(content) === 'yes' ? 'Auto Link' : content;
    return <span className='auto-link'>{autoLinkContent}</span>;
  };

  const handleOpenTraderPageOS = async () => {
    let adminSiteLeagueId = 0;
    if (sport === 'Tennis') {
      adminSiteLeagueId = await getLeagueIdFromAdminSite(adminsiteMatchId);
    }
    const url = tradingLinkItem[sport](adminSiteLeagueId);
    window.open(url, `${adminsiteMatchId}`);
  };

  return (
    <div className='match-cell'>
      <TooltipText
        text={
          <div className='match-content'>
            <div className='home'>{homeName}</div>
            <div className='away'>{away}</div>{' '}
          </div>
        }
        toolTipContent={`${homeName} -vs- ${away}`}
        innerClassName='home-away'
        rowIndex={rowIndex}
      />

      <div className='label'>
        {matchOffer && (
          <TooltipText
            text={<span className='match-offer'>{matchOffer}</span>}
            toolTipContent={matchOffer}
            rowIndex={rowIndex}
            innerClassName='match-offer'
          />
        )}

        {autoLink && (
          <TooltipText
            text={renderAutoLink(autoLink)}
            toolTipContent={renderAutoLink(autoLink)}
            rowIndex={rowIndex}
            innerClassName='auto-link'
          />
        )}
      </div>
      {adminsiteMatchId && tradingLinkItem[sport] && (
        <Button className='button-link-trader' onClick={handleOpenTraderPageOS}>
          <span className='icon-link'></span>
        </Button>
      )}
      <div className='copy-info'>
        <button onClick={handleCopy} className='copy-btn btn btn-primary'>
          <span className='icon-content_copy'></span>
        </button>
      </div>
    </div>
  );
};

MatchCell.propTypes = propTypes;
