import PropTypes from 'prop-types';

const propTypes = {
  subject: PropTypes.string.isRequired
};

const OtherIssueContent = props => {
  const { subject } = props;
  return (
    <span className='new-add-league-name ticket-title-link'>{subject}</span>
  );
};
OtherIssueContent.propTypes = propTypes;

export default OtherIssueContent;
