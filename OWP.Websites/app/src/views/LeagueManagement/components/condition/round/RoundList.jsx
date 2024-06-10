import PropTypes from 'prop-types';
import MaxbetConst from '../../../constants/MaxbetConst';
import RowsExpression from '../../common/RowsExpression';
import { ACTION_ROUND } from '../../../constants/action-type';

const propTypes = {
  rounds: PropTypes.array.isRequired
};
const RoundList = ({ rounds }) => {
  const expressionProps = {
    models: rounds,
    titleName: 'Round',
    hasWrite: true,
    modelsLength: rounds.length,
    actionType: ACTION_ROUND,
    modelExpressionDefault: {
      round: {
        expression: MaxbetConst.expressionOption[0],
        value: ''
      }
    }
  };
  return <RowsExpression {...expressionProps} />;
};

RoundList.propTypes = propTypes;
export default RoundList;
