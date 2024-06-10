import { Fragment } from 'react';
import { TCellModel } from '../../type/type';

const RemarkCell = (props: TCellModel) => {
  const { dataSource, rowIndex } = props;
  const { Round, Division, TeamRound, Remark } = dataSource[rowIndex];
  const arrayCombine = [];
  Remark && arrayCombine.push(<p className='remark-note'>{Remark}</p>);
  Round &&
    arrayCombine.push(
      <p className='round-note'>
        <b>Round:</b>
        {Round}
      </p>
    );
  TeamRound &&
    arrayCombine.push(
      <p className='team-round-note'>
        <b>Round (H-A/T):</b>
        {TeamRound}
      </p>
    );
  Division &&
    arrayCombine.push(
      <p className='division-note'>
        <b>Division:</b>
        {Division}
      </p>
    );
  if (!arrayCombine.length) {
    return null;
  }
  return (
    <div className='remark-col'>
      {arrayCombine.map((v, index) => (
        <Fragment key={`remark_${index}`}>{v}</Fragment>
      ))}
    </div>
  );
};

export default RemarkCell;
