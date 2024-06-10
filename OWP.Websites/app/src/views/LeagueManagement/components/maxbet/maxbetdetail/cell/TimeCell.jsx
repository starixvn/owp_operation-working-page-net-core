import { createRef, useState } from 'react';
import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { useStore, useDispatch } from 'react-redux';
import { hasWriteOnPageMaxbetGeneral } from '../../../../helper/commonHelper';
import { buildModelLeagueDetail } from '../../../../helper/buildPostModelHelper';
import isEqual from 'lodash.isequal';
import { postDataWithExtraModel } from '../../../../actions/action';
import { LEAGUEDETAIL_UPDATEINFO } from '../../../../constants/action-type';
import { UncontrolledTooltip } from 'reactstrap';
import { buildUpdateSimpleApi } from '../../../../../Shared/Constants';

const propTypes = {
  data: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const TimeCell = ({ data, rowIndex }) => {
  const [canEdit, setCanEdit] = useState(false);
  const [timeDesc, setTimeDesc] = useState(data.TimeDesc);
  const dispatch = useDispatch();
  const input = createRef();
  const store = useStore();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = hasWriteOnPageMaxbetGeneral(policyData);
  const handleUpdateTime = () => {
    const newTimeDesc = input.current.value;
    setTimeDesc(newTimeDesc);
    setCanEdit(false);
    if (!isEqual(data.TimeDesc, newTimeDesc)) {
      const model = buildModelLeagueDetail(data, 'TimeDesc', newTimeDesc);
      const extraModel = {
        key: 'TimeDesc',
        value: newTimeDesc,
        rowIndex
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
    <Cell
      className='col-time'
      id={`Time${rowIndex}`}
      onClick={() => setCanEdit(policyData.lstNotePolicy.HasWrite)}>
      {!canEdit && <label className='lbl-time'>{timeDesc}</label>}
      {canEdit && (
        <input
          autoFocus
          ref={input}
          disabled={!hasWrite}
          type='text'
          name='league'
          className='input-field'
          defaultValue={timeDesc}
          onBlur={handleUpdateTime}
          maxLength='100'
        />
      )}
      {timeDesc && !canEdit && (
        <UncontrolledTooltip placement='top' target={`Time${rowIndex}`}>
          {timeDesc}
        </UncontrolledTooltip>
      )}
    </Cell>
  );
};

TimeCell.propTypes = propTypes;

export default TimeCell;
