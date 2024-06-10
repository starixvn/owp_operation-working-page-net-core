import PropTypes from 'prop-types';
import { dbColumnName } from '../../../../Const.json';
import DropdownCell from './dropdownCell/DropdownCell';
import { updateUserUserInfo } from '../../productSlice';
import useStableDispatch from '../../../../../Shared/customHook/useStableDispatch';
import store from '../../../../app/storeNative';

const propTypes = {
  productInfo: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const RoleCell = ({ productInfo, rowIndex }) => {
  const { role } = dbColumnName;
  const { roleId, ...rest } = productInfo;
  const { filter, policy } = store.getState();
  const { HasWrite, ActorId, Username } = policy.permission;
  const dispatch = useStableDispatch();
  const { roleOption } = filter.dropdownOptions;

  const handleSelectChange = value => {
    dispatch(updateUserUserInfo({
      key: 'roleId',
      rowIndex,
      value
    }));
  };

  const props = {
    options: roleOption,
    ...rest,
    label: role,
    selected: roleId,
    hasWrite: HasWrite,
    actorId: ActorId,
    actorName: Username,
    onSelectChange: handleSelectChange
  };
  return <DropdownCell {...props} />;
};

RoleCell.propTypes = propTypes;
export default RoleCell;
