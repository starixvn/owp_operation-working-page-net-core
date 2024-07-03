import React, { useEffect, useState } from 'react';
import { Option, UserInfo } from './type';
import { MultiSelect } from 'react-multi-select-component';
import UserService from '../../Shared/User/UserService';

type SearchEmployeeProps = {
  onChangeOptions: (selected: Option[]) => void;
};

const SearchEmployee: React.FC<SearchEmployeeProps> = ({
  onChangeOptions
}: SearchEmployeeProps) => {
  const [value, setValue] = useState<Array<Option>>([]);
  const [options, setOptions] = useState<Array<Option>>([]);

  useEffect(() => {
    const handleSetOptions = async () => {
      const userFromCache = await UserService.getUserFromCached();
      const userOptions: Option[] =
        userFromCache &&
        userFromCache.map((x: UserInfo) => {
          const nickNamePart = x.NickName ? `(${x.NickName})` : '';
          const label = `${x.UserName} ${nickNamePart}`;
          return {
            value: x.UserId,
            key: x.UserName,
            label
          };
        });
      setOptions(userOptions);
    };
    handleSetOptions();
  }, []);

  const handleOnChange = (selectedOptions: Option[]) => {
    setValue(selectedOptions);
    onChangeOptions(selectedOptions);
  };

  return (
    <div className='multi-select-dropdown filter-league'>
      <div className='league'>
        <div className='label'>Employee</div>
        <MultiSelect
          onChange={(selectedOptions: Option[]) =>
            handleOnChange(selectedOptions)
          }
          value={value || []}
          options={options || []}
          ClearSelectedIcon={null}
          className='multi-select'
          hasSelectAll
          labelledBy='Username...'
          overrideStrings={{
            selectAll: '-- Select All --',
            allItemsAreSelected: 'All'
          }}
        />
      </div>
    </div>
  );
};

export default SearchEmployee;
