import {
  No,
  EmployeeID,
  DefaultTableStyle,
  Group,
  AccountName,
  FullName,
  NickName,
  Department,
  Section,
  Role,
  Extension,
  Phone,
  Email,
  Skype,
  Language,
  SecondAccount,
  Address,
  Pickup,
  Level,
  Location,
  Category
} from './TableSchema.json';
import { Column, ColumnSchema } from '../type';
import { dynamicCell } from './ConfigNative';
import { calculateCustomTableWidth } from '../../Shared/common/CustomTable/Helper';

const buildColumn = (width: number, column: Column) => {
  const { headerName, columnKey, isFreeze } = column;

  return {
    headerName,
    columnKey,
    width,
    isFreeze
  };
};

const selectTable = (widthWindow: number) => {
  const {
    minWidth,
    initialWidth,
    NoSize,
    GroupSize,
    EmployeeIdSize,
    FullNameSize,
    AccountNameSize,
    NickNameSize,
    DepartmentSize,
    SectionSize,
    RoleSize,
    ExtensionSize,
    PhoneSize,
    EmailSize,
    SkypeSize,
    CategorySize,
    LanguageSize,
    LocationSize,
    LevelSize,
    PickupSize,
    AddressSize,
    SecondAccountSize
  } = DefaultTableStyle;
  const width = widthWindow < minWidth ? minWidth : widthWindow;

  return {
    columnSchema: [
      buildColumn(width * NoSize, No),
      buildColumn(width * EmployeeIdSize, EmployeeID),
      buildColumn(width * AccountNameSize, AccountName),
      buildColumn(width * FullNameSize, FullName),
      buildColumn(width * NickNameSize, NickName),
      buildColumn(width * DepartmentSize, Department),
      buildColumn(width * SectionSize, Section),
      buildColumn(width * GroupSize, Group),
      buildColumn(width * RoleSize, Role),
      buildColumn(width * ExtensionSize, Extension),
      buildColumn(width * PhoneSize, Phone),
      buildColumn(width * EmailSize, Email),
      buildColumn(width * SkypeSize, Skype),
      buildColumn(width * CategorySize, Category),
      buildColumn(width * LocationSize, Location),
      buildColumn(width * LevelSize, Level),
      buildColumn(width * PickupSize, Pickup),
      buildColumn(width * AddressSize, Address),
      buildColumn(width * SecondAccountSize, SecondAccount),
      buildColumn(width * LanguageSize, Language)
    ],
    tableStyle: DefaultTableStyle,
    dynamicCell,
    tableWidth: (columnSchema: ColumnSchema) =>
      calculateCustomTableWidth(columnSchema, initialWidth)
  };
};

export { selectTable };
