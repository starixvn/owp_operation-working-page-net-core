import { useState } from 'react';
import { CellProps } from '../../../type';
import CustomInput from '../../../component/CustomInput';
import Toast from '../../../../components/ToastrWrapper';
import store from '../../../app/useStoreNative';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

const NickNameCell = ({ dataSource, rowIndex }: CellProps) => {
  const { updateValue } = store.getState();
  const [isError, setIsError] = useState(false);
  const { nickName, userId, sectionName } = dataSource[rowIndex];

  const updateInfo = async (newValue: string) => {
    if (/\s/.test(newValue)) {
      setIsError(true);
      return Toast.showError('Nickname cannot contain a space');
    }
    const response = await updateUserInfo({
      ColumnName: 'NickName',
      UserId: `${userId}`,
      Value: newValue
    });

    if (response.errCode) {
      setIsError(true);
      return Toast.showError(response.errMsg);
    } else {
      setIsError(false);
      return updateValue(newValue, 'nickName', rowIndex);
    }
  };

  return (
    <CustomInput
      value={nickName}
      handleOnBlur={updateInfo}
      isError={isError}
      disabled={!hasPermissionUser(sectionName)} />
  );
};

export default NickNameCell;
