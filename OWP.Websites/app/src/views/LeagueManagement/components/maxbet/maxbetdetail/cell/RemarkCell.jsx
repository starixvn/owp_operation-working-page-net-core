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

const RemarkCell = ({ data, rowIndex }) => {
  const [canEdit, setCanEdit] = useState(false);
  const [remark, setRemark] = useState(data.Remark);
  const store = useStore();
  const dispatch = useDispatch();
  const input = createRef();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = hasWriteOnPageMaxbetGeneral(policyData);
  const handleUpdateRemark = () => {
    const newRemark = input.current.value;
    setCanEdit(false);
    setRemark(newRemark);
    if (!isEqual(data.Remark, newRemark)) {
      const model = buildModelLeagueDetail(data, 'Remark', newRemark);
      const extraModel = {
        key: 'Remark',
        value: newRemark,
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
      className='col-remark'
      id={`Remark${rowIndex}`}
      onClick={() => setCanEdit(policyData.lstNotePolicy.HasWrite)}>
      {!canEdit && <label className='lbl-remark'>{remark}</label>}
      {canEdit && (
        <input
          autoFocus
          ref={input}
          disabled={!hasWrite}
          type='text'
          name='league'
          className='input-field'
          defaultValue={remark}
          onBlur={handleUpdateRemark}
          maxLength='100'
        />
      )}
      {remark && !canEdit && (
        <UncontrolledTooltip placement='top' target={`Remark${rowIndex}`}>
          {remark}
        </UncontrolledTooltip>
      )}
    </Cell>
  );
};

RemarkCell.propTypes = propTypes;

export default RemarkCell;
