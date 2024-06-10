import PropTypes from 'prop-types';
import { ChevronDoubleDownIcon, ChevronDoubleUpIcon } from 'mdi-react';
import FlagComponent from './FlagComponent';
import { checkLeague } from './FlagLogic';

const propTypes = {
  isExpand: PropTypes.bool,
  leagueName: PropTypes.string,
  onChangeGeneralArea: PropTypes.func,
  teamName: PropTypes.object
};

const FlagWrapper = props => {
  const { isExpand, leagueName, onChangeGeneralArea, teamName } = props;
  const { homeName, AwayName } = teamName;

  const { isHaveFlag } = checkLeague({ leagueName });
  const renderFlagInfo = () => {
    return (
      <div className='flag-block'>
        <FlagComponent
          isExpand={isExpand}
          teamName={{ homeName, AwayName }}
          leagueName={leagueName}
        />
      </div>
    );
  };

  const renderExpandButton = () => {
    return isExpand ? (
      <span className='icon-toggle'>
        <ChevronDoubleUpIcon
          onClick={onChangeGeneralArea}
          className='icon-expand'
        />
      </span>
    ) : (
      <span className='icon-toggle'>
        <ChevronDoubleDownIcon
          onClick={onChangeGeneralArea}
          className='icon-collapse'
        />
      </span>
    );
  };

  if (!isHaveFlag) {
    return null;
  }

  return (
    <>
      {renderFlagInfo()}
      {renderExpandButton()}
    </>
  );
};

FlagWrapper.propTypes = propTypes;
export default FlagWrapper;
