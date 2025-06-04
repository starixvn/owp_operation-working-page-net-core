import { useState } from 'react';
import { useSelector } from 'react-redux';
import CreateTicketButton from '../../../Shared/createTicketNew/CreateTicketButton';

import { THeader } from '../../type/Header';
import Checkbox from '../component/Checkbox';
import { selectDataTaskList } from '../selector/selector';
import {
  handleShowUnreadMessage,
  handleHideCompleteMatches,
  filterByKeyword
} from '../table/taskListSlice';
import useStableDispatch from './../../../Shared/customHook/useStableDispatch';

const Header = (props: THeader) => {
  const {
    isHideCompletedMatches,
    isShowUnreadMsg,
    ticketPermission
  } = useSelector(selectDataTaskList);

  const dispatch = useStableDispatch();
  const [keyWord, setKeyWord] = useState('');

  const handleChangeKeyWord = (event: React.ChangeEvent<HTMLInputElement>) => {
    setKeyWord(event.target.value);
    dispatch(filterByKeyword(event.target.value));
  };

  const handleSearch = () => {
    dispatch(filterByKeyword(keyWord));
  };

  const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter') {
      handleSearch();
    }
  };

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
      <div className='filter'>
        {props.refreshComponent}
        <div className='filter-control'>
          <input
            className='input-field'
            placeholder='Search by keyword'
            value={keyWord}
            onChange={handleChangeKeyWord}
            onKeyDown={handleKeyDown}
          />
        </div>
      </div>
    </div>
  );
};

export default Header;
