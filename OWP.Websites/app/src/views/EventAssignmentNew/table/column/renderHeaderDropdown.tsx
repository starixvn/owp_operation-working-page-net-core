import React, { useEffect, useState } from 'react';
import DropdownIcon from '../../../../assets/svg-element/DropdownIcon';
import useClickOutside from '../../../Shared/customHook/useClickOutside';
import FilterIcon from 'mdi-react/FilterIcon';
import CloseIcon from 'mdi-react/CloseIcon';
import { Button } from 'reactstrap';
import { TDataTable } from '../../type/type';
import { get } from '../../../Shared/Service/Service';

const BASE_URLS = {
    Saba: '/SabaTrader/SabaMultipleControl.aspx',
    RT3: '/RobotTrader3/Robot3MulControl.aspx'
} as const;

interface ActionConfig {
    url: string;
    paramName: string;
    label: string;
    isDisabled?: boolean;
}

type ActionKey = 'Saba' | 'RT3';


const useActionConfigs = (tradingPageUrl: string, firstMatchCheckedIsSaba: boolean) => {
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
    const actionConfigs = useActionConfigs(tradingUrl, firstMatchCheckedIsSaba);
    const buildUrl = useUrlBuilder();

    const ref = useClickOutside<HTMLDivElement>(() => setIsDropdownAction(false));

    useEffect(() => {
        (async () => {
            const url = `/api/Configuration/soccer/get`;
            const response = await get({ url });
            const { tradingPage } = response.data;
            setTradingUrl(tradingPage);
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

    const handleOpenLink = (key: ActionKey) => {
        const matchIds = matchesSelected.map(match => match.AdminSiteId).join(',');
        const config = actionConfigs[key];
        const url = buildUrl(config, matchIds);
        window.open(url, '_blank');
        setIsDropdownAction(false);
    };

    const handleOpenDropdown = () => {
        setIsDropdownAction(true);
        setIsOpen(false);
    };

    return (
        <div className='title-header' style={{ display: 'flex', alignItems: 'center' }} ref={ref}>
            <span>{title}</span>
            <div className='dropdown-wrap'>
                <DropdownIcon
                    colorDropdown='black'
                    handleOnClick={handleOpenDropdown}
                />
                {isShowDropdownAction && (
                    <div className="dropdown-action is-show" ref={ref}

                    >
                        <ul style={{ margin: 0, padding: 0, listStyle: 'none' }}>
                            {Object.entries(actionConfigs).map(([key, config]) => (
                                <ActionItem
                                    key={key}
                                    config={config}
                                    onClick={() => !config.isDisabled && handleOpenLink(key as ActionKey)}
                                />
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