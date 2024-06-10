import { useEffect } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import ViewCreateTicketBoard from './ViewCreateTicketBoard';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { setSelectedRecipients } from '../../../Shared/createTicketNew/createTicketBoardSliceNew';

const propTypes = {
  creatorInfo: PropTypes.any
};
const CreateTicketPlusCircle = props => {
  const dispatch = useStableDispatch();
  const { creatorInfo } = props;
  const { isOpenTicket, modelByRowIndex } = useSelector(
    state => state.chatBoard
  );

  const { selectedRecipients, subTypeSelected } = useSelector(
    state => state.createTicketBoard
  );

  useEffect(() => {
    creatorInfo.userName && dispatch(setSelectedRecipients([creatorInfo]));
  }, [creatorInfo, dispatch]);

  return (
    <>
      {isOpenTicket && (
        <ViewCreateTicketBoard
          modelByRowIndex={modelByRowIndex}
          selectedRecipients={selectedRecipients}
          creatorInfo={creatorInfo}
          subTypeSelected={subTypeSelected}
        />
      )}
    </>
  );
};

CreateTicketPlusCircle.propTypes = propTypes;
export default CreateTicketPlusCircle;
