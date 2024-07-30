import CustomInput from '../../../component/CustomInput';
import { TUserInfo } from '../../../type';
import store from '../../../app/useStoreNative';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

type Props = {
  dataSource: Array<TUserInfo>;
  rowIndex: number;
};

const SkypeCell = ({ dataSource, rowIndex }: Props) => {
  const { updateValue } = store.getState();

  const updateInfo = (newValue: string) => {
    updateUserInfo({
      ColumnName: 'Skype',
      UserId: `${dataSource[rowIndex].userId}`,
      Value: newValue
    });

    updateValue(newValue, 'skype', rowIndex);
  };

  return (
    <CustomInput
      value={dataSource[rowIndex].skype}
      handleOnBlur={updateInfo}
      disabled={!hasPermissionUser(dataSource[rowIndex].sectionName)} />
  );
};

export default SkypeCell;
