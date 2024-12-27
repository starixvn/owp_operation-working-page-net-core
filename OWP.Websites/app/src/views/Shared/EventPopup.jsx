import PropTypes from 'prop-types';
import { UncontrolledPopover, PopoverBody } from 'reactstrap';
import { BettypeId } from './Constants';

const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  adminSiteId: PropTypes.number.isRequired,
  isFantasy: PropTypes.bool.isRequired,
  className: PropTypes.string,
  minorBettypeId: PropTypes.number,
  league: PropTypes.string
};

const EventPopup = props => {
  const {
    rowIndex,
    adminSiteId,
    isFantasy,
    className,
    minorBettypeId,
    league
  } = props;

  const leaguesUseSabaLink = [
    'SABA ELITE FRIENDLY Virtual PES 23 - PENALTY SHOOTOUTS',
    'SABA ELITE FRIENDLY Virtual PES 21 - 5 Mins Play'
  ];

  const renderItem = (href, value) => (
    <li>
      <a href={href} rel='noopener noreferrer' target='_blank'>
        {value}
      </a>
    </li>
  );

  const getLaControllerHref = (league, adminSiteId) => {
    const isUseSabaLink = leaguesUseSabaLink.includes(league);

    const baseUrl = isUseSabaLink
      ? 'http://live.oneops.net:805/Live/OddsControl_Saba.aspx'
      : 'http://live.oneops.net:805/Live/OddsControl.aspx';

    return `${baseUrl}?matchid=${adminSiteId}&type=s`;
  };

  const renderLaControllerItem = () => {
    const href = getLaControllerHref(league, adminSiteId);
    return renderItem(href, 'LA Controller');
  };

  const renderFantasySubItem = () => {
    const tradingPageUrl = `http://live.oneops.net/FantasyRobot/FantasyRobotControl.aspx?MatchId=${adminSiteId}`;
    return (
      <li>
        <ul className='fantasy-item'>
          {renderItem(tradingPageUrl, 'Fantasy')}
        </ul>
      </li>
    );
  };

  const renderTradingItem = () => (
    <li>
      <span>Live Trading</span>
    </li>
  );

  const isCornersMatch =
    minorBettypeId === BettypeId.nextCorners ||
    minorBettypeId === BettypeId.noOfCorners;

  const renderOtherMatchSubItem = () => {
    const tradingPageMlUrl = `http://live.oneops.net/TradingAutomation/MLRobotControl.aspx?MatchId=${adminSiteId}`;
    const tradingPageRt2Url = `http://live.oneops.net/RobotTrader2/RobotTraderController.aspx?MatchId=${adminSiteId}`;
    const tradingPageCornersRt3Url = `http://live.oneops.net/RobotTrader3/Robot3ChildMatchControl.aspx?MatchId=${adminSiteId}&rbtVer=7`;
    const tradingPageRt3Url = `http://live.oneops.net/RobotTrader3/RobotTrader3Control.aspx?MatchId=${adminSiteId}`;
    const tradingPageStmUrl = `http://live.oneops.net/FutsalRobot/FutsalRobotControl.aspx?MatchId=${adminSiteId}`;
    const tradingPageLsport = `http://live.oneops.net/TradingAutomation/MLRobotLSportControl.aspx?MatchId=${adminSiteId}`;
    const rt3Url =
      minorBettypeId === BettypeId.noOfCorners
        ? tradingPageCornersRt3Url
        : tradingPageRt3Url;

    return (
      <li>
        <ul className='fantasy-item'>
          {!isCornersMatch && renderItem(tradingPageMlUrl, 'ML')}
          {renderItem(tradingPageRt2Url, 'RT2')}
          {renderItem(rt3Url, 'RT3')}
          {!isCornersMatch && renderItem(tradingPageStmUrl, 'STM')}
          {renderItem(tradingPageLsport, 'Lsport')}
        </ul>
      </li>
    );
  };

  return (
    <UncontrolledPopover
      className={`popup-trader ${className}`}
      trigger='legacy'
      placement='right'
      target={`Event${rowIndex}`}>
      <PopoverBody>
        <ul className='menu'>
          {renderLaControllerItem()}
          <div className='vline'></div>
          {renderTradingItem()}
          {isFantasy ? renderFantasySubItem() : renderOtherMatchSubItem()}
        </ul>
      </PopoverBody>
    </UncontrolledPopover>
  );
};

EventPopup.propTypes = propTypes;

export default EventPopup;
