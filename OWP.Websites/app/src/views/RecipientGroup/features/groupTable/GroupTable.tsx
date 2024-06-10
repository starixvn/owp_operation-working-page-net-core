import { isEqual } from 'lodash';
import { useEffect } from 'react';
import { useSelector } from 'react-redux';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import usePermission from '../../../Shared/customHook/usePermission';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { RootState } from '../../app/storeNative';
import { functionKey } from '../../Const.json';
import { getRecipientGroup } from '../groupThunkNative';
import { GroupDataSource } from '../Type';
import ActionCell from './cell/ActionCell';
import RecipientCell from './cell/RecipientCell';

const GroupTable = () => {
  const recipientGroups = useSelector((state: RootState) => state.recipientGroup);
  const groups: GroupDataSource[] = recipientGroups.groups;
  const dispatch = useStableDispatch();
  const permissionModel = usePermission(functionKey);

  useEffect(() => {
    dispatch(getRecipientGroup());
  }, [dispatch]);

  if (isEqual(permissionModel.loading, REQUEST_STATUS.pending) ||
    isEqual(recipientGroups.loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  return (
    <table className='group-table'>
      <thead>
        <tr>
          <th className='column-group'>Group Name</th>
          <th className='column-recipient'>Recipient</th>
          <th className='column-action'>Action</th>
        </tr>
      </thead>
      <tbody>
        {groups.map((group: GroupDataSource, index: number) => (
          <tr key={index}>
            <td>{group.groupName}</td>
            <td><RecipientCell {...group} /> </td>
            <td><ActionCell {...group} /> </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default GroupTable;