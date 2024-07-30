import { useState } from 'react';
import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import NumericTextbox from 'react-numeric-textbox';
import { hasWriteOnPageMaxbetGeneral } from '../../../../helper/commonHelper';
import { useStore, useDispatch } from 'react-redux';
import isEqual from 'lodash.isequal';
import { buildModelLeagueDetail } from '../../../../helper/buildPostModelHelper';
import { LEAGUEDETAIL_UPDATEINFO } from '../../../../constants/action-type';
import { postDataWithExtraModel } from '../../../../actions/action';
import Utils from '../../../../../Shared/Utils';
import { buildUpdateSimpleApi } from '../../../../../Shared/Constants';

const correctSpread = spread =>
  spread > 0 ? Utils.convertToPercent(spread) : spread;

const SpreadCell = props => {
  const store = useStore();
  const dispatch = useDispatch();
  const { modelKey, data, rowIndex } = props;
  const [valueSpread, setValueSpread] = useState(
    typeof data[modelKey] === 'number' ? correctSpread(data[modelKey]) : null
  );
  const { policyData } = store.getState().policyReducer;
  const hasWrite = hasWriteOnPageMaxbetGeneral(policyData);
  const handleBlur = () => {
    const newSpread =
      typeof valueSpread === 'number' ? valueSpread / 100 : null;
    if (!isEqual(data[modelKey], newSpread)) {
      const model = buildModelLeagueDetail(data, modelKey, newSpread);
      const extraModel = {
        rowIndex,
        key: modelKey,
        value: newSpread
      };
      dispatch(
        postDataWithExtraModel(
          buildUpdateSimpleApi(),
          model,
          extraModel,
          LEAGUEDETAIL_UPDATEINFO
        )
      );
    }
  };
  return (
    <Cell>
      <div className='input-value'>
        <NumericTextbox
          className='input-field'
          disabled={!hasWrite}
          value={valueSpread}
          read
          min={0}
          max={99}
          decimals={2}
          autoCorrect
          format=''
          name='spread'
          onChange={value => setValueSpread(value)}
          onBlur={handleBlur}
        />
        {(valueSpread || valueSpread === 0) && <span>%</span>}
      </div>
    </Cell>
  );
};

SpreadCell.propTypes = {
  modelKey: PropTypes.string.isRequired,
  data: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};

export default SpreadCell;
