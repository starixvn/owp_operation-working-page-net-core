import PropTypes from 'prop-types';
import { UncontrolledTooltip } from 'reactstrap';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const RemarkCell = props => {
  const { rowIndex, dataSource } = props;
  const {
    lstNote,
    remark,
    remark2,
    homeRemark,
    awayRemark,
    infoSiteBlackList
  } = dataSource[rowIndex];

  const renderLstNote = () =>
    lstNote && (
      <span>
        <strong>LST Note: </strong>
        {lstNote}
      </span>
    );

  const renderRemark = () =>
    remark && (
      <span>
        <strong>Remark: </strong>
        {remark}
      </span>
    );

  const renderRemark2 = () =>
    remark2 && (
      <span>
        <strong>Information: </strong>
        {remark2}
      </span>
    );

  const renderTeamRemark = (teamRemark, label) =>
    teamRemark && (
      <span style={{ display: 'flex' }}>
        <strong>{label}: </strong>
        <span
          className={
            teamRemark.toLowerCase().includes('bad ranking strike out')
              ? 'bad-ranking'
              : ''
          }>
          {teamRemark}
        </span>
      </span>
    );

  const renderAwayRemark = () => renderTeamRemark(awayRemark, 'Away Remark');
  const renderHomeRemark = () => renderTeamRemark(homeRemark, 'Home Remark');

  const renderBlackListInfoSite = () =>
    infoSiteBlackList.length > 0 && (
      <>
        {infoSiteBlackList.map((blackList, i) => {
          return (
            <span key={i} className='black-list'>
              {blackList}
            </span>
          );
        })}
      </>
    );

  return (
    <>
      <div className='remark-cell' id={`remark${rowIndex}`}>
        {renderLstNote()}
        {renderRemark()}
        {renderRemark2()}
        {renderHomeRemark()}
        {renderAwayRemark()}
        {renderBlackListInfoSite()}
      </div>
      <UncontrolledTooltip
        className='tooltip-remark'
        target={`remark${rowIndex}`}>
        {renderLstNote()}
        {renderRemark()}
        {renderRemark2()}
        {renderHomeRemark()}
        {renderAwayRemark()}
      </UncontrolledTooltip>
    </>
  );
};

RemarkCell.propTypes = propTypes;
export default RemarkCell;
