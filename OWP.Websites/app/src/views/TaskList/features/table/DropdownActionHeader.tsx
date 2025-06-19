import { useEffect, useState } from 'react';
import { useAppSelector } from '../../app/hooks';
import { RootState } from '../../app/storeNative';
import { get } from '../../../Shared/Service/Service';

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

const ActionItem = ({
  config,
  onClick
}: {
  config: ActionConfig;
  onClick: () => void;
}) => (
  <li
    onClick={onClick}
    className={`${config.isDisabled && 'disable-menu-option'}`}>
    {config.label}
  </li>
);

const DropdownActionHeader = ({
  refValue,
  closePopup,
  firstMatchCheckedIsSaba
}: TDropdownActionHeader) => {
  const matchesSelected = useAppSelector(
    (state: RootState) => state.taskList.matchesSelected
  );

  const [tradingUrl, setTradingUrl] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(true);
  const actionConfigs = useActionConfigs(tradingUrl, firstMatchCheckedIsSaba);
  const buildUrl = useUrlBuilder();

  const handleOpenLink = (key: ActionKey) => {
    const matchIds = matchesSelected.map(match => match.AdminSiteId).join(',');
    const config = actionConfigs[key];
    const url = buildUrl(config, matchIds);
    window.open(url, '_blank');

    closePopup();
  };

  useEffect(() => {
    (async () => {
      const url = `/api/Configuration/soccer/get`;
      const response = await get({ url });
      const { tradingPage } = response.data;
      setTradingUrl(tradingPage);
      setLoading(false);
    })();
  }, []);

  if (loading) {
    return null;
  }
  return (
    <div className='dropdown-action is-show' ref={refValue}>
      {Object.entries(actionConfigs).map(([key, config]) => (
        <ActionItem
          key={key}
          config={config}
          onClick={() => !config.isDisabled && handleOpenLink(key as ActionKey)}
        />
      ))}
    </div>
  );
};

export default DropdownActionHeader;
