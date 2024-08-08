import { useAppDispatch } from '../../app/hooks';
import { TOpenPopupState } from '../../types/dataTableType';
import { setOpenPopupState } from '../eventPickingTableSlice/reducer';

type TDropdownActionHeader = {
  refValue: React.RefObject<HTMLDivElement>;
};

const DropdownActionHeader = ({ refValue }: TDropdownActionHeader) => {
  const dispatch = useAppDispatch();
  const handleOpenPopup = (key: keyof TOpenPopupState, value: boolean) => {
    dispatch(setOpenPopupState({ key, value }));
  };

  return (
    <div className='dropdown-action is-show' ref={refValue}>
      <li
        className='add-special-product'
        onClick={() => handleOpenPopup('isOpenAddSpecialProduct', true)}>
        Add Special Product
      </li>
      <li
        className='remove-special-product'
        onClick={() => handleOpenPopup('isOpenRemoveSpecialProduct', true)}>
        Remove Special Product
      </li>
      <li className='offer'>Select Game</li>
      <li className='no-offer'>Unselect Game</li>
    </div>
  );
};

export default DropdownActionHeader;
