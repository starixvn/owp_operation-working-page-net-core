const Columns = [
  {
    order: 1,
    fixed: 'right',
    name: 'Shifts',
    id: 'Shifts',
    width: 100,
    headerStyle: { textAlign: 'center' }
  },
  {
    order: 2,
    name: 'Usernames',
    id: 'Usernames',
    headerStyle: { textAlign: 'center' }
  }
];
const PageSizes = [10, 20, 50, 100, 200, 300, 200];
const DefaultPageSize = PageSizes[0];
const workingShiftColumn = {
  Columns,
  PageSizes,
  DefaultPageSize
};

export default workingShiftColumn;
