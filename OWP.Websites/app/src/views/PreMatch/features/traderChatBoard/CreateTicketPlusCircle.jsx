import { useEffect } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import ViewCreateTicketBoard from './ViewCreateTicketBoard';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { setDefaultRecipients } from '../../../Shared/createTicketNew/createTicketBoardSliceNew';

const propTypes = {
  creatorInfo: PropTypes.any
};
const CreateTicketPlusCircle = props => {
  const dispatch = useStableDispatch();
  const { creatorInfo } = props;
  const { isOpenTicket, modelByRowIndex } = useSelector(
    state => state.chatBoard
  );
  useEffect(() => {
    creatorInfo.userName && dispatch(setDefaultRecipients([creatorInfo]));
  }, [creatorInfo, dispatch]);

  return (
    <>
      {isOpenTicket && (
        <ViewCreateTicketBoard
          modelByRowIndex={modelByRowIndex}
          creatorInfo={creatorInfo}
        />
      )}
    </>
  );
};

CreateTicketPlusCircle.propTypes = propTypes;
export default CreateTicketPlusCircle;
