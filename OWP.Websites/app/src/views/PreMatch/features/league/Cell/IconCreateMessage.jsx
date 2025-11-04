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
  setSelectedRecipients,
  openPopupCreateDiscussion
} from '../../../../Shared/createTicketNew/createTicketBoardSliceNew';
import useStableDispatch from '../../../../Shared/customHook/useStableDispatch';
import { backupGetIssueModel } from '../../signalR/signalRSlice';
import { IconRocketChat } from '../../../../Shared/common/icons/common-icons';
import ToastrWrapper from '../../../../components/ToastrWrapper';

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

  const handleOpenCreateDiscussion = () => {
    const adminSiteMatchId = dataSource[rowIndex].adminSiteMatchId;
    if(adminSiteMatchId && adminSiteMatchId !== 0) {
      dispatch(openPopupCreateDiscussion());
    } else {
      ToastrWrapper.showError('The system could not find the  AdminSiteId to get the reader for  this match to send to  direction ticket. Please check the match again.');
    }
  };

  const renderQuickAddTicketHandler = () => (
    <>
      <div className='popup-add-ticket'>
        <div
          className='button-add-ticket btn-quick'
          onClick={handleOpenCreateDiscussion}>
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
    <div>
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
