import { CellProps } from '../../../type/TableType';

const RemarkCell = (props: CellProps): JSX.Element => {
  const { rowIndex, dataSource } = props;
  const { Remark, Round, Division, TeamRound } = dataSource[rowIndex];

  return (
    <div className='remark-cell'>
      {Remark && <span>{Remark},</span>}
      <br></br>
      {Round && (
        <span>
          <strong>Round:</strong> {Round}
        </span>
      )}
      <br></br>
      {TeamRound && (
        <span>
          <strong>Round (H-A/T):</strong> {TeamRound}
        </span>
      )}
      <br></br>
      {Division && (
        <span>
          <strong>Division:</strong> {Division}
        </span>
      )}
    </div>
  );
};

export default RemarkCell;
