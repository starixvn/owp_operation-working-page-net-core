import Spinner from '../../components/Spinner';
import { useAppDispatch } from '../app/hooks';
import store from '../app/store';
import { getEventPickings } from '../table/eventPickingTableSlice/eventSliceThunkNative';
import {
  updateMainEventsThunk,
  updateSpecialProductThunk
} from './updateEventThunkNative';

const Save = ({ onSetMode }: { onSetMode: () => void }) => {
  const dispatch = useAppDispatch();
  const handleSave = async () => {
    Spinner.show();
    const { entities } = store.getState().eventPickingTable;
    const eventModified = entities.filter(
      e => e.isModified || e.isReferenceLeagueManagement
    );
    const mainEvents = eventModified
      .filter(e => !e.isChildMatch)
      .map(e => ({
        InfoSiteMatchId: e.infoSiteMatchId,
        SpreadHdp: e.spreadHdp || null,
        SpreadOu: e.spreadOu || null,
        NoOfHDP: e.noOfHDP || null,
        MaxbetHdp: e.maxbetHdp || null,
        LstNote: e.lstNote || null,
        Remark2LeagueManagement: e.remark2LeagueManagement || null
      }));
    const specialProduct = eventModified
      .filter(e => e.isChildMatch)
      .map(e => ({
        ParentId: e.infoSiteMatchId,
        MinorBettypeId: e.minorBettypeId,
        SpreadHdp: e.spreadHdp || null,
        SpreadOu: e.spreadOu || null,
        NoOfHDP: e.noOfHDP || null,
        MaxbetHdp: e.maxbetHdp || null
      }));

    await Promise.allSettled([
      dispatch(updateMainEventsThunk(mainEvents)),
      dispatch(updateSpecialProductThunk(specialProduct))
    ]);
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
