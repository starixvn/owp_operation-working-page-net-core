import PropTypes from 'prop-types';
import { UncontrolledPopover, PopoverBody, Button, Collapse } from 'reactstrap';
import { BettypeId } from './Constants';
import React, { useState } from 'react';
import { ChevronUpIcon, ChevronDownIcon } from 'mdi-react';

const defaultPort = 80;
const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  adminSiteId: PropTypes.number.isRequired,
  isFantasy: PropTypes.bool.isRequired,
  className: PropTypes.string,
  minorBettypeId: PropTypes.number,
  league: PropTypes.string,
  laTradingPage: PropTypes.string,
  tradingPage: PropTypes.string,
  servers: PropTypes.array
};

const EventPopup = props => {
  const {
    rowIndex,
    adminSiteId,
    isFantasy,
    className,
    minorBettypeId,
    league,
    laTradingPage,
    tradingPage,
    servers = []
  } = props;

  const leaguesUseSabaLink = [
    'SABA ELITE FRIENDLY Virtual PES 23 - PENALTY SHOOTOUTS',
    'SABA ELITE FRIENDLY Virtual PES 21 - 5 Mins Play'
  ];

  const [laCollapseOpen, setLaCollapseOpen] = useState(false);
  const toggleLaCollapse = () => setLaCollapseOpen(!laCollapseOpen);

  const [openType, setOpenType] = useState(null);
  const handleToggle = type => {
    setOpenType(openType === type ? null : type);
  };

  const getLaControllerHref = (league, adminSiteId, url) => {
    const isUseSabaLink = leaguesUseSabaLink.includes(league);

    const baseUrl = isUseSabaLink
      ? `${url}/Live/OddsControl_Saba.aspx`
      : `${url}/Live/OddsControl.aspx`;
    return `${baseUrl}?matchid=${adminSiteId}&type=s`;
  };

  const getServiceWithPort = (url, port) => {
    if (port === defaultPort) {
      return url;
    }
    return `${url}:${port}`;
  };

  const renderLaControllerCollapse = () => {
    if (!servers || servers.length === 0) {
      return null;
    }
    return (
      <li>
        <a
          href={getLaControllerHref(league, adminSiteId, laTradingPage)}
          style={{ cursor: 'pointer', display: 'inline-flex' }}>
          LA Controller
        </a>
        <span
          style={{ marginLeft: 6, cursor: 'pointer' }}
          onClick={toggleLaCollapse}>
          {laCollapseOpen ? (
            <ChevronUpIcon size={16} />
          ) : (
            <ChevronDownIcon size={16} />
          )}
        </span>
        <Collapse isOpen={laCollapseOpen} className='la-collapse'>
          <ul>
            {servers.map((server, idx) => (
              <li key={server.name} style={{ listStyle: 'disc' }}>
                <a
                  href={getLaControllerHref(
                    league,
                    adminSiteId,
                    getServiceWithPort(server.url, server.laPort)
                  )}
                  rel='noopener noreferrer'
                  target='_blank'>
                  {server.name}
                </a>
              </li>
            ))}
          </ul>
        </Collapse>
      </li>
    );
  };

  const renderFantasySubItem = () => {
    const tradingPageUrl = `${tradingPage}/FantasyRobot/FantasyRobotControl.aspx?MatchId=${adminSiteId}`;
    return (
      <li>
        <ul className='fantasy-item'>
          <li>
            <a href={tradingPageUrl} rel='noopener noreferrer' target='_blank'>
              Fantasy
            </a>
          </li>
        </ul>
      </li>
    );
  };

  const getTradingPageUrl = (baseUrl, path, port) => {
    baseUrl = getServiceWithPort(baseUrl, port);
    const url = `${baseUrl}/${path}`;
    return url;
  };

  const isCornersMatch =
    minorBettypeId === BettypeId.nextCorners ||
    minorBettypeId === BettypeId.noOfCorners;

  const renderTradingTypeCollapse = (type, path) => (
    <li>
      <a
        href={getTradingPageUrl(tradingPage, path, defaultPort)}
        style={{
          cursor: 'pointer',
          display: 'inline-flex',
          alignItems: 'center'
        }}>
        {type}
      </a>
      <span style={{ marginLeft: 6 }} onClick={() => handleToggle(type)}>
        {openType === type ? (
          <ChevronUpIcon size={16} />
        ) : (
          <ChevronDownIcon size={16} />
        )}
      </span>
      <Collapse isOpen={openType === type}>
        <ul style={{ margin: 0, paddingLeft: 16 }}>
          {servers?.map((server, idx) => (
            <li key={server.name} style={{ listStyle: 'disc' }}>
              <a
                href={getTradingPageUrl(server.url, path, server.tradingPort)}
                rel='noopener noreferrer'
                target='_blank'>
                {server.name}
              </a>
            </li>
          ))}
        </ul>
      </Collapse>
    </li>
  );

  const renderOtherMatchSubItem = () => {
    const tradingPageMlPath = `TradingAutomation/MLRobotControl.aspx?MatchId=${adminSiteId}`;
    const tradingPageRt2Path = `RobotTrader2/RobotTraderController.aspx?MatchId=${adminSiteId}`;
    const tradingPageCornersRt3Path = `RobotTrader3/Robot3ChildMatchControl.aspx?MatchId=${adminSiteId}&rbtVer=7`;
    const tradingPageRt3Path = `RobotTrader3/RobotTrader3Control.aspx?MatchId=${adminSiteId}`;
    const tradingPageStmPath = `FutsalRobot/FutsalRobotControl.aspx?MatchId=${adminSiteId}`;
    const tradingPageLsportPath = `TradingAutomation/MLRobotLSportControl.aspx?MatchId=${adminSiteId}`;
    const rt3Path =
      minorBettypeId === BettypeId.noOfCorners
        ? tradingPageCornersRt3Path
        : tradingPageRt3Path;

    return (
      <li>
        <ul className='fantasy-item'>
          {!isCornersMatch &&
            renderTradingTypeCollapse('ML', tradingPageMlPath)}
          {renderTradingTypeCollapse('RT2', tradingPageRt2Path)}
          {renderTradingTypeCollapse('RT3', rt3Path)}
          {!isCornersMatch &&
            renderTradingTypeCollapse('STM', tradingPageStmPath)}
          {renderTradingTypeCollapse('Lsport', tradingPageLsportPath)}
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
          {renderLaControllerCollapse()}
          <div className='vline'></div>
          <li>
            <span>Live Trading</span>
          </li>
          {isFantasy ? renderFantasySubItem() : renderOtherMatchSubItem()}
        </ul>
      </PopoverBody>
    </UncontrolledPopover>
  );
};

EventPopup.propTypes = propTypes;

export default EventPopup;
