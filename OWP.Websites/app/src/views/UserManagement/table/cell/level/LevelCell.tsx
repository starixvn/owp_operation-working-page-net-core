import { ChangeEvent } from 'react';
import { TUserInfo } from '../../../type';
import store from '../../../app/useStoreNative';
import SingleSelection from '../cellCommon/SingleSelection';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

const LevelCell = (props: {
  dataSource: Array<TUserInfo>,
  rowIndex: number
}) => {
  const { updateValue, selectOption } = store.getState();
  const user = props.dataSource[props.rowIndex];

  const options = selectOption(user.sectionId);
  const handleChange = (event: ChangeEvent<HTMLSelectElement>) => {
    const { value } = event.target;
    updateUserInfo({
      ColumnName: "Level",
      UserId: `${user.userId}`,
      Value: value === '0' ? '' : value
    });
    updateValue(value, 'level', props.rowIndex);
  };

  return <SingleSelection
    defaultValue={user.level ?? 0}
    onChange={handleChange}
    options={options}
    disabled={!hasPermissionUser(user.sectionName)}
  />;
};

export default LevelCell;