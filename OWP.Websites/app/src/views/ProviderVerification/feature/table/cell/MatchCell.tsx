import { TMatchCell } from '../../../type/table';

const MatchCell = ({ className, home, away }: TMatchCell) => {
  return (
    <div className={className}>
      <span className='home'>{home}</span>
      <span className='away'>{away}</span>
    </div>
  );
};

export default MatchCell;
