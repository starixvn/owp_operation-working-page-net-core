import { DataSource } from '../type/TableType.native';
import TableBodyTotalGame from './TableBodyTotalGame';
import HistoryCell from '../components/HistoryCell';

const TWO = 2;
const TableBody = (props: {
  dataSource: Array<DataSource>;
  isGap: boolean;
}) => {
  return (
    <>
      {props.dataSource.map((item, index) => (
        <tr
          className={`${index % TWO === 0 ? 'row-even' : 'row-odd'}`}
          key={`${item.userName}-${index}`}>
          <td>{index + 1}</td>
          <td>{item.userName}</td>
          <td>{item.sectionName}</td>
          {props.isGap ? (
            <>
              <td
                className={`${item.isAlmostWinTarget ? 'almost-target' : ''} ${
                  item.progress === 'Fail' ? 'fail-progress' : ''
                }`}>
                {item.progress}
              </td>
              <td>{item.target}</td>
              <TableBodyTotalGame {...item.gapWithTarget} />
              <TableBodyTotalGame {...item.achievedGames} />
              <td>{item.reward}</td>
              <td>
                <HistoryCell userId={item.userId} userName={item.userName} />
              </td>
            </>
          ) : (
            <TableBodyTotalGame {...item.achievedGames} />
          )}
        </tr>
      ))}
    </>
  );
};

export default TableBody;
