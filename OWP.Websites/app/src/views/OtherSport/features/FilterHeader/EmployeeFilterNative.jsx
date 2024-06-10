// eslint-disable-next-line no-warning-comments
/* eslint-disable react-hooks/exhaustive-deps */
import { forwardRef, useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { AccountCheckIcon } from 'mdi-react';
import { LOCAL_STORAGE_KEY } from './FilterHeaderConst.json';

const propTypes = {
  defaultValue: PropTypes.string,
  onLoadEmployeeDone: PropTypes.func
};

const getUserCacheInfo = employee => {
  const userInfoString = localStorage.getItem(
    `${LOCAL_STORAGE_KEY}_${employee}`
  );
  return userInfoString ? JSON.parse(userInfoString) : {};
};

const EmployeeFilterNative = forwardRef((props, ref) => {
  const { defaultValue, onLoadEmployeeDone } = props;
  const userInfo = getUserCacheInfo(defaultValue);
  const { employee } = userInfo;
  const isCacheEmployee = typeof employee !== 'undefined';

  const [isDisableEmp, setIsDisableEmp] = useState(!isCacheEmployee);
  const onClickMyAssignee = () => {
    ref.current.value = !isDisableEmp ? defaultValue : '';
    setIsDisableEmp(!isDisableEmp);
  };

  const valueInput = isCacheEmployee ? employee : defaultValue;

  useEffect(() => {
    onLoadEmployeeDone(valueInput);
  }, []);

  return (
    <div className='filter-control employee'>
      <div className='text-box'>
        <div className='label'>Employee</div>
        <input
          disabled={isDisableEmp}
          ref={ref}
          type='text'
          name='username'
          className='input-field'
          maxLength='100'
          defaultValue={valueInput}
        />
      </div>
      <AccountCheckIcon
        className={`emp-icon${isDisableEmp ? '-activated' : ''}`}
        onClick={onClickMyAssignee}
      />
    </div>
  );
});

EmployeeFilterNative.propTypes = propTypes;
EmployeeFilterNative.displayName = 'EmployeeFilterNative';
export default EmployeeFilterNative;
