import { TableCell } from '../../TypeNative';

const MultipleGameCell = ({ value }: TableCell) => {
  return (
    <div className='multiple-game'>
      <span>{value}</span>
    </div>
  );
};

export default MultipleGameCell;
