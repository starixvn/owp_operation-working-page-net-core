import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { setIsOpenAddNewSetting } from '../../table/leagueInfoTableSlice/reducer';

const CommandOfAdd = () => {
  const dispatch = useAppDispatch();

  const handleSave = () => {
    console.log('close');
  };
  return (
    <div className='list-btn'>
      <button
        className='btn-cancel'
        onClick={() => dispatch(setIsOpenAddNewSetting())}>
        Cancel
      </button>
      <button className='button-export' onClick={handleSave}>
        Save
      </button>
    </div>
  );
};

export default CommandOfAdd;
