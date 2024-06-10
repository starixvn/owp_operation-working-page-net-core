import { Modal, ModalBody, ModalHeader } from 'reactstrap';
import useStore from '../../app/store';
import { TSettingPopup } from '../../typing/settingPopup';
import { shallow } from 'zustand/shallow';
import ToastrWrapper from '../../../components/ToastrWrapper';

const SettingPopup = (props: TSettingPopup) => {
  const { title, children, isAdd, onSave } = props;

  const {
    setPopupType,
    resetSetting,
    getPolicy,
    resetErrorMessage,
    isChangeData
  } = useStore(
    state => ({
      setPopupType: state.setPopupType,
      resetSetting: state.resetSetting,
      getPolicy: state.getPolicy,
      resetErrorMessage: state.resetErrorMessage,
      isChangeData: state.isChangeData
    }),
    shallow
  );

  const policy = getPolicy();

  const closePopup = () => {
    setPopupType(null);
    resetSetting();
    resetErrorMessage();
  };

  const handleClosePopup = async () => {
    if (isChangeData(isAdd)) {
      const result = await ToastrWrapper.confirm(
        'Your unsaved changes will be lost. Are you sure?'
      );
      if (result.value) {
        closePopup();
        return;
      } else {
        return;
      }
    }
    closePopup();
  };

  return (
    <Modal isOpen scrollable className='setting-modal' size='xl' centered>
      <ModalHeader className='header' toggle={handleClosePopup}>
        <span className='league-header-title'>{title}</span>
      </ModalHeader>
      <ModalBody>
        {children}
        <div className='list-btn'>
          <button className='button-cancel' onClick={handleClosePopup}>
            Cancel
          </button>
          {policy.HasWrite && (
            <button className='button-submit' onClick={onSave}>
              {isAdd ? 'Add' : 'Save'}
            </button>
          )}
        </div>
      </ModalBody>
    </Modal>
  );
};

export default SettingPopup;
