import Spinner from '../../components/Spinner';
import ToastrWrapper from '../../components/ToastrWrapper';
import store from '../app/store';
import updateNoOfferMatchThunk from '../table/updateNoOfferMatchThunkNative';
import { MESSAGE } from '../const/const.json';
import { TDataSource, TNoOfferMatch } from '../types/noOfferMatchType';
import { ChangeEvent } from 'react';

const messageOption = {
  'true': MESSAGE.CHECK_ALL,
  'false': MESSAGE.UN_CHECK_ALL
};

const handleNoOfferAllMatch = async (event: ChangeEvent<HTMLInputElement>) => {
  const { checked } = event.target;
  const { entities } = store.getState().eventPickingTable;
  if (entities.length) {
    const confirmResult = await ToastrWrapper.confirm(
      messageOption[`${checked}`]
    );
    if (confirmResult.value) {
      Spinner.show();

      const matchNoOffer: TNoOfferMatch[] = (entities as TDataSource[]).map(
        element => ({
          infoSiteMatchId: element.infoSiteMatchId,
          isNoOffer: checked,
          childMatchId: element.childMatchId
        })
      );

      await store.dispatch(updateNoOfferMatchThunk(matchNoOffer));
      Spinner.hide();
    }
  }
};

export default handleNoOfferAllMatch;