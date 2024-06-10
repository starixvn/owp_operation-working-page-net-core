import PropTypes from 'prop-types';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const RemarkCell = props => {
  const { rowIndex, dataSource } = props;
  const { lstNote, remark, homeRemark, awayRemark, infoSiteBlackList } =
    dataSource[rowIndex];

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

  const renderTeamRemark = (teamRemark, label) =>
    teamRemark && (
      <span>
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
      <div className='remark-cell'>
        {renderLstNote()}
        {renderRemark()}
        {renderHomeRemark()}
        {renderAwayRemark()}
        {renderBlackListInfoSite()}
      </div>
    </>
  );
};

RemarkCell.propTypes = propTypes;
export default RemarkCell;
