type Props = {
  handleOnClosePopup: () => void;
  handleSubmit: () => void;
};

const ButtonGroup = ({ handleOnClosePopup, handleSubmit }: Props) => {
  return (
    <div>
      <div className='btn-group-add-new'>
        <button className='button-close' onClick={handleOnClosePopup}>
          Cancel
        </button>
        <button className='button-submit' onClick={handleSubmit}>
          Submit
        </button>
      </div>
    </div>
  );
};

export default ButtonGroup;
