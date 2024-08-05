
import DeleteIcon from 'mdi-react/DeleteIcon';

const RemoveCell = ({index}:{index: number}) => {
  const handleRemoveSetting=()=>{

  }
  return (
    <td style={{textAlign: 'center'}}>
      <span
      onClick={handleRemoveSetting}
      >
    <DeleteIcon
          className='icon-delete'
          size={13}
        />
        </span>
    </td>
  );
};

export default RemoveCell;
