type ColumnOrder = {
  columnId: string;
  subColumnId: string;
  orderType: 'asc' | 'desc';
};

type ColumnProp = {
  name: string;
  id: string;
};

export { ColumnOrder, ColumnProp };
