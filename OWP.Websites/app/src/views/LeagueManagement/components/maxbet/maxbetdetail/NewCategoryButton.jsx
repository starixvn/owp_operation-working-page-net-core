import { useDispatch } from 'react-redux';
import { postData } from '../../../actions/action';
import MaxbetConst from '../../../constants/MaxbetConst';
import Service from '../../../Service';
import { QUERY_LEAGUE_DETAIL } from '../../../constants/action-type';
import { useParams } from 'react-router-dom';
import { buildQuerySimpleApi } from '../../../../Shared/Constants';

const NewCategoryButton = () => {
  const { id } = useParams();
  const dispatch = useDispatch();
  const handleAddNewCategory = event => {
    event.preventDefault();
    (async () => {
      const addModel = {
        OWPLeagueId: parseInt(id, 10),
        apiKey: MaxbetConst.leagueManagementInsertLeagueSettingAPIKey
      };
      const queryModel = {
        OWPLeagueId: parseInt(id, 10),
        apiKey: MaxbetConst.leagueManagementGetLeagueSettingAPIKey
      };
      await Service.addNewLeague(addModel);
      dispatch(
        postData(buildQuerySimpleApi(), queryModel, QUERY_LEAGUE_DETAIL)
      );
    })();
  };

  return (
    <button className='button-submit' onClick={handleAddNewCategory}>
      New Category
    </button>
  );
};

export default NewCategoryButton;
