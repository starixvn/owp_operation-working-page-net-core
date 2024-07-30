import AddSetting from '../add/AddSetting';
import EditSetting from '../edit/EditSetting';
import useStore from '../../app/store';
import { popup } from '../../osLeague.json';
import { shallow } from 'zustand/shallow';

const PopupWrapper = () => {
  const { popupType } = useStore(state => (
    {
      popupType: state.popupType
    }), shallow);

  const lookupPopup = {
    [popup.type.add]: <AddSetting />,
    [popup.type.edit]: <EditSetting />
  };
  return popupType && lookupPopup[popupType];
};


export default PopupWrapper;