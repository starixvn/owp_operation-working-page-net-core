import { ChangeEvent } from 'react';
import { TUserInfo } from '../../../type';
import store from '../../../app/useStoreNative';
import SingleSelection from '../cellCommon/SingleSelection';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

const RoleCell = (props: {
  dataSource: Array<TUserInfo>;
  rowIndex: number;
}) => {
  const { options, updateValue } = store.getState();
  const { roles } = options;
  const { rowIndex, dataSource } = props;
  const { roleId, userId, sectionName } = dataSource[rowIndex];

  const handleChange = (event: ChangeEvent<HTMLSelectElement>) => {
    const { value } = event.target;
    updateUserInfo({
      ColumnName: 'RoleId',
      UserId: `${userId}`,
      Value: value === '0' ? '' : value
    });

    updateValue(value, 'roleId', rowIndex);
  };

  return (
    <SingleSelection
      defaultValue={roleId ?? 0}
      onChange={handleChange}
      options={roles}
      disabled={!hasPermissionUser(sectionName)}
    />
  );
};

export default RoleCell;
