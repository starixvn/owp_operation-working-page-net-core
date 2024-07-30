import { CellProps } from '../../../type';
import CustomInput from '../../../component/CustomInput';
import store from '../../../app/useStoreNative';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

const FullNameCell = ({ dataSource, rowIndex }: CellProps) => {
  const { updateValue } = store.getState();
  const { fullName, userId, sectionName } = dataSource[rowIndex];

  const updateInfo = (newValue: string) => {
    updateUserInfo({
      ColumnName: 'FullName',
      UserId: `${userId}`,
      Value: newValue
    });

    updateValue(newValue, 'fullName', rowIndex);
  };

  return <CustomInput value={fullName} handleOnBlur={updateInfo} disabled={!hasPermissionUser(sectionName)} />;
};

export default FullNameCell;
