import { MARKET } from '../../../const/const.json';
import SelectFilterFactory from './SelectFilterFactory';
import PropTypes from 'prop-types';

const propTypes = {
  onSelectedChange: PropTypes.func
};

const MarketFilter = props => {
  return (
    <div className='market-filter-control'>
      <div className='label'>Market</div>
      <SelectFilterFactory option={MARKET} {...props} />
    </div>
  );
};

MarketFilter.propTypes = propTypes;
export default MarketFilter;
