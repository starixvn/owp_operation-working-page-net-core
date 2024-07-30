import { createRef } from 'react';
import PropTypes from 'prop-types';
import Select from 'react-select';
import { useDispatch } from 'react-redux';
import MaxbetConst from '../../constants/MaxbetConst';
import { crud } from '../../actions/action';

const propTypes = {
  item: PropTypes.object.isRequired,
  index: PropTypes.number.isRequired,
  hasWrite: PropTypes.bool,
  actionType: PropTypes.object.isRequired
};
const Expression = props => {
  const { item, index, hasWrite, actionType } = props;
  const dispatch = useDispatch();
  let selectRef = createRef();
  const handleChangeExpression = () => {
    dispatch(
      crud(actionType.UPDATE, {
        key: 'expression',
        value: selectRef.state.value,
        index
      })
    );
  };
  return (
    <Select
      isDisabled={!hasWrite}
      ref={select => (selectRef = select)}
      options={MaxbetConst.expressionOption}
      defaultValue={item.expression}
      className='basic-multi-select'
      classNamePrefix='select'
      name='list-condition'
      onBlur={handleChangeExpression}
    />
  );
};

Expression.propTypes = propTypes;
export default Expression;
