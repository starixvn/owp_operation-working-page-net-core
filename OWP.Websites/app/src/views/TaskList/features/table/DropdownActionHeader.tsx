import { useAppSelector } from '../../app/hooks';
import { RootState } from '../../app/storeNative';

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
  Saba: 'http://live.oneops.net/SabaTrader/SabaMultipleControl.aspx',
  RT3: 'http://live.oneops.net/RobotTrader3/Robot3MulControl.aspx'
} as const;

const useActionConfigs = (firstMatchCheckedIsSaba?: boolean) => {
  return {
    Saba: {
      url: BASE_URLS.Saba,
      paramName: 'MatchIds',
      label: 'Saba Batch',
      isDisabled:
        firstMatchCheckedIsSaba === undefined ||
        firstMatchCheckedIsSaba === false
    },
    RT3: {
      url: BASE_URLS.RT3,
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
  const actionConfigs = useActionConfigs(firstMatchCheckedIsSaba);
  const buildUrl = useUrlBuilder();

  const handleOpenLink = (key: ActionKey) => {
    const matchIds = matchesSelected.map(match => match.AdminSiteId).join(',');
    const config = actionConfigs[key];
    const url = buildUrl(config, matchIds);
    window.open(url, '_blank');

    closePopup();
  };

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
