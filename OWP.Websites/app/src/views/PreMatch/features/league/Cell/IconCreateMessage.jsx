import PropTypes from 'prop-types';
import { PlusCircleIcon, PlusIcon } from 'mdi-react';
import { useSelector } from 'react-redux';
import {
  openTicketPopup,
  getModelByRowIndex,
  closePopup
} from '../../../../Shared/chatBoardNew/chatBoardSlice';
import {
  changeErrorRecipient,
  closeCreatePopup,
  setSubTypeSelected,
  setSelectedRecipients
} from '../../../../Shared/createTicketNew/createTicketBoardSliceNew';
import useStableDispatch from '../../../../Shared/customHook/useStableDispatch';
import { backupGetIssueModel } from '../../signalR/signalRSlice';
import { IconRocket, IconRocketChat } from '../../../../Shared/common/icons/common-icons';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};


const IconCreateMessage = props => {
  const { dataSource, rowIndex } = props;

  const hasWrite = useSelector(state => state.policy).permission.HasWrite;
  const { defaultRecipients } = useSelector(state => state.createTicketBoard);
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
    dispatch(setSelectedRecipients(defaultRecipients));
  };

  const renderQuickAddTicketHandler = () => (
    <>
      <span style={{ margin: '10px' }}>
        <PlusCircleIcon className='icon-plus' />
      </span>
      <div className='popup-add-ticket'>
        <div
          className='button-add-ticket btn-quick'
          onClick={() => console.log('Add Match Group Ticket')}>
          <IconRocketChat className={'mdi-icon icon-plus icon-rocket'} />
          <span className='btn-name'>Match Group Discussion</span>
        </div>
        <div
          className='button-add-ticket btn-add'
          onClick={handleOpenCreateTicket}>
          <PlusIcon className='icon-plus' />
          <span className='btn-name'>New Ticket</span>
        </div>
      </div>
    </>
  );

  if (!hasWrite) {
    return null;
  }


  return (
    <div className='ticket-cell'>
      <PlusCircleIcon
        onClick={() => {
          handleOpenCreateTicket();
        }}
        className='icon-plus'
      />
      {dataSource[rowIndex].InfoSiteMatchId && <div className=''>{renderQuickAddTicketHandler()}</div>}
    </div>
    
  );
};

IconCreateMessage.propTypes = propTypes;
export default IconCreateMessage;
