import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { TOpenPopupState } from '../../types/dataTableType';
import { setOpenPopupState } from '../eventPickingTableSlice/reducer';
import updateNoOfferMatchThunk from '../updateNoOfferMatchThunkNative';

type TDropdownActionHeader = {
  refValue: React.RefObject<HTMLDivElement>;
};

const DropdownActionHeader = ({ refValue }: TDropdownActionHeader) => {
  const dispatch = useAppDispatch();
  const matchSelected = useAppSelector(
    state => state.eventPickingTable.matchSelected
  );
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
      <li
        className='offer'
        onClick={() => dispatch(updateNoOfferMatchThunk(matchSelected))}>
        Select Game
      </li>
      <li
        className='no-offer'
        onClick={() => dispatch(updateNoOfferMatchThunk(matchSelected))}>
        Unselect Game
      </li>
    </div>
  );
};

export default DropdownActionHeader;
