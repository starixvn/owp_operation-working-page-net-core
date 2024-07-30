import { useSelector } from 'react-redux';
import CreateTicketButton from '../../../Shared/createTicketNew/CreateTicketButton';

import { THeader } from '../../type/Header';
import Checkbox from '../component/Checkbox';
import { selectDataTaskList } from '../selector/selector';
import {
  handleShowUnreadMessage,
  handleHideCompleteMatches
} from '../table/taskListSlice';
import useStableDispatch from './../../../Shared/customHook/useStableDispatch';

const Header = (props: THeader) => {
  const {
    isHideCompletedMatches,
    isShowUnreadMsg,
    ticketPermission
  } = useSelector(selectDataTaskList);
  const dispatch = useStableDispatch();

  return (
    <div className='header'>
      <div className='left-header'>
        <Checkbox
          id='completed-match'
          text='Hide completed matches'
          handleClick={() => dispatch(handleHideCompleteMatches())}
          value={isHideCompletedMatches}
        />
        <Checkbox
          id='message-issue'
          text='Show unread message only'
          handleClick={() => dispatch(handleShowUnreadMessage())}
          value={isShowUnreadMsg}
        />
        {ticketPermission.HasWrite && (
          <CreateTicketButton buttonType='live-game' />
        )}
      </div>
      {props.refreshComponent}
    </div>
  );
};

export default Header;
