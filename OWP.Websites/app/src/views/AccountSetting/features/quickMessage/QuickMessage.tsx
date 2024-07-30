import { useEffect } from 'react';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import Loading from '../../../components/Loading';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import Action from './cell/Action';
import Message from './cell/Message';
import Shortcut from './cell/Shortcut';
import { queryQuickMessageThunk } from './slice/quickMessageThunk';
import PlusCircleIcon from 'mdi-react/PlusCircleIcon';
import { addQuickMessage } from './slice/quickMessageSlice';

const QuickMessage = () => {
  const loading = useAppSelector(state => state.quickMessageSlice.loading);
  const editIndex = useAppSelector(state => state.quickMessageSlice.editIndex);
  const quickMessage = useAppSelector(state => state.quickMessageSlice.quickMessage);
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(queryQuickMessageThunk());
  }, []);

  if (loading === REQUEST_STATUS.pending) {
    return <Loading />;
  }

  return (
    <div className='quick-message'>
      <div className='head'>
        <h4 className='title'>Quick Message</h4>
        <button className='plus-icon' onClick={() => dispatch(addQuickMessage())}>
          <PlusCircleIcon color='#1E88E5' />
        </button>
      </div>
      <table>
        <thead>
          <tr>
            <th className='shortcut-col'>Shortcut</th>
            <th className='message-col'>Message</th>
            <th className='action-col'>Action</th>
          </tr>
        </thead>
        <tbody>
          {quickMessage.length > 0 && quickMessage.map((message, index) => {
            const isEdit = editIndex === index;
            return (
              <tr key={index}>
                <Shortcut shortcut={message.shortcut} isEdit={isEdit} />
                <Message message={message.message} isEdit={isEdit} />
                <Action isEdit={isEdit} index={index} />
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
};

export default QuickMessage;


