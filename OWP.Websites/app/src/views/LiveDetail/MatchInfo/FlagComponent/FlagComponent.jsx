import PropTypes from 'prop-types';
import { Flag } from './FlagLogic';
import FlagMatch from './FlagMatch';

const propTypes = {
  teamName: PropTypes.object,
  leagueName: PropTypes.string
};

const FlagComponent = props => {
  const { homeName, AwayName } = props.teamName;

  const flag = Flag({
    awayName: AwayName,
    homeName,
    leagueName: props.leagueName
  });

  return (
    <>
      <FlagMatch {...props} {...flag} />
    </>
  );
};

FlagComponent.propTypes = propTypes;
export default FlagComponent;
