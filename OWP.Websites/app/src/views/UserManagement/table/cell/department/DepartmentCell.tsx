import { ChangeEvent } from 'react';
import { CellProps } from '../../../type';
import store from '../../../app/useStoreNative';
import SingleSelection from '../cellCommon/SingleSelection';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

const DepartmentCell = ({ dataSource, rowIndex }: CellProps) => {
  const { options, updateValue } = store.getState();
  const { departments } = options;
  const { userId, sectionName, departmentId } = dataSource[rowIndex];

  const handleChange = (event: ChangeEvent<HTMLSelectElement>) => {
    const { value } = event.target;
    updateUserInfo({
      ColumnName: 'DepartmentId',
      UserId: `${userId}`,
      Value: value === '0' ? '' : value
    });

    updateValue(value, 'departmentId', rowIndex);
  };

  return (
    <SingleSelection
      defaultValue={departmentId ?? 0}
      onChange={handleChange}
      options={departments}
      disabled={!hasPermissionUser(sectionName)}
    />
  );
};

export default DepartmentCell;
