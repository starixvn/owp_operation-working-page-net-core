import { MESSAGE } from '../const/const.json';
import store from '../app/store';
import ToastrWrapper from '../../components/ToastrWrapper';
import Spinner from '../../components/Spinner';
import updateConfirmedThunk from '../table/updateConfirmedThunkNative';
import { convertToGMT8String } from './confirmedHelper';

const exportAndConfirmAllEvent = async exportCallback => {
  const { param } = store.getState().filter;
  const confirmResult = await ToastrWrapper.confirm(
    MESSAGE.EXPORT_AND_CONFIRM_ALL
  );
  const { lastSearchTime } = store.getState().filter;
  const gmt8TimeStr = convertToGMT8String(lastSearchTime);

  if (confirmResult.value) {
    Spinner.show();
    const { entities } = store.getState().eventPickingTable;
    await store.dispatch(exportCallback(param));
    if (entities.length) {
      const infoSiteMatchIds = entities
        .filter(element => !element.isMovedEventDate)
        .map(element => element.infoSiteMatchId);
      await store.dispatch(
        updateConfirmedThunk({
          infoSiteMatchIds,
          confirmed: true,
          confirmedAt: gmt8TimeStr || ''
        })
      );
    }
    Spinner.hide();
  }
};

const exportOnly = exportCallback => {
  (async () => {
    const { param } = store.getState().filter;
    Spinner.show();
    await store.dispatch(exportCallback(param));
    Spinner.hide();
  })();
};

export { exportAndConfirmAllEvent, exportOnly };
