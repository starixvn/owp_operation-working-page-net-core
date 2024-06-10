import { TableCell } from '../../TypeNative';

const LevelCell = ({ value }: TableCell) => {
  return (
    <div className='level '>
      <span>{value}</span>
    </div>
  );
};

export default LevelCell;
