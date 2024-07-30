import CustomInput from '../../../component/CustomInput';
import { CellProps } from '../../../type';
import store from '../../../app/useStoreNative';
import { hasPermissionUser, updateUserInfo } from '../../../utils';
const EmployeeIdCell = ({ dataSource, rowIndex }: CellProps) => {
  const { updateValue } = store.getState();
  const { employeeId, userId, sectionName } = dataSource[rowIndex];

  const updateInfo = (newValue: string) => {
    updateUserInfo({
      ColumnName: 'EmployeeId',
      UserId: `${userId}`,
      Value: newValue
    });

    updateValue(newValue, 'employeeId', rowIndex);
  };

  return <CustomInput
    value={employeeId}
    handleOnBlur={updateInfo}
    disabled={!hasPermissionUser(sectionName)} />;
};

export default EmployeeIdCell;
