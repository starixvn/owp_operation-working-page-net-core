import PropTypes from 'prop-types';
import { Dropdown, Menu } from 'antd';
import DropdownIcon from '../../assets/svg-element/DropdownIcon';
import { DataSource } from '../TaskList/type/TTaskListTable';
import React, { useEffect, useState } from 'react';
import { get } from './Service/Service';

type TTradingMenuPopup = {
  firstMatchCheckedIsSaba?: boolean;
  matchesSelected: Readonly<Array<DataSource>>;
};

type ActionKey = 'Saba' | 'RT3';

interface ActionConfig {
  url: string;
  paramName: string;
  label: string;
  isDisabled: boolean;
}

const propTypes = {
  firstMatchCheckedIsSaba: PropTypes.bool,
  matchesSelected: PropTypes.array
};

const defaultPort = 80;

const BASE_URLS = {
  Saba: '/SabaTrader/SabaMultipleControl.aspx',
  RT3: '/RobotTrader3/Robot3MulControl.aspx'
} as const;

const useActionConfigs = (
  tradingPageUrl: string,
  firstMatchCheckedIsSaba?: boolean
) => {
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

const createLabelClickHandler =
  (href: string, disabled: boolean) => (e: React.MouseEvent) => {
    e.stopPropagation();

    if (!disabled) {
      window.open(href, '_blank', 'noopener,noreferrer');
    }
  };

const createMenuTitle = (label: string, href: string, disabled: boolean) => (
  <span
    onClick={createLabelClickHandler(href, disabled)}
    style={{
      cursor: disabled ? 'not-allowed' : 'pointer',
      color: disabled ? 'rgba(0, 0, 0, 0.25)' : undefined
    }}>
    {label}
  </span>
);

const buildUrl = (config: ActionConfig, matchIds: string) => {
  const url = new URL(config.url);
  url.searchParams.append(config.paramName, matchIds);
  return url.toString();
};

const createMenu = (
  actionConfigs: Record<string, ActionConfig>,
  getMenuLink: (key: ActionKey) => string
) => (
  <Menu mode='vertical' triggerSubMenuAction='click'>
    {Object.entries(actionConfigs).map(([key, config], index) => (
      <React.Fragment key={key}>
        <Menu.Item key={key} disabled={config.isDisabled}>
          {createMenuTitle(
            config.label,
            getMenuLink(key as ActionKey),
            config.isDisabled
          )}
        </Menu.Item>
        {index < Object.keys(actionConfigs).length - 1 && <Menu.Divider />}
      </React.Fragment>
    ))}
  </Menu>
);

const TradingMenuPopup = (props: TTradingMenuPopup) => {
  const { firstMatchCheckedIsSaba, matchesSelected } = props;
  const [tradingUrl, setTradingUrl] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(true);

  const actionConfigs = useActionConfigs(tradingUrl, firstMatchCheckedIsSaba);
  const getMenuLink = (key: ActionKey) => {
    const matchIds = matchesSelected.map(match => match.AdminSiteId).join(',');
    const config = actionConfigs[key];
    return buildUrl(config, matchIds);
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const url = `/api/Configuration/soccer/get`;
        const response = await get({ url });
        const { tradingPage, servers } = response.data;

        //get api return  serverSettingId
        const serverSettingId = 'Server 203';
        const serverSetting = servers.find(
          (server: any) => server.name === serverSettingId
        );

        setTradingUrl(
          serverSetting === undefined
            ? tradingPage
            : serverSetting.port !== defaultPort
            ? `${serverSetting.url}:${serverSetting.tradingPort}`
            : serverSetting.url
        );
      } catch (error) {
        console.error('Error fetching trading URL:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, []);

  if (loading || !tradingUrl) {
    return null;
  }

  const menu = createMenu(actionConfigs, getMenuLink);

  return (
    <Dropdown dropdownRender={() => menu} trigger={['click']}>
      <a onClick={e => e.preventDefault()}>
        <DropdownIcon colorDropdown='black' handleOnClick={() => {}} />
      </a>
    </Dropdown>
  );
};

TradingMenuPopup.propTypes = propTypes;

export default TradingMenuPopup;
