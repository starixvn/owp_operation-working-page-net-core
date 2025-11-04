import { Button } from 'reactstrap';

const DirectionTicketCommand = () => {
  // Example: Get dispatch from Redux if needed
  // const dispatch = useDispatch();

  const handleSave = () => {
    // Add save logic here
    // dispatch(saveDirectionTicketThunk());
    console.log('Save clicked');
  };

  const handleCancel = () => {
    // Add cancel/close logic here
    // dispatch(closeDirectionTicketPopup());
    console.log('Cancel clicked');
  };

  return (
    <div className='direction-ticket-command'>
      <Button color='primary' onClick={handleSave}>
        Save
      </Button>
      <Button color='secondary' onClick={handleCancel}>
        Cancel
      </Button>
    </div>
  );
};

export default DirectionTicketCommand;

