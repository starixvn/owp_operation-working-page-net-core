import AddSettingPopup from '../addPopup/AddSettingPopup';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import { setIsOpenAddNewSetting } from '../table/leagueInfoTableSlice/reducer';
import { setValueDefault } from '../addPopup/slice/addSettingSlice';

const AddLeagueSettingButton = () => {
  const dispatch = useAppDispatch();
  const { isOpenAddNewSetting } = useAppSelector(
    state => state.leagueInfoTableSlice
  );

  const handleOpenAddNewSetting = () => {
    dispatch(setValueDefault());
    dispatch(setIsOpenAddNewSetting());
  };
  return (
    <>
      <button
        className='button-working-shift'
        onClick={handleOpenAddNewSetting}>
        Add New Setting
      </button>
      {isOpenAddNewSetting && <AddSettingPopup />}
    </>
  );
};

export default AddLeagueSettingButton;
