import { ChangeEvent } from 'react';
import { TUserInfo } from '../../../type';
import store from '../../../app/useStoreNative';
import SingleSelection from '../cellCommon/SingleSelection';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

const secondAccountOption = [{
  value: 'true',
  label: 'Yes'
},
{
  value: 'false',
  label: 'No'
}];

const SecondAccountCell = (props: {
  dataSource: Array<TUserInfo>,
  rowIndex: number
}) => {
  const { updateValue } = store.getState();
  const { rowIndex, dataSource } = props;
  const user = dataSource[rowIndex];

  const handleChange = (event: ChangeEvent<HTMLSelectElement>) => {
    const { value } = event.target;

    updateUserInfo({
      ColumnName: "IsSecondAccount",
      UserId: `${user.userId}`,
      Value: value
    });

    updateValue(value, 'isSecondAccount', rowIndex);
  };

  return <SingleSelection
    defaultValue={user.isSecondAccount ?? undefined}
    onChange={handleChange}
    options={secondAccountOption}
    disabled={!hasPermissionUser(user.sectionName)}
  />;
};

export default SecondAccountCell;