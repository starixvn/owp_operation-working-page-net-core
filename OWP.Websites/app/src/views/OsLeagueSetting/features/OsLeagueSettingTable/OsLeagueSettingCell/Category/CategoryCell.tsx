import { CellProps } from '../../../../typing/tableType';

const CategoryCell = (props: CellProps): JSX.Element => {
  const { dataSource, rowIndex } = props;
  const { categoryName } = dataSource[rowIndex];
  return (
    <div className='category-cell'>
      <span>{categoryName}</span>
    </div>
  );
};

export default CategoryCell;
