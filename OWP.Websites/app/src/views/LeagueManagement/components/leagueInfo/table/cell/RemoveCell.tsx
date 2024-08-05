
import DeleteIcon from 'mdi-react/DeleteIcon';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import Spinner from '../../../../../components/Spinner';
import { removeSettingService } from '../../service/service';

const RemoveCell = ({leagueSettingId}:{leagueSettingId: number}) => {
  
  const handleRemoveSetting=async ()=>{
    const confirmResult=await ToastrWrapper.confirmYesNo("Are you sure that you want to delete this league setting ?");
    if(confirmResult.value){
      Spinner.show();
      await removeSettingService(leagueSettingId);
      //await post(postModel);
      
      Spinner.hide();
    }
  };

  return (
    <td style={{textAlign: 'center'}}>
      <span
      onClick={handleRemoveSetting}
      >
    <DeleteIcon
          className='icon-delete'
          size={13}
        />
        </span>
    </td>
  );
};

export default RemoveCell;
