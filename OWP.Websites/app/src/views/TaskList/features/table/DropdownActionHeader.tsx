import ToastrWrapper from '../../../components/ToastrWrapper';
import { useAppSelector } from '../../app/hooks';
import { RootState } from '../../app/storeNative';

type TDropdownActionHeader = {
  refValue: React.RefObject<HTMLDivElement>;
  closePopup: () => void;
};

type ActionKey = 'Saba' | 'RT3';

interface ActionConfig {
  url: string;
  paramName: string;
  label: string;
}

const ACTION_CONFIGS: Record<ActionKey, ActionConfig> = {
  Saba: {
    url: 'http://live.oneops.net/SabaTrader/SabaMultipleControl.aspx',
    paramName: 'MatchIds',
    label: 'Saba Batch'
  },
  RT3: {
    url: 'http://live.oneops.net/RobotTrader3/Robot3MulControl.aspx',
    paramName: 'matchID',
    label: 'RT3 Batch'
  }
};

const DropdownActionHeader = ({
  refValue,
  closePopup
}: TDropdownActionHeader) => {
  const matchesSelected = useAppSelector(
    (state: RootState) => state.taskList.matchesSelected
  );

  const buildUrl = (config: ActionConfig, matchIds: string): string => {
    const url = new URL(config.url);
    url.searchParams.append(config.paramName, matchIds);
    return url.toString();
  };

  const handleOpenLink = (key: ActionKey) => {
    closePopup();
    if (matchesSelected.length === 0) {
      ToastrWrapper.showInfo(<span>No match has been selected!</span>);
      return;
    }

    const matchIds = matchesSelected.map(match => match.AdminSiteId).join(',');
    const config = ACTION_CONFIGS[key];
    const url = buildUrl(config, matchIds);
    window.open(url, '_blank');
  };

  return (
    <div className='dropdown-action is-show' ref={refValue}>
      {Object.entries(ACTION_CONFIGS).map(([key, config]) => (
        <li key={key} onClick={() => handleOpenLink(key as ActionKey)}>
          {config.label}
        </li>
      ))}
    </div>
  );
};

export default DropdownActionHeader;
