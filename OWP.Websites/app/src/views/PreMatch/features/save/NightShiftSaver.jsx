import { useDispatch, useStore } from 'react-redux';
import { changeMode } from '../edit/editSlice';
import { resetCopyInfo } from '../copy/copySlice';
import {
  clearStateNightShift,
  setIsShowLeague
} from '../league/shiftSlice/shiftSlice';
import Spinner from '../../../components/Spinner';
import ToastrWrapper from '../../../components/ToastrWrapper';
import SaveComponent from './SaveComponent';
import { LAST_UPDATED, simpleApiQueryUrl } from '../../const/const';
import { getLastUpdate } from '../header/headerSlice';
import { post } from '../../ImportPrematchHelperNative';
import { buildModelNightShiftSaver } from './modelSaveNightShiftBuilder';
import { reloadDataAfterEdit } from '../filter/filterSlice';

const NightShiftSaver = () => {
  const dispatch = useDispatch();
  const store = useStore();
  const commonDispatchAction = () => {
    dispatch(resetCopyInfo());
    dispatch(clearStateNightShift());
    dispatch(changeMode({ isEdit: false }));
    dispatch(reloadDataAfterEdit());
    dispatch(setIsShowLeague());
  };

  const handleCancel = e => {
    e.preventDefault();
    commonDispatchAction();
  };

  const handleSave = async event => {
    event.preventDefault();
    const { dayShift, header, filter } = store.getState();
    const { postModel, message } = buildModelNightShiftSaver(
      dayShift,
      header,
      filter
    );
    let canSave = true;
    if (message) {
      const confirmResult = await ToastrWrapper.confirmYesNo(message);
      canSave = confirmResult.value;
    }
    if (canSave) {
      Spinner.show();
      await post(postModel);
      ToastrWrapper.showInfo('The assignment has been updated successfully!');
      const model = {
        url: simpleApiQueryUrl,
        apiKey: LAST_UPDATED.apiKey,
        isDay: false
      };
      dispatch(getLastUpdate(model));
      commonDispatchAction();
      Spinner.hide();
    }
  };

  return <SaveComponent onSave={handleSave} onCancel={handleCancel} />;
};

export default NightShiftSaver;
