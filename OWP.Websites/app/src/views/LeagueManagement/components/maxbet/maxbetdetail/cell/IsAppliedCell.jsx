import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import CheckboxCell from '../../../common/CheckboxCell';
import { hasWriteOnPageMaxbetGeneral } from '../../../../helper/commonHelper';
import { useStore, useDispatch } from 'react-redux';
import { buildModelLeagueDetail } from '../../../../helper/buildPostModelHelper';
import { postDataWithExtraModel } from '../../../../actions/action';
import { LEAGUEDETAIL_UPDATEINFO } from '../../../../constants/action-type';
import { buildUpdateSimpleApi } from '../../../../../Shared/Constants';

const IsAppliedCell = ({ data, rowIndex }) => {
  const store = useStore();
  const dispatch = useDispatch();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = hasWriteOnPageMaxbetGeneral(policyData);
  const handleIsAppliedChange = checked => {
    const model = buildModelLeagueDetail(data, 'IsApplied', checked);
    const extraModel = {
      rowIndex,
      key: 'IsApplied',
      value: checked
    };
    dispatch(
      postDataWithExtraModel(
        buildUpdateSimpleApi(),
        model,
        extraModel,
        LEAGUEDETAIL_UPDATEINFO
      )
    );
  };
  return (
    <Cell>
      <CheckboxCell
        hasWrite={hasWrite}
        defaultCheck={data.IsApplied}
        onChangeChecked={handleIsAppliedChange}
        id={`IsApplied_${rowIndex}_${data.IsApplied}`}
      />
    </Cell>
  );
};

IsAppliedCell.propTypes = {
  data: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};

export default IsAppliedCell;
