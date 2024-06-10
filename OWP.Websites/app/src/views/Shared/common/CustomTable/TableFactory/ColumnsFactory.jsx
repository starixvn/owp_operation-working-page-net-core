import createColumn from './createColumn';

const ColumnsFactory = props => {
  const { columnSchema, ...rest } = props;
  const columns = [];
  columnSchema.forEach(columnModel => {
    const columnInfo = {
      columnModel,
      ...rest
    };
    columns.push(createColumn(columnInfo));
  });
  return columns;
};
export default ColumnsFactory;
