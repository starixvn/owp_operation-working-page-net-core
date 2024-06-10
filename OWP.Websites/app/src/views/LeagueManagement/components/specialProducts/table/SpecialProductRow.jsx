import MaxbetCell from './cell/MaxbetCell';
import PropTypes from 'prop-types';

const SpecialProductRow = props => {
  const { data } = props;

  return <MaxbetCell value={data.Maxbet_FTHDP} />;
};

SpecialProductRow.propTypes = {
  data: PropTypes.object.isRequired
};

export default SpecialProductRow;
