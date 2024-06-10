import Spinner from '../../components/Spinner';
import store from '../app/store';
import { TDataSource } from '../types/noOfferMatchType';
import updateConfirmedThunk from '../table/updateConfirmedThunkNative';
import ToastrWrapper from '../../components/ToastrWrapper';
import { MESSAGE } from '../const/const.json';
import { ChangeEvent } from 'react';

const updateConfirmAllEvent = async (event: ChangeEvent<HTMLInputElement>) => {
  const { checked } = event.target;
  const { entities } = store.getState().eventPickingTable;
  if (checked && entities.length) {
    const confirmResult = await ToastrWrapper.confirm(MESSAGE.CONFIRM_ALL);
    if (confirmResult.value) {
      Spinner.show();
      const infoSiteMatchIds = (entities as TDataSource[]).map(
        element => element.infoSiteMatchId
      );
      await store.dispatch(
        updateConfirmedThunk({
          infoSiteMatchIds,
          confirmed: checked
        })
      );
      Spinner.hide();
    }
  }
};

export default updateConfirmAllEvent;
