import AddSettingPopup from '../addPopup/AddSettingPopup';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import { setIsOpenAddNewSetting } from '../table/leagueInfoTableSlice/reducer';

const AddLeagueSettingButton = () => {
  const dispatch = useAppDispatch();
  const { isOpenAddNewSetting } = useAppSelector(
    state => state.leagueInfoTableSlice
  );
  return (
    <>
      <button
        className='button-working-shift'
        onClick={() => dispatch(setIsOpenAddNewSetting())}>
        Add New Setting
      </button>
      {isOpenAddNewSetting && <AddSettingPopup />}
    </>
  );
};

export default AddLeagueSettingButton;
