type Prop = {
  onClose: () => void,
  onSave: () => void
};

const Command = (props: Prop) => {
  const { onClose, onSave } = props;
  return (
    <div className='list-btn'>
      <button
        className='button button-cancel'
        onClick={onClose}>
        Cancel
      </button>
      <button
        className='button button-save'
        onClick={onSave}>
        Save
      </button>
    </div>
  );
};


export default Command;