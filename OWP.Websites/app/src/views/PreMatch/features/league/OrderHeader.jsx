import { useCallback, useState } from 'react';
import { isEqual } from 'lodash';
import { useDispatch, useSelector } from 'react-redux';
import { sortByColumnKey } from './shiftSlice/shiftSlice';
import { TableConst } from './constants/preMatchConst';
import { nanoid } from '@reduxjs/toolkit';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { get } from '../../ImportPrematchHelperNative';
import { URL_QUERY_ASSIGNED_LEAGUE_ID } from '../../const/const';
import { buildGoToTradingPageLink } from '../../helper/preMatchHelper';
import { resetCopyInfo } from '../copy/copySlice';

const OrderHeader = headerModel => {
  const {
    columnKey,
    headerName,
    sortState,
    isRedirect,
    bettypeId,
    isTakeOver,
    isNightShift
  } = headerModel;
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);

  const orderType = isEqual(columnKey, sortState.columnKey)
    ? sortState.orderType
    : '';

  const { permission } = useSelector(state => state.policy);

  const [linkHref, setLink] = useState();
  const handleOnClickOrder = () => {
    const orderSelection = isEqual(orderType, TableConst.ORDER_ASC)
      ? TableConst.ORDER_DESC
      : TableConst.ORDER_ASC;

    const postModel = {
      columnKey,
      orderType: orderSelection,
      tableKey: nanoid()
    };
    stableDispatch(resetCopyInfo());
    stableDispatch(sortByColumnKey(postModel));
  };

  const handleOpenNewTab = async event => {
    event.preventDefault();
    const resp = await get({
      url: URL_QUERY_ASSIGNED_LEAGUE_ID,
      bettypeId,
      isTakeOver,
      actorId: permission.ActorId,
      isDay: !isNightShift
    });
    const { LeagueIds, TodayLeagueCount, UnSupportedLeagueNames } = resp.data;

    const link = buildGoToTradingPageLink(bettypeId, LeagueIds);
    setLink(link);

    if (!TodayLeagueCount) {
      ToastrWrapper.showWarning(
        <p>You have no assignments for this kind of bet type at the moment</p>
      );
      return;
    }

    if (UnSupportedLeagueNames?.length) {
      const leagueView = UnSupportedLeagueNames.map(
        (l, i) => `<ol>${i + 1}. ${l}</ol>`
      ).join('');
      ToastrWrapper.showWarning(
        `<div class="league-noti">You are assigned to ${TodayLeagueCount} league(s) today, but there are ${UnSupportedLeagueNames.length} 
        league(s) not supported for opening Prematch Trading page:</div> 
        <ul class="league-list">${leagueView}</ul>`
      );
      return;
    }

    window.open(link, '_blank').focus();
  };

  return (
    <div>
      {isRedirect ? (
        <a className='link-prematch' href={linkHref} onClick={handleOpenNewTab}>
          {headerName}
        </a>
      ) : (
        <span className='header-name'>{headerName}</span>
      )}
      <span className={`sort-icon ${orderType}`} onClick={handleOnClickOrder} />
    </div>
  );
};

export default OrderHeader;
