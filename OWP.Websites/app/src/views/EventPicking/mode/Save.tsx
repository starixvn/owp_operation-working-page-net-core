import Spinner from "../../components/Spinner";
import { useAppDispatch } from "../app/hooks";
import store from "../app/store";
import { getEventPickings } from "../table/eventPickingTableSlice/eventSliceThunkNative";
import { updateEventsThunk } from "./updateEventThunkNative";

const Save = ({ onSetMode }: { onSetMode: () => void }) => {
  const dispatch = useAppDispatch();
  const handleSave = async () => {
    Spinner.show();
    const { entities } = store.getState().eventPickingTable;
    const eventModified = entities.filter(e => e.isModified || e.isReferenceLeagueManagement).map(e => ({
      InfoSiteMatchId: e.infoSiteMatchId,
      SpreadHdp: e.spreadHdp||null,
      SpreadOu: e.spreadOu||null,
      NoOfHDP: e.noOfHDP||null,
      MaxbetHdp: e.maxbetHdp||null,
      LstNote: e.lstNote||null
    }));
    
    eventModified.length&&await dispatch(updateEventsThunk(eventModified));
    Spinner.hide();
    const { param } = store.getState().filter;
    await dispatch(getEventPickings(param));
    onSetMode();
  };

  const handleCancel = async () => {
    const { param } = store.getState().filter;
    await dispatch(getEventPickings(param));
    onSetMode();
  };
  return (
    <>
      <button className='button-cancel btn-cancel' onClick={handleCancel}>
        Cancel
      </button>
      <button className='button-submit' onClick={handleSave}>
        Save
      </button>
    </>
  );
};

export default Save;