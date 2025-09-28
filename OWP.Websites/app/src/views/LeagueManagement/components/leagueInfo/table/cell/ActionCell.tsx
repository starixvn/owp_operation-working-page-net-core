import DeleteIcon from 'mdi-react/DeleteIcon';
import CopyIcon from 'mdi-react/ContentCopyIcon';
import HistoryIcon from 'mdi-react/HistoryIcon';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import Spinner from '../../../../../components/Spinner';
import { removeSettingService } from '../../service/service';
import { useAppDispatch } from '../../app/hooks';
import { removeBySettingId } from '../leagueInfoTableSlice/reducer';
import { setIsOpenAddNewSetting } from '../../table/leagueInfoTableSlice/reducer';
import { TDataSource } from '../TableType';
import { setInitSetting } from '../../addPopup/slice/addSettingSlice';
import { useState } from 'react';
import HistoryModal from '../../modal/HistoryModal';

interface Props {
  leagueSettingId: number;
  dataSource: TDataSource;
}
const ActionCell = (props: Props) => {
  const { leagueSettingId, dataSource } = props;
  const [openLog, setOpenLog] = useState(false);
  const dispatch = useAppDispatch();

  const handleRemoveSetting = async () => {
    const confirmResult = await ToastrWrapper.confirmYesNo(
      'Are you sure that you want to delete this league setting ?'
    );
    if (confirmResult.value) {
      Spinner.show();
      const response = await removeSettingService(leagueSettingId);
      if (response?.status === 200) {
        await ToastrWrapper.showInfo(
          'The setting has been remove successfully!'
        );
        dispatch(removeBySettingId({ settingId: leagueSettingId }));
      }
      Spinner.hide();
    }
  };
  const handleCopySetting = () => {
    dispatch(setInitSetting(dataSource));
    dispatch(setIsOpenAddNewSetting());
  };

  return (
    <>
      <td style={{ textAlign: 'center' }}>
        <span onClick={handleCopySetting}>
          <CopyIcon className='icon-copy' size={17} />
        </span>
        {leagueSettingId ? (
          <span onClick={handleRemoveSetting}>
            <DeleteIcon className='icon-delete' size={17} />
          </span>
        ) : (
          <></>
        )}
        <span onClick={() => setOpenLog(true)}>
          <HistoryIcon className='icon-history-event' size={17} />
        </span>
      </td>
      {openLog && (
        <HistoryModal
          leagueSettingId={leagueSettingId}
          leagueName={dataSource.league}
          onToggle={() => setOpenLog(false)}
        />
      )}
    </>
  );
};

export default ActionCell;
