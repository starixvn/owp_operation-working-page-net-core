import PersonIcon from '@mui/icons-material/Person';
import GroupIcon from '@mui/icons-material/Group';
import SectionIcon from './SectionIcon';
import MatchAssigneeIcon from './MatchAssigneeIcon';
import PropTypes from 'prop-types';

export const RecipientIcon = ({ type = 1 }) => {
  const icon = new Map();
  icon.set(1, <PersonIcon />);
  icon.set(2, <GroupIcon />);
  icon.set(3, <SectionIcon />);
  icon.set(4, <MatchAssigneeIcon />);
  return <>{icon.get(type)}</>;
};

RecipientIcon.propTypes = {
  type: PropTypes.number
};
