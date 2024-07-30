import {
  NoCell,
  EmployeeIdCell,
  RoleCell,
  CategoryCell,
  AccountNameCell,
  FullNameCell,
  NickNameCell,
  DepartmentCell,
  SectionCell,
  GroupCell,
  ExtensionCell,
  PhoneCell,
  EmailCell,
  SkypeCell
} from './cell';
import {
  No,
  EmployeeID,
  AccountName,
  FullName,
  NickName,
  Department,
  Section,
  Language,
  Location,
  Level,
  Pickup,
  SecondAccount,
  Role,
  Category,
  Address,
  Group,
  Extension,
  Phone,
  Email,
  Skype
} from './TableSchema.json';
import LanguageCell from './cell/language/LanguageCell';
import LocationCell from './cell/location/LocationCell';
import { TUserInfo } from '../type';
import LevelCell from './cell/level/LevelCell';
import PickupCell from './cell/pickup/PickupCell';
import SecondAccountCell from './cell/secondAccount/SecondAccountCell';
import AddressCell from './cell/address/AddressCell';

const dynamicCell = (
  rowIndex: number,
  cellModel: {
    dataSource: Array<TUserInfo>;
    columnKey: string;
    onShowDropdownListOption: (isOpen: boolean) => void;
  }
) => {
  const { columnKey, dataSource, onShowDropdownListOption } = cellModel;
  switch (columnKey) {
    case No.columnKey:
      return <NoCell rowIndex={rowIndex} />;
    case EmployeeID.columnKey:
      return <EmployeeIdCell rowIndex={rowIndex} dataSource={dataSource} />;
    case AccountName.columnKey:
      return <AccountNameCell rowIndex={rowIndex} dataSource={dataSource} />;
    case FullName.columnKey:
      return <FullNameCell rowIndex={rowIndex} dataSource={dataSource} />;
    case NickName.columnKey:
      return <NickNameCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Department.columnKey:
      return <DepartmentCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Section.columnKey:
      return <SectionCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Group.columnKey:
      return <GroupCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Language.columnKey:
      return (
        <LanguageCell
          rowIndex={rowIndex}
          handleShowDropdown={onShowDropdownListOption}
          dataSource={dataSource}
        />
      );
    case Role.columnKey:
      return <RoleCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Extension.columnKey:
      return <ExtensionCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Phone.columnKey:
      return <PhoneCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Email.columnKey:
      return <EmailCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Skype.columnKey:
      return <SkypeCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Category.columnKey:
      return <CategoryCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Location.columnKey:
      return <LocationCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Level.columnKey:
      return <LevelCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Pickup.columnKey:
      return <PickupCell rowIndex={rowIndex} dataSource={dataSource} />;
    case SecondAccount.columnKey:
      return <SecondAccountCell rowIndex={rowIndex} dataSource={dataSource} />;
    case Address.columnKey:
      return <AddressCell rowIndex={rowIndex} dataSource={dataSource} />;
  }
};

export { dynamicCell };
