import Utils from "../../../../Shared/Utils";
import ToastrWrapper from "../../../../components/ToastrWrapper";
import { useAppDispatch } from "../../../app/hooksNative";
import store from "../../../app/storeNative";
import { getEventTickets } from "../../table/slice/ticketSliceThunkNative";

const Command = () => {
  const dispatch= useAppDispatch();
  const handleSubmit=(event: React.MouseEvent<HTMLButtonElement, MouseEvent>)=>{
    event.preventDefault();
    const {
      ticketTypeId,
      ticketStatuses}= store.getState().filter.entities.filterInfo;
  
    if (!ticketTypeId) {
      ToastrWrapper.showError(
        'Please select at least one option for Ticket Type or Subtype'
      );
      return;
    }

    if (!ticketStatuses) {
      ToastrWrapper.showError('Please select at least one option for Status');
      return;
    }

    dispatch(getEventTickets());
  };

  return (
    <button className='button-submit' onClick={handleSubmit}>
    Search
  </button>
  );
};

export default Command;