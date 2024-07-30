import { isEqual } from 'lodash';
import { useRef, KeyboardEvent } from 'react';
import { Input, Modal, ModalBody, ModalHeader } from 'reactstrap';
import ToastrWrapper from '../../../../components/ToastrWrapper';
import { covertToNumber, validateMaxbet } from '../helper/helper';
import useStore, { RootState } from '../store/storeNative';

const EditMaxbetSettingPopup = () => {
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
    if (!validateMaxbet(inputValue)) {
      ToastrWrapper.showError('Please input the correct value');
    } else {
      const formattedNumber = covertToNumber(inputValue);
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
        Edit Max Bet
      </ModalHeader>
      <ModalBody className='edit-modal-body'>
        <div className='input-inline'>
          <Input
            innerRef={inputRef}
            autoFocus
            placeholder='New value'
            min={-1}
            max={999999}
            type='number'
            onKeyDown={handleKeyDown}
          />
          <span className='thousand'>K</span>
        </div>
        <div className='hint'>
          Hint: Please input -1 if you would like to follow FT HDP Main Match
        </div>
        <div className='hint-chinese'>
          請輸入 &ldquo;-1&rdquo; 是根據全場最高投注額來設定
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

export default EditMaxbetSettingPopup;
