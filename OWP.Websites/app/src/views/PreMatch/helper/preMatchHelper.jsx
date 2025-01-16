import { isEqual } from 'lodash';
import {
    TAKE_OVER_API_KEY,
    TradingPageUrl,
    simpleApiQueryUrl,
    localStoreKeyTime
} from '../const/const';
import { formatTime } from '../../Shared/DateTime/dateTimeHelper';
import dayjs from 'dayjs';
const filterPrematchUpdated = (allPrematch, prematchIdUpdated) =>
    allPrematch.filter(
        p =>
            p.MatchCount &&
            prematchIdUpdated.some(id => isEqual(p.PreMatchLeagueId, id))
    );

const buildClassNameAssignmentCell = (isHighlight, isCopying) => {
    let className = '';
    if (isHighlight) {
        className = 'highlight';
        if (isCopying) {
            className = `${className} copied`;
        }
    }
    return className;
};

const buildLAST_UPDATEDFormatter = props => {
    const { updateTime, updateBy } = props;
    const LAST_UPDATEDFormatter = updateTime
        ? formatTime(updateTime, 'minute', 'LLL')
        : undefined;
    return {
        LAST_UPDATEDFormatter,
        updateBy
    };
};

const buildLastUpdate = props => {
    const { LAST_UPDATEDFormatter, updateBy } = props;
    const lastUpdate = LAST_UPDATEDFormatter
        ? ` (Last Updated at ${LAST_UPDATEDFormatter} GMT+8 by ${updateBy})`
        : null;
    return lastUpdate;
};

const pipe = (...fns) => x => fns.reduce((y, func) => func(y), x);

const buildHeaderTitle = (updateTime, updateBy) => {
    const builderPipe = pipe(buildLAST_UPDATEDFormatter, buildLastUpdate);
    return builderPipe({ updateTime, updateBy });
};

const buildStartTimeQueryModel = evenDate => {
    return {
        url: simpleApiQueryUrl,
        apiKey: TAKE_OVER_API_KEY,
        EventDate: evenDate
    };
};

const buildGoToTradingPageLink = (betType, assignedLeagueIds) =>
    `${TradingPageUrl}?bt=${betType}&rng=T&c=${assignedLeagueIds}&lge=&lgt=${assignedLeagueIds}&lgl=&lgc=&fcSite=0&fcSource=4`;

const getLocalKey = isNightShift =>
    isNightShift ? localStoreKeyTime.night : localStoreKeyTime.day;

const getDefaultTimeCondition = isNightShift => {
    const format = 'h:mm A';
    const localKey = getLocalKey(isNightShift);
    const prevTime = localStorage.getItem(localKey);
    const defaultTime = isNightShift ? '10:59 PM' : '05:31 PM';
    return prevTime ? dayjs(prevTime, format) : dayjs(defaultTime, format);
};

const saveTimeConditionToLocalStorage = (isNightShift, startTimeSelected) => {
    const localKey = getLocalKey(isNightShift);
    localStorage.setItem(localKey, startTimeSelected.format('h:mm A'));
};

export {
    filterPrematchUpdated,
    buildClassNameAssignmentCell,
    buildHeaderTitle,
    buildStartTimeQueryModel,
    buildLAST_UPDATEDFormatter,
    buildLastUpdate,
    buildGoToTradingPageLink,
    getDefaultTimeCondition,
    saveTimeConditionToLocalStorage
};
