import DeleteIcon from 'mdi-react/DeleteIcon';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import Spinner from '../../../../../components/Spinner';
import { removeSettingService } from '../../service/service';
import { useAppDispatch } from '../../app/hooks';
import { removeBySettingId } from '../leagueInfoTableSlice/reducer';

const RemoveCell = ({ leagueSettingId }: { leagueSettingId: number }) => {
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

  return (
    <td style={{ textAlign: 'center' }}>
      {leagueSettingId && <span onClick={handleRemoveSetting}>
        <DeleteIcon className='icon-delete' size={13} />
      </span>}
    </td>
  );
};

export default RemoveCell;
