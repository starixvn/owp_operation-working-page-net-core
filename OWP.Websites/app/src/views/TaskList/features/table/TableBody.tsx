import { TBody } from '../../type/TTaskListTable';
import RowContent from './RowContent';
import { useAppSelector } from '../../app/hooks';

const TableBody = (props: TBody) => {
  const { dataSources, hasDetailInformationPermission } = props;

  const firstMatchCheckedIsSaba = useAppSelector(
    state => state.taskList.matchesSelected[0]?.IsSaba
  );

  if (!dataSources.length) {
    return (
      <tr>
        <td colSpan={18}>There is no data.</td>
      </tr>
    );
  }

  return (
    <>
      {dataSources.map((dataSource, rowIndex) => (
        <RowContent
          key={`match-info-${rowIndex}-${dataSource.OWPMatchId}`}
          hasDetailPermission={hasDetailInformationPermission}
          dataSource={dataSource}
          rowIndex={rowIndex}
          firstMatchCheckedIsSaba={firstMatchCheckedIsSaba}></RowContent>
      ))}
    </>
  );
};

export default TableBody;
