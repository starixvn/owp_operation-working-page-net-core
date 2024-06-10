import { createRef, useState } from 'react';
import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import NumericTextbox from 'react-numeric-textbox';
import { useStore, useDispatch } from 'react-redux';
import { hasWriteOnPageMaxbetGeneral } from '../../../../helper/commonHelper';
import isEqual from 'lodash.isequal';
import { buildModelLeagueDetail } from '../../../../helper/buildPostModelHelper';
import { postDataWithExtraModel } from '../../../../actions/action';
import { LEAGUEDETAIL_UPDATEINFO } from '../../../../constants/action-type';
import Utils from '../../../../../Shared/Utils';
import { buildUpdateSimpleApi } from '../../../../../Shared/Constants';

const correctMaxbet = maxbet =>
  maxbet > 0 ? Utils.convertToThousand(maxbet) : maxbet;

const MaxbetCell = props => {
  const { data, modelKey, rowIndex } = props;
  const [valueMaxbet, setValueMaxbet] = useState(
    typeof data[modelKey] === 'number' ? correctMaxbet(data[modelKey]) : null
  );
  const input = createRef();
  const store = useStore();
  const dispatch = useDispatch();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = hasWriteOnPageMaxbetGeneral(policyData);
  const handleBlur = () => {
    const newMaxbet =
      typeof input.current.value === 'number'
        ? input.current.value * 1000
        : null;
    if (!isEqual(data[modelKey], newMaxbet)) {
      const model = buildModelLeagueDetail(data, modelKey, newMaxbet);
      const extraModel = {
        rowIndex,
        key: modelKey,
        value: newMaxbet
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
          ref={input}
          className='input-field'
          value={valueMaxbet}
          disabled={!hasWrite}
          min={0}
          max={999999}
          decimals={3}
          autoCorrect
          format=''
          name='maxbet'
          onChange={value => setValueMaxbet(value)}
          onBlur={handleBlur}
        />
        {(valueMaxbet || valueMaxbet === 0) && <span>K</span>}
      </div>
    </Cell>
  );
};

MaxbetCell.propTypes = {
  modelKey: PropTypes.string.isRequired,
  data: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};

export default MaxbetCell;
