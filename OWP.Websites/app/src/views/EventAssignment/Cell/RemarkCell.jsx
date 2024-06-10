import { PureComponent, Fragment } from 'react';
import { v4 as uuidv4 } from 'uuid';
import PropTypes from 'prop-types';

class RemarkCell extends PureComponent {
  render() {
    const { data, rowIndex } = this.props;
    const { Round, Division, TeamRound } = data[rowIndex];
    const arrayCombine = [];
    data[rowIndex].Remark &&
      arrayCombine.push(<p className='remark-note'>{data[rowIndex].Remark}</p>);
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
        {arrayCombine.map(v => (
          <Fragment key={uuidv4()}>{v}</Fragment>
        ))}
      </div>
    );
  }
}

const propTypesBase = {
  data: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

RemarkCell.propTypes = propTypesBase;

export default RemarkCell;
