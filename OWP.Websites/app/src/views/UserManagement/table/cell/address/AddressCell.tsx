import { TUserInfo } from '../../../type';
import CustomInput from '../../../component/CustomInput';
import store from '../../../app/useStoreNative';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

const AddressCell = (props: {
  dataSource: Array<TUserInfo>;
  rowIndex: number;
}) => {
  const { dataSource, rowIndex } = props;
  const { updateValue } = store.getState();

  const user = dataSource[rowIndex];

  const updateInfo = (newValue: string) => {
    updateUserInfo({
      ColumnName: 'Address',
      UserId: `${user.userId}`,
      Value: newValue
    });

    updateValue(newValue, 'address', rowIndex);
  };
  return <CustomInput
    value={user.address}
    handleOnBlur={updateInfo}
    disabled={!hasPermissionUser(user.sectionName)} />;

};

export default AddressCell;