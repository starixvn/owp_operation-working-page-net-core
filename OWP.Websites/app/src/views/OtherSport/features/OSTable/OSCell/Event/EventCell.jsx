import PropTypes from 'prop-types';
import Utils from '../../../../../Shared/Utils';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};
const { toLowerCaseForFilter } = Utils;

const EventCell = props => {
  const { dataSource, rowIndex } = props;
  const {
    league,
    category,
    noHDP,
    matchOffer,
    autoLink,
    homeName,
    away
  } = dataSource[rowIndex];

  const renderAutoLink = content => {
    const autoLinkContent =
      toLowerCaseForFilter(content) === 'yes' ? 'Auto Link' : content;
    return <span className='auto-link'>{autoLinkContent}</span>;
  };

  return (
    <div className='league-name-cell'>
      <div className='league-name'>{league}</div>
      <div className='match-cell'>
        <div className='home-name'>{homeName}</div>
        <div className='vs'>-vs-</div>
        <div className='away-name'>{away}</div>
      </div>
      <div>
        {category && <span className='category'>{category}</span>}
        {noHDP && <span className='no-hdp'>{noHDP}</span>}
        {matchOffer && <span className='match-offer'>{matchOffer}</span>}
        {autoLink && renderAutoLink(autoLink)}
      </div>
    </div>
  );
};

EventCell.propTypes = propTypes;
export default EventCell;
