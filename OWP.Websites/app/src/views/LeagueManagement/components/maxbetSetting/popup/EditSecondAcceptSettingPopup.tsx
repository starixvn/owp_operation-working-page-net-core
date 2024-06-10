import { useState } from 'react';
import { Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';
import useStore, { RootState } from '../store/storeNative';
import { SelectOption } from '../typeNative';
import Select from 'react-select-ts';
import { FilterOptionOption } from 'react-select-ts/dist/declarations/src/filters';

const EditSecondAcceptSettingPopup = () => {
  const {
    editModel,
    isOpenEditPopup,
    setCloseEditPopup,
    updateMaxbetSetting,
    secondAcceptValues,
    getMaxbetSettings
  } = useStore((state: RootState) => state);
  const [selected, setSelected] = useState<SelectOption | null>(null);

  const handleChange = (event: SelectOption | null) => {
    setSelected(event);
  };

  const handleSave = () => {
    const DELAY_TIME = 200;
    if (selected != null) {
      updateMaxbetSetting({ ...editModel, value: selected.value });
      setTimeout(() => {
        getMaxbetSettings();
      }, DELAY_TIME);
    }
    setCloseEditPopup();
  };

  const handleFilter = (option: FilterOptionOption<SelectOption>, inputValue: string) => option.label.includes(inputValue);

  return (
    <Modal isOpen={isOpenEditPopup} scrollable className='edit-modal' centered autoFocus size='sm'>
      <ModalHeader className='header' toggle={setCloseEditPopup}>
        Edit Second Accept Value
      </ModalHeader>
      <ModalBody
        style={{ 'height': 300 }}
        className='edit-modal-body'>
        <Select
          menuIsOpen
          className='select-field'
          options={secondAcceptValues}
          onChange={handleChange}
          classNamePrefix="react-select"
          value={selected}
          data-testid='select-test-id'
          placeholder='Search'
          filterOption={handleFilter}
        />
      </ModalBody>
      <ModalFooter>
        <div className="list-btn">
          <button className='button-export' onClick={handleSave}>
            Save
          </button>
        </div>
      </ModalFooter>
    </Modal>
  );
};

export default EditSecondAcceptSettingPopup;