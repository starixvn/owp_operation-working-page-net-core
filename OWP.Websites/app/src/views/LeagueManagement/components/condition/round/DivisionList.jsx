import PropTypes from 'prop-types';
import MaxbetConst from '../../../constants/MaxbetConst';
import RowsExpression from '../../common/RowsExpression';
import { ACTION_DIVISION } from '../../../constants/action-type';

const propTypes = {
  divisions: PropTypes.array.isRequired
};
const DivisionList = ({ divisions }) => {
  const expressionProps = {
    models: divisions,
    titleName: 'Division',
    hasWrite: true,
    modelsLength: divisions.length,
    actionType: ACTION_DIVISION,
    modelExpressionDefault: {
      division: {
        expression: MaxbetConst.expressionOption[0],
        value: ''
      }
    }
  };
  return <RowsExpression {...expressionProps} />;
};

DivisionList.propTypes = propTypes;
export default DivisionList;
