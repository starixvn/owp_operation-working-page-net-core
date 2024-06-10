import { TUserInfo } from '../../../type';
import store from '../../../app/useStoreNative';
import { hasPermissionUser, updateUserInfo } from '../../../utils';
import {
  SELECT_CTI_SUPERVISOR_OPTION,
  SELECT_CTI_STAFF_OPTION,
  SELECT_SMS_SUPERVISOR_OPTION,
  SELECT_SMS_STAFF_OPTION,
  SELECT_OTHER_OPTION
} from '../../../Const.json';
const GroupCell = (props: {
  dataSource: Array<TUserInfo>;
  rowIndex: number;
}) => {
  const { options, updateValue } = store.getState();
  const { userGroups } = options;
  const { rowIndex, dataSource } = props;
  const { groupId, userId, sectionName } = dataSource[rowIndex];

  const handleOnchangeOptions = (value: string) => {
    updateUserInfo({
      ColumnName: "GroupId",
      UserId: `${userId}`,
      Value: value !== '0' ? value : ''
    });
    updateValue(parseInt(value), 'groupId', rowIndex);
  };

  const renderOptions = (startIndex: number, endIndex: number) => {
    const dropdownOptions = [];
    for (let i = startIndex; i <= endIndex; i++) {
      dropdownOptions.push(
        <option
          key={userGroups[i]?.value}
          value={userGroups[i]?.value ?? undefined}>
          {userGroups[i]?.label}
        </option>
      );
    }
    return dropdownOptions;
  };
  const renderCTISupervisor = () => renderOptions(
    SELECT_CTI_SUPERVISOR_OPTION.startIndex, SELECT_CTI_SUPERVISOR_OPTION.endIndex
  );
  const renderCTIStaff = () => renderOptions(
    SELECT_CTI_STAFF_OPTION.startIndex, SELECT_CTI_STAFF_OPTION.endIndex);
  const renderSMSSupervisor = () => renderOptions(
    SELECT_SMS_SUPERVISOR_OPTION.startIndex, SELECT_SMS_SUPERVISOR_OPTION.endIndex);
  const renderSMSStaff = () => renderOptions(
    SELECT_SMS_STAFF_OPTION.startIndex, SELECT_SMS_STAFF_OPTION.endIndex);
  const renderOther = () => renderOptions(SELECT_OTHER_OPTION.startIndex, userGroups.length - 1);
  return (
    <div className='select-wrapper-default'>
      <select
        disabled={!hasPermissionUser(sectionName)}
        value={groupId ?? 0}
        onChange={e => handleOnchangeOptions(e.target.value)}
        className='browser-default'>
        <option value={0}></option>
        <optgroup label='CTI LA - Supervisor/Leader'>
          {renderCTISupervisor()}
        </optgroup>
        <optgroup label='CTI LA - Staff'>{renderCTIStaff()}</optgroup>
        <optgroup label='SMS LA - Supervisor/Leader'>
          {renderSMSSupervisor()}
        </optgroup>
        <optgroup label='SMS LA - Staff'>{renderSMSStaff()}</optgroup>
        <optgroup label='Others'>{renderOther()}</optgroup>
      </select>
    </div>
  );
};

export default GroupCell;
