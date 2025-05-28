import Spinner from '../../components/Spinner';
import store from '../app/store';
import { TDataSource } from '../types/noOfferMatchType';
import updateConfirmedThunk from '../table/updateConfirmedThunkNative';
import ToastrWrapper from '../../components/ToastrWrapper';
import { MESSAGE } from '../const/const.json';
import { ChangeEvent } from 'react';

export const convertToGMT8String = (date: Date | null | undefined): string => {
  if (!date) return '';

  return date
    .toLocaleString('en-US', {
      timeZone: 'Asia/Taipei',
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: false
    })
    .replace(
      /(\d+)\/(\d+)\/(\d+),\s(\d+):(\d+):(\d+)/,
      '$3-$1-$2T$4:$5:$6+08:00'
    );
};

const updateConfirmAllEvent = async (event: ChangeEvent<HTMLInputElement>) => {
  const { checked } = event.target;
  const { entities } = store.getState().eventPickingTable;
  const { lastSearchTime } = store.getState().filter;
  const gmt8TimeStr = convertToGMT8String(lastSearchTime);
  if (checked && entities.length) {
    const confirmResult = await ToastrWrapper.confirm(MESSAGE.CONFIRM_ALL);
    if (confirmResult.value) {
      Spinner.show();
      const infoSiteMatchIds = (entities as TDataSource[])
        .filter(match => !match.isMovedEventDate)
        .map(element => element.infoSiteMatchId);
      await store.dispatch(
        updateConfirmedThunk({
          infoSiteMatchIds,
          confirmed: checked,
          confirmedAt: gmt8TimeStr || ''
        })
      );
      Spinner.hide();
    }
  }
};

export default updateConfirmAllEvent;
