import PropTypes from 'prop-types';
import { Button } from 'reactstrap';
import { TooltipText } from '../../../../../Shared/common/Tooltip';
import Utils from '../../../../../Shared/Utils';
import { formatTime } from '../../../../../Shared/DateTime/dateTimeHelper';
import {
  getLeagueIdFromAdminSite,
  getTradingLinkInfo
} from '../../../../services/Service';
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
      `/bmaio?match_id=${adminsiteMatchId}`,
    'Baseball': () => `/baseball-aio?match_id=${adminsiteMatchId}`,
    'Basketball': () =>
      `/Market/LinkInfoSite.aspx?type=BasketballAIO&page=Control&marketType=Live&range=L&matchid=${adminsiteMatchId}&bettype=20`,
    'Cricket': () => `/AllInOneView/Trader?matchid=${adminsiteMatchId}&range=L`,
    'Darts': () => `/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Football': () => `/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Field Hockey': () => `/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Handball': () => `/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Ice Hockey': () => `/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Netball': () => `/Os/Index?matchid=${adminsiteMatchId}&range=L `,
    'Snooker': () => `/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Pool': () => `/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Table Tennis': () => `/Os/Index?matchid=${adminsiteMatchId}&range=L`,
    'Tennis': leagueId =>
      `/Live/ThirdPartyAllInOne_TennisDetail.aspx?matchid=${adminsiteMatchId}&leagueID=${leagueId}&range=L`,
    'Volleyball': () =>
      `/VolleyballAIO/Index?matchid=${adminsiteMatchId}&range=L`
  };

  const buildAdminSiteUrl = async (sport, adminSiteLeagueId) => {
    const adminSiteUrlInfo = await getTradingLinkInfo();

    const url = tradingLinkItem[sport](adminSiteLeagueId);
    let domain = adminSiteUrlInfo.general;
    switch (sport.toLowerCase()) {
      case 'badminton':
        domain = adminSiteUrlInfo.badminton;
        break;
      case 'cricket':
        domain = adminSiteUrlInfo.cricket;
        break;
      case 'baseball':
        domain = adminSiteUrlInfo.baseball;
        break;
      case 'basketball':
        domain = adminSiteUrlInfo.basketball;
        break;
      case 'tennis':
        domain = adminSiteUrlInfo.tennis;
        break;
    }

    return `${domain}${url}`;
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

    const url = await buildAdminSiteUrl(sport, adminSiteLeagueId);
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
