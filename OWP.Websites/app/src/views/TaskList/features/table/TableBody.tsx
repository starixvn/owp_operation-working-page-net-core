import { TBody } from '../../type/TTaskListTable';
import RowContent from './RowContent';

const TableBody = (props: TBody) => {
  const {
    dataSources,
    hasDetailInformationPermission,
    firstMatchCheckedIsSaba
  } = props;

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
