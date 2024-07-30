import { CellProps } from '../../../type';
import { UserIcon } from 'mdi-react';

const AccountNameCell = ({ dataSource, rowIndex }: CellProps) => {
  return (
    <div className='account-name-cell'>
      <UserIcon className='account-name-icon'></UserIcon>
      {dataSource[rowIndex]?.userName}
    </div>
  );
};

export default AccountNameCell;
