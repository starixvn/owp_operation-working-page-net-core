import PropTypes from 'prop-types';
import { PlusCircleIcon } from 'mdi-react';
import { useSelector } from 'react-redux';
import {
  openTicketPopup,
  getModelByRowIndex,
  closePopup
} from '../../../../Shared/chatBoardNew/chatBoardSlice';
import {
  changeErrorRecipient,
  closeCreatePopup,
  setSubTypeSelected
} from '../../../../Shared/createTicketNew/createTicketBoardSliceNew';
import useStableDispatch from '../../../../Shared/customHook/useStableDispatch';
import { backupGetIssueModel } from '../../signalR/signalRSlice';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const IconCreateMessage = props => {
  const { dataSource, rowIndex } = props;

  const hasWrite = useSelector(state => state.policy).permission.HasWrite;

  const dispatch = useStableDispatch();

  const handleOpenCreateTicket = () => {
    const model = {
      ...dataSource[rowIndex]
    };
    dispatch(getModelByRowIndex(model));
    dispatch(backupGetIssueModel(model));
    dispatch(closePopup());
    dispatch(closeCreatePopup());
    dispatch(openTicketPopup());
    dispatch(changeErrorRecipient(false));
    dispatch(setSubTypeSelected([...[{ value: 0, label: '' }]]));
  };

  if (!hasWrite) {
    return null;
  }

  return (
    <PlusCircleIcon
      onClick={() => {
        handleOpenCreateTicket();
      }}
      className='icon-plus'
    />
  );
};

IconCreateMessage.propTypes = propTypes;
export default IconCreateMessage;
