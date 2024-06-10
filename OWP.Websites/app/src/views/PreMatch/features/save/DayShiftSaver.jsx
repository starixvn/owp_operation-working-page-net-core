import { useDispatch, useStore } from 'react-redux';
import { changeMode } from '../edit/editSlice';
import { resetCopyInfo } from '../copy/copySlice';
import { reloadDataAfterEdit } from '../filter/filterSlice';
import {
  clearStateDayShift,
  setIsShowLeague
} from '../league/shiftSlice/shiftSlice';
import Spinner from '../../../components/Spinner';
import { filterPrematchUpdated } from '../../helper/preMatchHelper';
import SaveComponent from './SaveComponent';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { getLastUpdate } from '../header/headerSlice';
import {
  LAST_UPDATED,
  netCoreUrl,
  URL,
  simpleApiQueryUrl
} from '../../const/const';
import { post } from '../../ImportPrematchHelperNative';

const DayShiftSaver = () => {
  const dispatch = useDispatch();
  const store = useStore();

  const commonDispatchAction = () => {
    dispatch(resetCopyInfo());
    dispatch(clearStateDayShift());
    dispatch(changeMode({ isEdit: false }));
    dispatch(reloadDataAfterEdit());
    dispatch(setIsShowLeague());
  };

  const handleCancel = e => {
    e.preventDefault();

    commonDispatchAction();
  };

  const createSaveModel = () => {
    const { dayShift, filter } = store.getState();
    const { entities, prematchIdDeleted, prematchIdUpdated } = dayShift;
    const { filterDate } = filter;
    const PrematchUpdated = filterPrematchUpdated(entities, prematchIdUpdated);
    const postModel = {
      url: URL.SAVE_PREMATCH_ASSIGNMENT,
      PrematchIdDeleted: prematchIdDeleted,
      PrematchUpdated,
      Date: filterDate,
      NetCoreUrl: netCoreUrl
    };

    return { postModel };
  };

  const handleSave = async event => {
    event.preventDefault();
    const model = {
      url: simpleApiQueryUrl,
      apiKey: LAST_UPDATED.apiKey,
      isDay: true
    };
    Spinner.show();
    const { postModel } = createSaveModel();
    await post(postModel);
    Spinner.hide();
    ToastrWrapper.showInfo('The assignment has been updated successfully!');
    dispatch(getLastUpdate(model));
    commonDispatchAction();
  };

  return <SaveComponent onSave={handleSave} onCancel={handleCancel} />;
};
export default DayShiftSaver;
