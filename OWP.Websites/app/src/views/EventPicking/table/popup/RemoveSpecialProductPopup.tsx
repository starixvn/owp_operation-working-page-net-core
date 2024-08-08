import { useState } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import {
  TActionOfSpecialProduct,
  TOpenPopupState
} from '../../types/dataTableType';
import { setOpenPopupState } from '../eventPickingTableSlice/reducer';
import { Special_Product_Type } from '../TableSchema.json';
import { Modal, ModalBody, ModalFooter } from 'reactstrap';
import { getEventPickings } from '../eventPickingTableSlice/eventSliceThunkNative';
import { removeSpecialProduct } from '../../service/Service';
import ToastrWrapper from '../../../components/ToastrWrapper';

const RemoveSpecialProductPopup = () => {
  const { isOpenRemoveSpecialProduct } = useAppSelector(
    state => state.eventPickingTable.openPopupState
  );
  const matchSelected = useAppSelector(
    state => state.eventPickingTable.matchSelected
  );
  const param = useAppSelector(state => state.filter.param);
  const dispatch = useAppDispatch();
  const [specialProductType, setSpecialProductType] = useState(Array<number>);

  const handleCancel = (key: keyof TOpenPopupState, value: boolean) => {
    dispatch(setOpenPopupState({ key, value }));
    setSpecialProductType([]);
  };

  const handleRemove = async () => {
    const model: TActionOfSpecialProduct = {
      ParentId: matchSelected.map(x => x.infoSiteMatchId).join(','),
      MinorBettypeId: specialProductType.join(',')
    };
    const response = await removeSpecialProduct(model);
    if (response.status === 200) {
      ToastrWrapper.showInfo('Delete success!');
      dispatch(getEventPickings(param));
    }
  };

  const handleOnChangeSpecial = (value: number) => {
    const newSelected = specialProductType.some(x => x === value)
      ? specialProductType.filter(x => x !== value)
      : [...specialProductType, value];
    setSpecialProductType(newSelected);
  };
  return (
    <Modal
      isOpen={isOpenRemoveSpecialProduct}
      scrollable
      modalClassName='edit-modal'
      centered>
      <ModalBody>
        <div className='option-special-product'>
          {Special_Product_Type.map((element, index) => (
            <div className='wrapper-element' key={`${element}-${index}`}>
              <input
                className='form-check-input'
                type='checkbox'
                checked={specialProductType.some(x => x === element.value)}
                onChange={() => handleOnChangeSpecial(element.value)}
              />
              <span className='name-product'>{element.label}</span>
            </div>
          ))}
        </div>
      </ModalBody>
      <ModalFooter>
        <button
          className='btn-cancel'
          onClick={() => handleCancel('isOpenRemoveSpecialProduct', false)}>
          Cancel
        </button>
        <button className='btn-remove' onClick={handleRemove}>
          Remove
        </button>
      </ModalFooter>
    </Modal>
  );
};

export default RemoveSpecialProductPopup;
