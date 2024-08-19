import Spinner from '../../../components/Spinner';
import ToastrWrapper from '../../../components/ToastrWrapper';
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
    if (matchSelected.length === 0) {
      ToastrWrapper.showError('You must select at least one match!');
      return;
    }
    dispatch(setOpenPopupState({ key, value }));
  };

  const handleMatchSelection = async (isNoOffer: boolean) => {
    if (matchSelected.length === 0) {
      ToastrWrapper.showError('You must select at least one match!');
      return;
    }

    const action = isNoOffer ? 'unselect' : 'select';
    const confirmMessage = `Are you sure that you want to ${action} the game for those events?`;
    const confirmResult = await ToastrWrapper.confirmYesNo(confirmMessage);

    if (confirmResult.value) {
      const noOfferMatch = matchSelected.map(x => ({
        infoSiteMatchId: x.infoSiteMatchId,
        childMatchId: null,
        isNoOffer
      }));

      Spinner.show();
      await dispatch(updateNoOfferMatchThunk(noOfferMatch));
      Spinner.hide();
    }
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
      <li className='offer' onClick={() => handleMatchSelection(false)}>
        Offer
      </li>
      <li className='no-offer' onClick={() => handleMatchSelection(true)}>
        No Offer
      </li>
    </div>
  );
};

export default DropdownActionHeader;
