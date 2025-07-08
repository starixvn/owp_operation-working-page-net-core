import { TABLE_HEADER } from '../TableSchema.json';
import FilterHeader from './FilterHeader';
import SortHeader from './SortHeader';
import FilterTrader from './FilterTrader';

const TableHeader = () => {
  const colSpanObject: { [key: string]: number } = {
    'FT Trader': 3,
    'Other Product Trader': 3,
    '1H Trader': 2,
    'default': 1
  };

  return (
    <tr>
      {TABLE_HEADER.map((item, index) => {
        const colSpan = colSpanObject[item.headerName] || colSpanObject.default;
        return (
          <th style={{ width: `${item.size}%` }} key={index} colSpan={colSpan}>
            {index === 0 ? (
              <FilterTrader
                columnKey={item.headerName}
                name={item.headerName}
                title={item.headerName}
              />
            ) : (
              <div className='header-table'>
                <span>{item.headerName}</span>
                {item?.isFilter && <FilterHeader columnKey={item.headerName} />}
                {item?.isSort && <SortHeader columnKey={item.headerName} />}
              </div>
            )}
          </th>
        );
      })}
    </tr>
  );
};

export default TableHeader;
