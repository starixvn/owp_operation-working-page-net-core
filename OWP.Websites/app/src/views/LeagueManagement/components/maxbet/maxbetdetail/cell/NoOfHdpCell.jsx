import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { v4 as uuidv4 } from 'uuid';
import { hasWriteOnPageMaxbetGeneral } from '../../../../helper/commonHelper';
import { useStore, useDispatch } from 'react-redux';
import MaxbetConst from '../../../../constants/MaxbetConst';
import { buildModelLeagueDetail } from '../../../../helper/buildPostModelHelper';
import { postDataWithExtraModel } from '../../../../actions/action';
import { LEAGUEDETAIL_UPDATEINFO } from '../../../../constants/action-type';
import { isEqual } from 'lodash';
import { buildUpdateSimpleApi } from '../../../../../Shared/Constants';

const propTypes = {
  modelKey: PropTypes.string.isRequired,
  data: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};
const NoOfHdpCell = props => {
  const { modelKey, data, rowIndex } = props;
  const store = useStore();
  const dispatch = useDispatch();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = hasWriteOnPageMaxbetGeneral(policyData);

  const handleChangeNoOfHdp = event => {
    const index = event.nativeEvent.target.selectedIndex;
    const label = event.nativeEvent.target[index].text;
    const model = buildModelLeagueDetail(data, modelKey, label);
    const extraModel = {
      rowIndex,
      key: modelKey,
      value: label
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
  let noOfHDP = MaxbetConst.noOfHDP.find(item =>
    isEqual(item.label, data[modelKey])
  );
  noOfHDP = noOfHDP ?? MaxbetConst.noOfHDP[MaxbetConst.noOfHDP.length - 1];
  return (
    <Cell key={uuidv4()} className='league-group'>
      <div className='select-wrapper-default'>
        <select
          className='browser-default'
          disabled={!hasWrite}
          defaultValue={noOfHDP.value}
          onChange={handleChangeNoOfHdp}>
          {MaxbetConst.noOfHDP.map(opt => {
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

NoOfHdpCell.propTypes = propTypes;
export default NoOfHdpCell;
