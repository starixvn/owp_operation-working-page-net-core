import { ChangeEvent } from 'react';
import { TUserInfo } from '../../../type';
import store from '../../../app/useStoreNative';
import SingleSelection from '../cellCommon/SingleSelection';
import { hasPermissionUser, updateUserInfo } from '../../../utils';

const LocationCell = (props: {
  dataSource: Array<TUserInfo>,
  rowIndex: number
}) => {
  const { options, updateValue } = store.getState();
  const { locationCodes } = options;
  const { rowIndex, dataSource } = props;
  const { locationCode, userId, sectionName, flagCode } = dataSource[rowIndex];

  const handleChange = (event: ChangeEvent<HTMLSelectElement>) => {
    const { value } = event.target;
    updateUserInfo({
      ColumnName: "LocationCode",
      UserId: `${userId}`,
      Value: value === 'None' ? '' : value
    });
    updateValue(value, 'locationCode', rowIndex);
  };

  return hasPermissionUser(sectionName)
    ? <SingleSelection
      defaultValue={locationCode ?? 'None'}
      onChange={handleChange}
      options={locationCodes}
    />
    : <div className='location-cell'>
      <a className="flag-item" href="#!">
        <i className={`flag-icon flag-icon-${flagCode}`} />
      </a>
      {locationCode ?? 'None'}
    </div>;
};

export default LocationCell;