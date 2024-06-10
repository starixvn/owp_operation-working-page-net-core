import { useAppDispatch } from "../app/hooks";
import store from "../app/store";
import { editEventPickings } from "../table/eventPickingTableSlice/eventSliceThunkNative";

const Edit = ({ onSetMode }: { onSetMode: () => void }) => {
  const dispatch = useAppDispatch();

  const handleEdit = () => {
    const { param } = store.getState().filter;
    dispatch(editEventPickings(param));
    onSetMode();
  };

  return (
    <button className='button-submit' onClick={handleEdit}>
      Edit
    </button>
  );
};

export default Edit;