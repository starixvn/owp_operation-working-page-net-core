import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { TOpenPopupState } from '../../types/dataTableType';
import { setOpenPopupState } from '../eventPickingTableSlice/reducer';
import { Special_Product_Type } from '../TableSchema.json';
import { Modal, ModalBody, ModalFooter } from 'reactstrap';

const AddSpecialProductPopup = () => {
  const { isOpenAddSpecialProduct } = useAppSelector(
    state => state.eventPickingTable.openPopupState
  );
  const dispatch = useAppDispatch();

  const handleCancel = (key: keyof TOpenPopupState, value: boolean) => {
    dispatch(setOpenPopupState({ key, value }));
  };

  return (
    <Modal
      isOpen={isOpenAddSpecialProduct}
      scrollable
      modalClassName='edit-modal'
      centered>
      <ModalBody>
        <div className='option-special-product'>
          {Special_Product_Type.map((element, index) => (
            <div className='wrapper-element' key={`${element}-${index}`}>
              <input
                className='select-special-product form-check-input'
                type='checkbox'
              />
              <span className='name-product'>{element}</span>
            </div>
          ))}
        </div>
      </ModalBody>
      <ModalFooter>
        <button
          className='btn-cancel'
          onClick={() => handleCancel('isOpenAddSpecialProduct', false)}>
          Cancel
        </button>
        <button className='button-export'>Add</button>
      </ModalFooter>
    </Modal>
  );
};

export default AddSpecialProductPopup;
