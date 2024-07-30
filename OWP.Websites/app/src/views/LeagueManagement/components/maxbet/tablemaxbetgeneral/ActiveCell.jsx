import CheckboxCell from '../../common/CheckboxCell';
import { useStore, useDispatch } from 'react-redux';
import { postDataWithExtraModel } from '../../../actions/action';
import MaxbetConst from '../../../constants/MaxbetConst';
import PropTypes from 'prop-types';
import { MAXBETGENARAL_INFO_UPDATE_ACTIVE } from '../../../constants/action-type';
import { hasWriteOnPageMaxbetGeneral } from '../../../helper/commonHelper';
import { buildUpdateSimpleApi } from '../../../../Shared/Constants';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const ActiveCell = ({ dataSource, rowIndex }) => {
  const store = useStore();
  const dispatch = useDispatch();
  const OWPLeagueId = dataSource[rowIndex].OWPLeagueId;
  const active =
    dataSource[rowIndex].IsActive ??
    store.getState().statusReducer.status.value === 1;
  const { policyData } = store.getState().policyReducer;
  const handleChangeChecked = checked => {
    const model = {
      IsActive: checked,
      OWPLeagueId,
      apiKey: MaxbetConst.maxbetUpdateLeagueIsActiveAPIKey
    };
    const extraModel = {
      rowIndex,
      IsActive: checked
    };
    dispatch(
      postDataWithExtraModel(
        buildUpdateSimpleApi(),
        model,
        extraModel,
        MAXBETGENARAL_INFO_UPDATE_ACTIVE
      )
    );
  };
  return (
    <CheckboxCell
      defaultCheck={active}
      id={`action-${rowIndex}`}
      onChangeChecked={handleChangeChecked}
      hasWrite={hasWriteOnPageMaxbetGeneral(policyData)}
    />
  );
};

ActiveCell.propTypes = propTypes;
export default ActiveCell;
