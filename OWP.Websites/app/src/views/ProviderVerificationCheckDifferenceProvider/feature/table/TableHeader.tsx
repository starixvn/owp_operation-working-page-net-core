import { TColumnKey } from '../../type/table';
import MultipleFilterHeader from './MultipleFilterHeader';
import { TABLE_HEADER } from './TableSchema.json';

const TableHeader = () => <thead>
  <tr>
    {TABLE_HEADER.map((item, index) => {
      return (
        <th
          key={index} style={{ width: item.WIDTH }}>
          <div className='header-table'>
            <span>{item.HEADER_NAME}</span>
            {item.IS_FILTER && <MultipleFilterHeader columnKey={item.HEADER_KEY as TColumnKey} />}
          </div>
        </th>
      );
    })}
  </tr>
</thead>;

export default TableHeader;