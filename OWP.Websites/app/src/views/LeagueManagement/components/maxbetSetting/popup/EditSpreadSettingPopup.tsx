import { isEqual } from 'lodash';
import { useRef, KeyboardEvent } from 'react';
import { Input, Modal, ModalBody, ModalHeader } from 'reactstrap';
import ToastrWrapper from '../../../../components/ToastrWrapper';
import useStore, { RootState } from '../store/storeNative';

const EditSpreadSettingPopup = () => {
  const {
    editModel,
    isOpenEditPopup,
    setCloseEditPopup,
    updateMaxbetSetting,
    getMaxbetSettings
  } = useStore((state: RootState) => state);

  const inputRef = useRef<HTMLInputElement>(null);

  const handleSave = () => {
    const value = inputRef?.current?.value;
    const inputValue = value ? Number(value) : null;
    const DELAY_TIME = 200;
    if (inputValue === null || inputValue < 1 || inputValue > 100) {
      ToastrWrapper.showError('Please input the correct value');
    } else {
      const formattedNumber = inputValue / 100;
      if (!isEqual(editModel.oldValue, formattedNumber)) {
        updateMaxbetSetting({ ...editModel, value: formattedNumber });
        setTimeout(() => {
          getMaxbetSettings();
        }, DELAY_TIME);
      }
      setCloseEditPopup();
    }
  };

  const handleKeyDown = (event: KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter') {
      handleSave();
    }
  };

  return (
    <Modal
      autoFocus={false}
      isOpen={isOpenEditPopup}
      scrollable
      className='edit-modal'
      centered
      size='sm'>
      <ModalHeader className='header' toggle={setCloseEditPopup}>
        Edit Spread
      </ModalHeader>
      <ModalBody className='edit-modal-body'>
        <div className='input-inline'>
          <Input
            innerRef={inputRef}
            autoFocus
            placeholder='New value'
            min={1}
            max={100}
            type='number'
            onKeyDown={handleKeyDown}
          />
          <span className='thousand'>%</span>
        </div>
        <div className='list-btn'>
          <button className='button-export' onClick={handleSave}>
            Save
          </button>
        </div>
      </ModalBody>
    </Modal>
  );
};

export default EditSpreadSettingPopup;
