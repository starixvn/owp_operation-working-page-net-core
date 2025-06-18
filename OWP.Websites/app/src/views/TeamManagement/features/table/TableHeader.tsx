import { TABLE_HEADER } from './TableSchema.json';

const TableHeader = () => <thead>
  <tr>
    {TABLE_HEADER.map((item, index) => {
      return (
        <th
          key={index} style={{ width: item.WIDTH }}>
          <div className='header-table'>
            <span>{item.HEADER_NAME}</span>
          </div>
        </th>
      );
    })}
  </tr>
</thead>;

export default TableHeader;