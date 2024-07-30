import {
  keyOfTotalGameGap,
  keyOfTotalAchievedMatches
} from '../../TableConst.json';
import { uniqueId } from 'lodash';
import FilterIcon from 'mdi-react/FilterIcon';
import SortHeader from '../components/SortHeader';

type PropsTableHeader = {
  isGap: boolean;
  isActiveIconFilter: boolean;
  openFilterHeader: () => void;
};

const TableHeader = (props: PropsTableHeader) => {
  const headerTotalGameList = props.isGap
    ? [...keyOfTotalGameGap, ...keyOfTotalAchievedMatches]
    : keyOfTotalAchievedMatches;

  return (
    <>
      <tr>
        <th rowSpan={2}>#</th>
        <th className='username' rowSpan={2}>
          <span>Username</span>
          <span
            className={`icon-filter ${
              props.isActiveIconFilter ? 'active' : ''
            }`}
            onClick={props.openFilterHeader}>
            <FilterIcon size={14} />
          </span>
        </th>
        <th rowSpan={2} className='section-header'>
          <span>Section</span>
        </th>
        {props.isGap ? (
          <>
            <th rowSpan={2}>
              <SortHeader
                columnKey={'progress'}
                headerName={'Progress'}
                property={'progress'}
              />
            </th>
            <th rowSpan={2}>
              <SortHeader
                columnKey={'target'}
                headerName={'Target'}
                property={'target'}
              />
            </th>
            <th className='gap-matches' colSpan={5}>
              The gap to win the average of two conditions
            </th>
            <th colSpan={5} className='achieved-matches'>
              Achieved matches
            </th>
            <th rowSpan={2} className='reward-column'>
              Reward
            </th>
            <th rowSpan={2} className='history-column'>
              History
            </th>
          </>
        ) : (
          <th colSpan={5}>Achieved matches</th>
        )}
      </tr>
      <tr>
        {headerTotalGameList.map(x => (
          <th key={uniqueId()}>
            <SortHeader
              columnKey={x.key}
              headerName={x.label}
              property={x.property}
            />
          </th>
        ))}
      </tr>
    </>
  );
};

export default TableHeader;
