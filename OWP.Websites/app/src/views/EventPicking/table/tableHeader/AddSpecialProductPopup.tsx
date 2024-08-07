import { Special_Product_Type } from '../TableSchema.json';
import { Modal, ModalBody, ModalFooter } from 'reactstrap';

const AddSpecialProductPopup = () => {
  return (
    <Modal
      isOpen
      scrollable
      wrapClassName='add-special-popup'
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
        <button className='button-submit'>Add</button>
      </ModalFooter>
    </Modal>
  );
};

export default AddSpecialProductPopup;
