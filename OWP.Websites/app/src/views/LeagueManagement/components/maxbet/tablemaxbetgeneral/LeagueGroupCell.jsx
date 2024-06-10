import PropTypes from 'prop-types';
import {
  rebuildLeagueGroupOption as includeEmptyItem,
  hasWriteOnPageMaxbetGeneral
} from '../../../helper/commonHelper';
import { useStore, useDispatch } from 'react-redux';
import { Cell } from 'fixed-data-table-2';
import { postDataWithExtraModel } from '../../../actions/action';
import MaxbetConst from '../../../constants/MaxbetConst';
import { message } from '../../../constants/Const';
import { v4 as uuidv4 } from 'uuid';
import { MAXBETGENARAL_INFO_UPDATE_LEAGUEGROUP } from '../../../constants/action-type';
import { buildUpdateSimpleApi } from '../../../../Shared/Constants';
import ToastsWrapper from '../../../../components/ToastrWrapper';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};
const LeagueGroupCell = ({ dataSource, rowIndex }) => {
  const dispatch = useDispatch();
  const store = useStore();
  const { leagueGroupOption } = store.getState().leagueGroupReducer;
  const { policyData } = store.getState().policyReducer;

  const leagueGroupOptionIncludeNoneItem = includeEmptyItem(leagueGroupOption);

  const handleChangeLeagueGroup = async event => {
    const { target } = event.nativeEvent;
    const index = target.selectedIndex;
    const label = target[index].text;
    const GroupValue = event.target.value;
    const OWPLeagueId = dataSource[rowIndex].OWPLeagueId;

    const confirmResult = await ToastsWrapper.show({
      type: 'warning',
      html: message.changeLeagueGroup,
      showCloseButton: true,
      showCancelButton: true
    });

    if (confirmResult.value) {
      const model = {
        LeagueGroupId: parseInt(GroupValue),
        OWPLeagueId,
        apiKey: MaxbetConst.maxbetUpdateLeagueGroupAPIKey
      };
      const extraModel = {
        rowIndex,
        GroupValue,
        GroupLabel: label
      };
      dispatch(
        postDataWithExtraModel(
          buildUpdateSimpleApi(),
          model,
          extraModel,
          MAXBETGENARAL_INFO_UPDATE_LEAGUEGROUP
        )
      );
    }
  };

  return (
    <Cell key={uuidv4()} className='league-group'>
      <div className='select-wrapper-default'>
        <select
          className='browser-default'
          disabled={!hasWriteOnPageMaxbetGeneral(policyData)}
          value={dataSource[rowIndex].GroupValue}
          onChange={handleChangeLeagueGroup}>
          {leagueGroupOptionIncludeNoneItem.map(opt => {
            return (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            );
          })}
        </select>
      </div>
    </Cell>
  );
};

LeagueGroupCell.propTypes = propTypes;
export default LeagueGroupCell;
