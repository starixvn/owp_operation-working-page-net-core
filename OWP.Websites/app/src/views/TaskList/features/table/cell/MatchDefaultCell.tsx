import { CellProps } from '../../../type/TTaskListTable';

const MatchDefaultCell = (props: CellProps) => {
  const { dataSource } = props;
  const { Home, Away, MatchCode } = dataSource;
  return (
    <>
      <a className='team-name'>
        <p className='home-team'>{Home}</p>
        <p className='away-team'>{Away}</p>
      </a>
      <p className='match-code'>{MatchCode}</p>
    </>
  );
};

export default MatchDefaultCell;
