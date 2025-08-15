import { TABLE_HEADER } from '../TableSchema.json';
import FilterHeader from './FilterHeader';
import SortHeader from './SortHeader';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/storeNative';
import TradingMenuPopup from '../../../Shared/TradingMenuPopup';
import { DataSource } from '../../../TaskList/type/TTaskListTable';

const TableHeader = () => {
  const colSpanObject: { [key: string]: number } = {
    'FT Trader': 3,
    'Other Product Trader': 3,
    '1H Trader': 2,
    'default': 1
  };
  const model = useSelector((state: RootState) => state.events);
  const { matchesSelected } = model;
  const firstMatchCheckedIsSaba = matchesSelected[0]?.IsSaba;

  return (
    <tr>
      {TABLE_HEADER.map((item, index) => {
        const colSpan = colSpanObject[item.headerName] || colSpanObject.default;
        return (
          <th style={{ width: `${item.size}%` }} key={index} colSpan={colSpan}>
            <div className='header-table'>
              <span>{item.headerName}</span>
              {index === 0 && (
                <span style={{ marginLeft: '-20px', marginTop: '-4px' }}>
                  <TradingMenuPopup
                    firstMatchCheckedIsSaba={firstMatchCheckedIsSaba}
                    matchesSelected={matchesSelected as unknown as readonly DataSource[]}
                  />
                </span>
              )}
              {item?.isFilter && <FilterHeader columnKey={item.headerName} />}
              {item?.isSort && <SortHeader columnKey={item.headerName} />}
            </div>
          </th>
        );
      })}
    </tr>
  );
};

export default TableHeader;
