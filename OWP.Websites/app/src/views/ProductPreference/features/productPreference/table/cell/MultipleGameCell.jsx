import DropdownCell from './dropdownCell/DropdownCell';
import { dbColumnName } from '../../../../Const.json';
import PropTypes from 'prop-types';
import { updateUserUserInfo } from '../../productSlice';
import useStableDispatch from '../../../../../Shared/customHook/useStableDispatch';
import store from '../../../../app/storeNative';

const propTypes = {
  productInfo: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const MultipleGameCell = ({ productInfo, rowIndex }) => {
  const { multipleGames } = dbColumnName;
  const { multipleGamesId, userId, username } = productInfo;
  const { filter, policy } = store.getState();
  const { multipleMaxGamesOption } = filter.dropdownOptions;
  const { HasWrite, ActorId, Username } = policy.permission;
  const dispatch = useStableDispatch();

  const handleSelectChange = value => {
    dispatch(
      updateUserUserInfo({
        key: 'multipleGamesId',
        rowIndex,
        value
      })
    );
  };

  const props = {
    options: multipleMaxGamesOption,
    userId,
    username,
    label: multipleGames,
    selected: multipleGamesId,
    hasWrite: HasWrite,
    actorId: ActorId,
    actorName: Username,
    onSelectChange: handleSelectChange
  };
  return <DropdownCell {...props} />;
};

MultipleGameCell.propTypes = propTypes;
export default MultipleGameCell;
