import { createRef, useState } from 'react';
import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { useStore, useDispatch } from 'react-redux';
import { postDataWithExtraModel } from '../../../../actions/action';
import { LEAGUEDETAIL_UPDATEINFO } from '../../../../constants/action-type';
import { UncontrolledTooltip } from 'reactstrap';
import isEqual from 'lodash.isequal';
import { buildModelLeagueDetail } from '../../../../helper/buildPostModelHelper';
import { buildUpdateSimpleApi } from '../../../../../Shared/Constants';

const propTypes = {
  data: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const LstNoteCell = ({ data, rowIndex }) => {
  const [canEdit, setCanEdit] = useState(false);
  const [lSTNote, setLstNote] = useState(data.LSTNote);
  const dispatch = useDispatch();
  const input = createRef();
  const store = useStore();
  const { policyData } = store.getState().policyReducer;
  const handleUpdateNote = () => {
    const newNote = input.current.value;
    setLstNote(newNote);
    setCanEdit(false);
    if (!isEqual(data.LSTNote, newNote)) {
      const model = buildModelLeagueDetail(data, 'LSTNote', newNote);
      const extraModel = {
        key: 'LSTNote',
        value: newNote,
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
      className='col-lst-note'
      id={`LstNote${rowIndex}`}
      onClick={() => setCanEdit(policyData.lstNotePolicy.HasWrite)}>
      {!canEdit && <label className='lbl-lstNote'>{lSTNote}</label>}
      {canEdit && (
        <input
          autoFocus
          ref={input}
          disabled={!policyData.lstNotePolicy.HasWrite}
          type='text'
          name='note'
          className='input-field'
          defaultValue={lSTNote}
          onBlur={handleUpdateNote}
          maxLength='100'
        />
      )}
      {data.LSTNote && !canEdit && (
        <UncontrolledTooltip placement='top' target={`LstNote${rowIndex}`}>
          {lSTNote}
        </UncontrolledTooltip>
      )}
    </Cell>
  );
};

LstNoteCell.propTypes = propTypes;

export default LstNoteCell;
