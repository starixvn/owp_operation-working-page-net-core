import { Cell } from 'fixed-data-table-2';
import DeleteIcon from 'mdi-react/DeleteIcon';
import PropTypes from 'prop-types';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import MaxbetConst from '../../../../constants/MaxbetConst';
import { postDataWithExtraModel } from '../../../../actions/action';
import { LEAGUEDETAIL_DELETE } from '../../../../constants/action-type';
import { useDispatch } from 'react-redux';
import { buildUpdateSimpleApi } from '../../../../../Shared/Constants';

const ActionCell = ({ data, rowIndex }) => {
  const dispatch = useDispatch();
  const handleDeleteLeague = async () => {
    const result = await ToastrWrapper.confirm(
      MaxbetConst.deleteLeagueConfirmation
    );
    if (result.value) {
      const model = {
        LeagueSettingId: data.LeagueSettingId,
        apiKey: MaxbetConst.leagueManagementDeleteLeagueSettingAPIKey
      };
      const extraModel = {
        rowIndex
      };
      dispatch(
        postDataWithExtraModel(
          buildUpdateSimpleApi(),
          model,
          extraModel,
          LEAGUEDETAIL_DELETE
        )
      );
    }
  };
  return (
    <Cell className='action-cell'>
      <DeleteIcon
        name='iconDelete'
        className='icon-delete'
        size={13}
        onClick={handleDeleteLeague}
      />
    </Cell>
  );
};

ActionCell.propTypes = {
  data: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};

export default ActionCell;
