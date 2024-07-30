import PropTypes from 'prop-types';

const propTypes = {
  teamName: PropTypes.object,
  awayFlag: PropTypes.string,
  homeFlag: PropTypes.string,
  width: PropTypes.string,
  height: PropTypes.string
};

const FlagDetail = props => {
  const { homeName, AwayName } = props.teamName;
  const { awayFlag, homeFlag, width, height } = props;

  const renderFlag = flag => {
    const classNameFlag = flag ? 'flag-control' : 'flag-default';
    return (
      <img src={flag} width={width} height={height} className={classNameFlag} />
    );
  };

  const teamName = (
    <>
      <div>{homeName}</div>
      <div>-vs-</div>
      <div>{AwayName}</div>
    </>
  );
  return (
    <>
      {renderFlag(homeFlag)}
      <span className='team-flag'>{teamName}</span>
      {renderFlag(awayFlag)}
    </>
  );
};

FlagDetail.propTypes = propTypes;
export default FlagDetail;
