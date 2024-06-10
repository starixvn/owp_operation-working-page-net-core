import PropTypes from 'prop-types';
import MaxbetConst from '../../constants/MaxbetConst';
import DeleteIcon from 'mdi-react/DeleteIcon';
import PlusIcon from 'mdi-react/PlusIcon';
import { useDispatch } from 'react-redux';
import { crud } from '../../actions/action';
import isEqual from 'lodash.isequal';

const propTypes = {
  index: PropTypes.number.isRequired,
  modelsLength: PropTypes.number.isRequired,
  hasWrite: PropTypes.bool,
  actionType: PropTypes.object.isRequired,
  modelExpressionDefault: PropTypes.object.isRequired
};
const Action = props => {
  const {
    index,
    modelsLength,
    hasWrite,
    actionType,
    modelExpressionDefault
  } = props;
  const dispatch = useDispatch();
  const handleAddNewTV = e => {
    e.preventDefault();
    dispatch(crud(actionType.ADD, modelExpressionDefault));
  };
  const handleDeleteTV = e => {
    e.preventDefault();
    dispatch(crud(actionType.DELETE, { index }));
  };
  return (
    <>
      {modelsLength > 1 && hasWrite && (
        <DeleteIcon
          name='iconDelete'
          className='icon-delete'
          size={13}
          onClick={handleDeleteTV}
        />
      )}
      {isEqual(index, modelsLength - 1) &&
        hasWrite &&
        index < MaxbetConst.tvList.maxLength - 1 && (
          <PlusIcon
            size={13}
            name='iconAdd'
            className='icon-add'
            onClick={handleAddNewTV}
          />
        )}
    </>
  );
};

Action.propTypes = propTypes;
export default Action;
