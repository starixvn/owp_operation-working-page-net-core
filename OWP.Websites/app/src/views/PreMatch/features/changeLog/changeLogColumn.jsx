const Columns = [
  {
    order: 1,
    fixed: 'left',
    name: 'League Name',
    id: 'LeagueName',
    headerStyle: { textAlign: 'left' }
  },
  {
    order: 2,
    name: 'Action',
    id: 'Action',
    width: 150,
    headerStyle: { textAlign: 'left' }
  },
  {
    order: 3,
    name: 'Old Value',
    width: 150,
    id: 'OldValue',
    headerStyle: { textAlign: 'left' }
  },
  {
    order: 4,
    name: 'New Value',
    id: 'NewValue',
    width: 150,
    headerStyle: { textAlign: 'left' }
  },
  {
    order: 5,
    name: 'Last Modified(GMT+8)',
    id: 'LastModifiedInString',
    width: 150,
    headerStyle: { textAlign: 'left' }
  },
  {
    order: 6,
    name: 'Modified by',
    id: 'ModifiedBy',
    width: 100,
    headerStyle: { textAlign: 'left' }
  }
];
const PageSizes = [10, 20, 50, 100, 200, 300, 200];
const DefaultPageSize = PageSizes[0];
const changeLogColumn = {
  Columns,
  PageSizes,
  DefaultPageSize
};

export default changeLogColumn;
