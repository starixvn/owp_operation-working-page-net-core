import CustomInput from '../../../component/CustomInput';
import { TUserInfo } from '../../../type';
import store from '../../../app/useStoreNative';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

type Props = {
  dataSource: Array<TUserInfo>;
  rowIndex: number;
};

const ExtensionCell = ({ dataSource, rowIndex }: Props) => {
  const { updateValue } = store.getState();
  const { userId, extension, sectionName } = dataSource[rowIndex];
  const updateInfo = (newValue: string) => {
    updateUserInfo({
      ColumnName: 'Extension',
      UserId: `${userId}`,
      Value: newValue
    });

    updateValue(newValue, 'extension', rowIndex);
  };

  return (
    <CustomInput
      value={extension}
      handleOnBlur={updateInfo}
      disabled={!hasPermissionUser(sectionName)}
    />
  );
};

export default ExtensionCell;
