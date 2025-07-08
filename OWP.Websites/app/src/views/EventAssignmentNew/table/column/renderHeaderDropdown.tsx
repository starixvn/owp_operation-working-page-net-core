import React, { useEffect, useState } from 'react';
import DropdownIcon from '../../../../assets/svg-element/DropdownIcon';
import useClickOutside from '../../../Shared/customHook/useClickOutside';
import FilterIcon from 'mdi-react/FilterIcon';
import CloseIcon from 'mdi-react/CloseIcon';
import { Button, Collapse } from 'reactstrap';
import { TDataTable } from '../../type/type';
import { get } from '../../../Shared/Service/Service';
import { ChevronUpIcon, ChevronDownIcon } from 'mdi-react';

const BASE_URLS = {
  Saba: '/SabaTrader/SabaMultipleControl.aspx',
  RT3: '/RobotTrader3/Robot3MulControl.aspx'
} as const;

type ActionKey = 'Saba' | 'RT3';

const useActionConfigs = (firstMatchCheckedIsSaba: boolean) => {
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
  } as const;
};

type RenderHeaderDropdownProps = {
  name: string;
  title: string;
  keyword: string;
  onClear: (name: string) => void;
  onSubmit: (name: string, keyword: string) => void;
  clearFilterTitle?: string;
  firstMatchCheckedIsSaba: boolean;
  matchesSelected: TDataTable;
};

const RenderHeaderDropdown = ({
  name,
  title,
  keyword,
  onClear,
  onSubmit,
  clearFilterTitle = 'Clear All Filters',
  firstMatchCheckedIsSaba,
  matchesSelected
}: RenderHeaderDropdownProps) => {
  const [isShowDropdownAction, setIsDropdownAction] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const [value, setValue] = useState(keyword || '');
  const [tradingUrl, setTradingUrl] = useState<string>('');
  const [servers, setServers] = useState<any[]>([]);
  const [openServerType, setOpenServerType] = useState<string | null>(null);
  const actionConfigs = useActionConfigs(firstMatchCheckedIsSaba);

  const ref = useClickOutside<HTMLDivElement>(() => setIsDropdownAction(false));

  useEffect(() => {
    (async () => {
      const url = `/api/Configuration/soccer/get`;
      const response = await get({ url });
      const { tradingPage, servers } = response.data;
      setTradingUrl(tradingPage);
      setServers(servers);
    })();
  }, []);

  const onHandleClickOpen = () => setIsOpen(!isOpen);
  const onHandleTextChange = (event: React.ChangeEvent<HTMLInputElement>) => setValue(event.target.value);
  const onHandleClearTextFilter = () => {
    setIsOpen(false);
    setValue('');
    onClear(name);
  };
  const onHandleClickSubmit = () => {
    setIsOpen(false);
    onSubmit(name, value);
  };
  const onKeyPress = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter') { onHandleClickSubmit(); }
  };

  const handleOpenLink = (baseUrl: string, key: ActionKey) => {
    const matchIds = matchesSelected.map(match => match.AdminSiteId).join(',');
    const config = actionConfigs[key];
    const url = `${baseUrl}${config.path}?${config.paramName}=${matchIds}`;
    window.open(url, '_blank');
    setIsDropdownAction(false);
  };

  const handleOpenDropdown = () => {
    setIsDropdownAction(true);
    setIsOpen(false);
  };

  const handleToggle = (type: string) => {
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
          <div className="dropdown-action is-show" ref={ref}>
            <ul style={{ margin: 0, padding: 0, listStyle: 'none' }}>
              {Object.entries(actionConfigs).map(([key, config]) => (
                <li
                  key={key}
                  className={`${config.isDisabled ? 'disable-menu-option' : ''}`}
                  onClick={() => !config.isDisabled && handleOpenLink(tradingUrl, key as ActionKey)}
                >
                  <div>
                    <span>{config.label}</span>
                    {!config.isDisabled && (
                      <span
                        onClick={e => {
                          e.stopPropagation();
                          handleToggle(config.label);
                        }}
                        style={{ cursor: 'pointer' }}
                      >
                        {openServerType === config.label ? <ChevronUpIcon size={16} /> : <ChevronDownIcon size={16} />}
                      </span>
                    )}
                  </div>
                  {!config.isDisabled && (
                    <Collapse isOpen={openServerType === config.label} className='server-list'>
                      <ul >
                        {servers.map((server, idx) => (
                          <li key={server.name || idx} >
                            <a
                              rel="noopener noreferrer"
                              target="_blank"
                              onClick={() => handleOpenLink(server.url, key as ActionKey)}
                            >
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
            <span
              className='clear-all'
              onClick={onHandleClearTextFilter}>
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

export default RenderHeaderDropdown; 