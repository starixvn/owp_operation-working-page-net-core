import useRocketChat from '../../../Shared/customHook/useRocketChat';
import { TBody } from '../../type/TTaskListTable';
import RowContent from './RowContent';

const TableBody = (props: TBody) => {
  const {
    dataSources,
    hasDetailInformationPermission,
    firstMatchCheckedIsSaba
  } = props;
  const { authToken } = useRocketChat();
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
          firstMatchCheckedIsSaba={firstMatchCheckedIsSaba}
          authToken={authToken ?? ''}></RowContent>
      ))}
    </>
  );
};

export default TableBody;
