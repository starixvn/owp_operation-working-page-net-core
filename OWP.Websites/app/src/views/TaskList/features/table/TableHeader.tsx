import { TABLE_HEADER } from '../const/TableSchema.json';

type colSpanType = {
  [key: string]: number;
};

const TableHeader = () => {
  const colSpanObject: colSpanType = {
    'FT Trader': 3,
    'Other Product Trader': 3,
    '1H Trader': 2,
    'default': 1
  };
  return (
    <tr className='header-table'>
      {TABLE_HEADER.map((header: string, index) => {
        const colSpan = colSpanObject[header] || colSpanObject.default;
        return (
          <th key={index} colSpan={colSpan} className={header}>
            {header}
          </th>
        );
      })}
    </tr>
  );
};

export default TableHeader;
