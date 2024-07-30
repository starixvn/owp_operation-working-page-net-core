import { ChangeEvent } from 'react';
import { TUserInfo } from '../../../type';
import store from '../../../app/useStoreNative';
import SingleSelection from '../cellCommon/SingleSelection';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

const pickupOption = [{
  value: 'true',
  label: 'Yes'
},
{
  value: 'false',
  label: 'No'
}, {
  value: 'none',
  label: ''
}];

const PickupCell = (props: {
  dataSource: Array<TUserInfo>,
  rowIndex: number
}) => {
  const { updateValue } = store.getState();
  const { rowIndex, dataSource } = props;
  const user = dataSource[rowIndex];


  const handleChange = (event: ChangeEvent<HTMLSelectElement>) => {
    const { value } = event.target;

    updateUserInfo({
      ColumnName: "Pickup",
      UserId: `${user.userId}`,
      Value: value !== 'none' ? value : ''
    });

    updateValue(value, 'pickup', rowIndex);
  };

  return <SingleSelection
    defaultValue={user.pickup ?? undefined}
    onChange={handleChange}
    options={pickupOption}
    disabled={!hasPermissionUser(user.sectionName)}
  />;
};

export default PickupCell;