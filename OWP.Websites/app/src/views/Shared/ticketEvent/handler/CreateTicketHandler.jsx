import PropTypes from 'prop-types';
import { PlusCircleIcon } from 'mdi-react';

const propTypes = {
  handleOpenCreateTicket: PropTypes.func,
  hasWrite: PropTypes.bool
};
const CreateTicketHandler = props => {
  const { handleOpenCreateTicket, hasWrite } = props;

  if (!hasWrite) {
    return null;
  }

  return (
    <span style={{ margin: '10px' }}>
      <PlusCircleIcon onClick={handleOpenCreateTicket} className='icon-plus' />
    </span>
  );
};

CreateTicketHandler.propTypes = propTypes;

export default CreateTicketHandler;
