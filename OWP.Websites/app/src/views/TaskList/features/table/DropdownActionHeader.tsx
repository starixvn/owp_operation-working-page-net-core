import { useEffect, useState } from 'react';
import { useAppSelector } from '../../app/hooks';
import { RootState } from '../../app/storeNative';
import { get } from '../../../Shared/Service/Service';
import { Collapse } from 'reactstrap';
import { ChevronUpIcon, ChevronDownIcon } from 'mdi-react';

type TDropdownActionHeader = {
  refValue: React.RefObject<HTMLDivElement>;
  closePopup: () => void;
  firstMatchCheckedIsSaba?: boolean;
};

type ActionKey = 'Saba' | 'RT3';

interface ActionConfig {
  url: string;
  paramName: string;
  label: string;
  isDisabled?: boolean;
}

const BASE_URLS = {
  Saba: '/SabaTrader/SabaMultipleControl.aspx',
  RT3: '/RobotTrader3/Robot3MulControl.aspx'
} as const;

const useActionConfigs = (tradingPageUrl: string, firstMatchCheckedIsSaba?: boolean) => {
  return {
    Saba: {
      url: `${tradingPageUrl}${BASE_URLS.Saba}`,
      paramName: 'MatchIds',
      label: 'Saba Batch',
      isDisabled:
        firstMatchCheckedIsSaba === undefined ||
        firstMatchCheckedIsSaba === false
    },
    RT3: {
      url: `${tradingPageUrl}${BASE_URLS.RT3}`,
      paramName: 'matchID',
      label: 'RT3 Batch',
      isDisabled:
        firstMatchCheckedIsSaba === undefined ||
        firstMatchCheckedIsSaba === true
    }
  } as const;
};

const useUrlBuilder = () => {
  return (config: ActionConfig, matchIds: string): string => {
    const url = new URL(config.url);
    url.searchParams.append(config.paramName, matchIds);
    return url.toString();
  };
};

const DropdownActionHeader = ({
  refValue,
  closePopup,
  firstMatchCheckedIsSaba
}: TDropdownActionHeader) => {
  const matchesSelected = useAppSelector(
    (state: RootState) => state.taskList.matchesSelected
  );

  const [tradingUrl, setTradingUrl] = useState<string>('');
  const [servers, setServers] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const actionConfigs = useActionConfigs(tradingUrl, firstMatchCheckedIsSaba);
  const buildUrl = useUrlBuilder();
  const [openServerType, setOpenServerType] = useState<ActionKey | null>(null);

  const handleOpenLink = (key: ActionKey) => {
    const matchIds = matchesSelected.map(match => match.AdminSiteId).join(',');
    const config = actionConfigs[key];
    const url = buildUrl(config, matchIds);
    window.open(url, '_blank');
    closePopup();
  };

  const handleServerLink = (serverUrl: string, key: ActionKey) => {
    const matchIds = matchesSelected.map(match => match.AdminSiteId).join(',');
    const config = actionConfigs[key];
    const url = `${serverUrl}${config.url.split(tradingUrl)[1]}?${config.paramName}=${matchIds}`;
    window.open(url, '_blank');
    closePopup();
  };

  useEffect(() => {
    const fetchData = async () => {
      const url = `/api/Configuration/soccer/get`;
      const response = await get({ url });
      const { tradingPage, servers } = response.data;
      setTradingUrl(tradingPage);
      setServers(servers);
      setLoading(false);
    };
    fetchData();
  }, []);

  const handleToggle = (type: ActionKey) => {
    setOpenServerType(openServerType === type ? null : type);
  };

  if (loading) {
    return null;
  }

  return (
    <div className='dropdown-action is-show' ref={refValue}>
      <ul style={{ margin: 0, padding: 0, listStyle: 'none' }}>
        {Object.entries(actionConfigs).map(([key, config]) => (
          <li
            key={key}
            className={`${config.isDisabled ? 'disable-menu-option' : ''}`}
            onClick={() => !config.isDisabled && handleOpenLink(key as ActionKey)}
          >
            <div>
              <span>{config.label}</span>
              {!config.isDisabled && (
                <span
                  onClick={e => {
                    e.stopPropagation();
                    handleToggle(key as ActionKey);
                  }}
                  style={{ cursor: 'pointer' }}
                >
                  {openServerType === key ? <ChevronUpIcon size={16} /> : <ChevronDownIcon size={16} />}
                </span>
              )}
            </div>
            {!config.isDisabled && (
              <Collapse isOpen={openServerType === key} className='server-list'>
                <ul>
                  {servers.map((server, idx) => (
                    <li key={server.name || idx}>
                      <a
                        rel="noopener noreferrer"
                        target="_blank"
                        onClick={() => handleServerLink(server.url, key as ActionKey)}
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
  );
};

export default DropdownActionHeader;
