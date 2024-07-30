import { CellProps } from '../../../type/TTaskListTable';

const RemarkCell = (props: CellProps) => {
  const { Remark, Round, Division, TeamRound } = props.dataSource;
  const separator = (Remark || Division) && ',';
  return (
    <>
      <div className='js-content-remark'>
        {Remark && <p>{Remark}</p>}
        {Round &&
          <p>
            <b>Round</b>: {Round}
            {separator}{' '}
          </p>
        }
        {TeamRound &&
          <p>
            <b>Round (H-A/T)</b>: {TeamRound}
          </p>
        }
        {Division &&
          <p>
            <b>Division</b>: {Division}
          </p>
        }
      </div>
    </>
  );
};

export default RemarkCell;
