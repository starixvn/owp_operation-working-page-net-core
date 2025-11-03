import Spinner from '../../components/Spinner';
import { useAppDispatch } from '../app/hooks';
import store from '../app/store';
import { getEventPickings } from '../table/eventPickingTableSlice/eventSliceThunkNative';
import { updateMainEventsThunk } from './updateEventThunkNative';

const Save = ({ onSetMode }: { onSetMode: () => void }) => {
  const dispatch = useAppDispatch();

  const handleSave = async () => {
    Spinner.show();
    const { param } = store.getState().filter;
    const { entities, entitiesOriginal } = store.getState().eventPickingTable;
    // Get all modified events from both filtered entities and original entities
    const allEntities = [...entities, ...entitiesOriginal];
    const seen = new Set<string>();
    const uniqueEntities = allEntities.filter(event => {
      const key = `${event.infoSiteMatchId}-${event.minorBettypeId}`;
      if (seen.has(key)) {
        return false;
      }
      seen.add(key);
      return true;
    });
    const eventModified = uniqueEntities.filter(
      e => e.isModified || e.isReferenceLeagueManagement
    );

    const mainEvents = eventModified.map(e => ({
      InfoSiteMatchId: e.infoSiteMatchId,
      SpreadHdp: e.spreadHdp || null,
      SpreadOu: e.spreadOu || null,
      NoOfHDP: e.noOfHDP || null,
      MaxbetHdp: e.maxbetHdp || null,
      MaxbetOu: e.maxbetOu || null,
      LstNote: e.lstNote || null,
      Remark2LeagueManagement: e.remark2LeagueManagement || null,
      IsChildMatch: e.isChildMatch,
      MinorBettypeId: e.minorBettypeId || 0
    }));

    await Promise.allSettled([
      dispatch(
        updateMainEventsThunk({
          Events: mainEvents,
          EventDate: param.date as string
        })
      )
    ]);
    Spinner.hide();

    // await dispatch(getEventPickings(param));
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
