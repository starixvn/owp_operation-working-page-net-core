import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import DropdownIcon from '../../assets/svg-element/DropdownIcon';
import useClickOutside from './customHook/useClickOutside';
import FilterIcon from 'mdi-react/FilterIcon';
import CloseIcon from 'mdi-react/CloseIcon';
import { Button, Collapse } from 'reactstrap';
import { get } from './Service/Service';
import { ChevronUpIcon, ChevronDownIcon } from 'mdi-react';

const BASE_URLS = {
  Saba: '/SabaTrader/SabaMultipleControl.aspx',
  RT3: '/RobotTrader3/Robot3MulControl.aspx'
};

const defaultPort = 80;

const useActionConfigs = firstMatchCheckedIsSaba => {
  return {
    Saba: {
      path: BASE_URLS.Saba,
      paramName: 'MatchIds',
      label: 'Saba Batch',
      isDisabled:
        firstMatchCheckedIsSaba === undefined ||
        firstMatchCheckedIsSaba === false
    },
    RT3: {
      path: BASE_URLS.RT3,
      paramName: 'matchID',
      label: 'RT3 Batch',
      isDisabled:
        firstMatchCheckedIsSaba === undefined ||
        firstMatchCheckedIsSaba === true
    }
  };
};

const FilterTradingLink = ({
  name,
  title,
  keyword,
  onClear,
  onSubmit,
  clearFilterTitle = 'Clear All Filters',
  firstMatchCheckedIsSaba,
  matchesSelected
}) => {
  const [isShowDropdownAction, setIsDropdownAction] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const [value, setValue] = useState(keyword || '');
  const [tradingUrl, setTradingUrl] = useState('');
  const [servers, setServers] = useState([]);
  const [openServerType, setOpenServerType] = useState(null);
  const actionConfigs = useActionConfigs(firstMatchCheckedIsSaba);

  const ref = useClickOutside(() => setIsDropdownAction(false));
  useEffect(() => {
    const fetchData = async () => {
      const url = `/api/Configuration/soccer/get`;
      const response = await get({ url });
      const { tradingPage, servers } = response.data;
      setTradingUrl(tradingPage);
      setServers(servers);
    };
    fetchData();
  }, []);

  const onHandleClickOpen = () => setIsOpen(!isOpen);
  const onHandleTextChange = event => setValue(event.target.value);
  const onHandleClearTextFilter = () => {
    setIsOpen(false);
    setValue('');
    onClear(name);
  };
  const onHandleClickSubmit = () => {
    setIsOpen(false);
    onSubmit(name, value);
  };
  const onKeyPress = event => {
    if (event.key === 'Enter') {
      onHandleClickSubmit();
    }
  };

  const handleOpenLink = (baseUrl, port, key) => {
    const matchIds = matchesSelected.map(match => match.AdminSiteId).join(',');
    const config = actionConfigs[key];
    baseUrl = port !== defaultPort ? `${baseUrl}:${port}` : baseUrl;
    const url = `${baseUrl}${config.path}?${config.paramName}=${matchIds}`;
    window.open(url, '_blank');
    setIsDropdownAction(false);
  };

  const handleOpenDropdown = () => {
    setIsDropdownAction(true);
    setIsOpen(false);
  };

  const handleToggle = type => {
    setOpenServerType(openServerType === type ? null : type);
  };

  return (
    <div className='title-header' ref={ref}>
      <span>{title}</span>
      <div className='dropdown-wrap'>
        <DropdownIcon
          colorDropdown='black'
          handleOnClick={handleOpenDropdown}
        />
        {isShowDropdownAction && (
          <div className='dropdown-action is-show' ref={ref}>
            <ul style={{ margin: 0, padding: 0, listStyle: 'none' }}>
              {Object.entries(actionConfigs).map(([key, config]) => (
                <li
                  key={key}
                  className={`${config.isDisabled ? 'disable-menu-option' : ''
                    }`}
                  onClick={() =>
                    !config.isDisabled && handleOpenLink(tradingUrl, defaultPort, key)
                  }>
                  <div>
                    <span>{config.label}</span>
                    {!config.isDisabled && (
                      <span
                        onClick={e => {
                          e.stopPropagation();
                          handleToggle(config.label);
                        }}
                        style={{ cursor: 'pointer' }}>
                        {openServerType === config.label ? (
                          <ChevronUpIcon size={16} />
                        ) : (
                          <ChevronDownIcon size={16} />
                        )}
                      </span>
                    )}
                  </div>
                  {!config.isDisabled && (
                    <Collapse
                      isOpen={openServerType === config.label}
                      className='server-list'>
                      <ul>
                        {servers.map((server, idx) => (
                          <li key={server.name || idx}>
                            <a
                              rel='noopener noreferrer'
                              target='_blank'
                              onClick={e => {
                                e.stopPropagation();
                                handleOpenLink(server.url, server.tradingPort, key);
                              }}>
                              {server.name}
                            </a>
                          </li>
                        ))}
                      </ul>
                    </Collapse>
                  )}
                </li>
              ))}
            </ul>
          </div>
        )}
      </div>
      <div className='filter-table'>
        <span
          id={name}
          className={`icon-filter ${keyword ? 'is-filter' : ''}`}
          onClick={onHandleClickOpen}>
          <FilterIcon size={14} />
        </span>
        {isOpen && (
          <div className='filter-popup filter-popup is-show'>
            <span className='close-popup' onClick={onHandleClickOpen}>
              <CloseIcon size={14} />
            </span>
            <input
              className='input-field'
              placeholder='Search'
              value={value}
              autoFocus
              onChange={onHandleTextChange}
              onKeyPress={onKeyPress}
            />
            <span className='clear-all' onClick={onHandleClearTextFilter}>
              {clearFilterTitle}
            </span>
            <Button
              className='btn-search'
              disabled={!value}
              onClick={onHandleClickSubmit}>
              Search
            </Button>
          </div>
        )}
      </div>
    </div>
  );
};

FilterTradingLink.propTypes = {
  name: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired,
  keyword: PropTypes.string.isRequired,
  onClear: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired,
  clearFilterTitle: PropTypes.string,
  firstMatchCheckedIsSaba: PropTypes.bool.isRequired,
  matchesSelected: PropTypes.arrayOf(
    PropTypes.shape({
      AdminSiteId: PropTypes.oneOfType([PropTypes.string, PropTypes.number])
    })
  ).isRequired
};

export default FilterTradingLink;
