import useStore, { RootState } from '../store/storeNative';
import { editPopupType } from '../Const.json';
import EditMaxbetSettingPopup from './EditMaxbetSettingPopup';
import EditSecondAcceptSettingPopup from './EditSecondAcceptSettingPopup';

const EditPopup = () => {
  const { popupType } = useStore((state: RootState) => state);

  const popup = {
    [editPopupType.maxbet]: <EditMaxbetSettingPopup />,
    [editPopupType.secondAccept]: <EditSecondAcceptSettingPopup />
  };
  return popupType && popup[popupType] || <></>;
};

export default EditPopup;