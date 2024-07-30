import CustomInput from '../../../component/CustomInput';
import { TUserInfo } from '../../../type';
import store from '../../../app/useStoreNative';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

type Props = {
  dataSource: Array<TUserInfo>;
  rowIndex: number;
};

const PhoneCell = ({ dataSource, rowIndex }: Props) => {
  const { updateValue } = store.getState();
  const { userId, sectionName, phone } = dataSource[rowIndex];
  const updateInfo = (newValue: string) => {
    updateUserInfo({
      ColumnName: 'Phone',
      UserId: `${userId}`,
      Value: newValue
    });

    updateValue(newValue, 'phone', rowIndex);
  };

  return (
    <CustomInput
      value={phone}
      handleOnBlur={updateInfo}
      disabled={!hasPermissionUser(sectionName)} />
  );
};

export default PhoneCell;
