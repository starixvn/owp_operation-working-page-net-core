import { TMatchCell } from '../../../type/table';

const MatchCell = ({
  className,
  home,
  away
}: TMatchCell) => {
  return (
    <td className={className}>
      <span className='home'>
        {home}
      </span>
      <span className='away'>
        {away}
      </span>
    </td>
  );
};

export default MatchCell;
