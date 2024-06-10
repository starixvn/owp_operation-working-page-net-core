import CheckboxCell from '../../common/CheckboxCell';
import { useDispatch, useStore } from 'react-redux';
import { postDataWithExtraModel } from '../../../actions/action';
import MaxbetConst from '../../../constants/MaxbetConst';
import PropTypes from 'prop-types';
import { MAXBETGENARAL_INFO_UPDATE_BLACK_LIST } from '../../../constants/action-type';
import { hasWriteOnPageMaxbetGeneral } from '../../../helper/commonHelper';
import { buildUpdateSimpleApi } from '../../../../Shared/Constants';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const BlackListCell = ({ dataSource, rowIndex }) => {
  const store = useStore();
  const dispatch = useDispatch();
  const isBlackList = dataSource[rowIndex].IsBlackList;
  const OWPLeagueId = dataSource[rowIndex].OWPLeagueId;
  const { policyData } = store.getState().policyReducer;
  const handleChangeChecked = checked => {
    const model = {
      IsBlackList: checked,
      OWPLeagueId,
      apiKey: MaxbetConst.maxbetUpdateLeagueIsBlacklistAPIKey
    };
    const extraModel = {
      rowIndex,
      IsBlackList: checked
    };
    dispatch(
      postDataWithExtraModel(
        buildUpdateSimpleApi(),
        model,
        extraModel,
        MAXBETGENARAL_INFO_UPDATE_BLACK_LIST
      )
    );
  };
  return (
    <CheckboxCell
      id={`blacklist-${rowIndex}`}
      defaultCheck={isBlackList}
      onChangeChecked={handleChangeChecked}
      hasWrite={hasWriteOnPageMaxbetGeneral(policyData)}
    />
  );
};

BlackListCell.propTypes = propTypes;
export default BlackListCell;
