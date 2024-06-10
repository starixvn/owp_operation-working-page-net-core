const Columns = [
  {
    order: 1,
    fixed: 'left',
    name: 'No.',
    id: 'PreMatchLeagueId',
    width: 50,
    headerStyle: { textAlign: 'left' }
  },
  {
    order: 2,
    name: 'League Name',
    id: 'LeagueName',
    headerStyle: { textAlign: 'left' }
  },
  {
    order: 3,
    name: 'Deleted By',
    id: 'DeletedBy',
    width: 180,
    headerStyle: { textAlign: 'left' }
  },
  {
    order: 4,
    name: 'Deleted Time(GMT+8)',
    id: 'DeletedTime',
    width: 220,
    headerStyle: { textAlign: 'left' }
  },
  {
    order: 5,
    name: 'Recover',
    id: 'Recover',
    width: 70,
    headerStyle: { textAlign: 'center' }
  }
];

const PageSizes = [10, 20, 50, 100, 200, 300, 200];
const DefaultPageSize = PageSizes[0];
const DeletedLeagueColumn = {
  Columns,
  PageSizes,
  DefaultPageSize
};

export default DeletedLeagueColumn;
