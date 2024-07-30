import { Table } from 'fixed-data-table-2';
import MaxbetConst from '../../../constants/MaxbetConst';
import renderTableBody from './renderTableBody';
import { useSelector } from 'react-redux';
import { selectMaxbetGeneralData } from '../../../selector/SelectorMaxbetGeneral';
import Loading from '../../../../components/Loading';
import { withResizeDetector } from 'react-resize-detector';

const {
  rowHeight,
  groupHeaderHeight,
  maxHeight,
  headerHeight,
  defaultHeight
} = MaxbetConst.tableMaxbetGeneralInfo;

const calculateTableHeight = dataLength => {
  return dataLength > 27
    ? maxHeight
    : groupHeaderHeight + rowHeight + dataLength * rowHeight + 30;
};

const TableMaxbetGeneral = () => {
  const { maxbetGeneralPending, maxbetGeneralData, filterInfo } = useSelector(
    selectMaxbetGeneralData
  );
  const dataLength = maxbetGeneralData.length;
  if (maxbetGeneralPending) {
    return <Loading />;
  }

  const widthTable = window.innerWidth > 680 ? window.innerWidth - 110 : window.innerWidth;

  return (
    <div className='table'>
      <Table
        groupHeaderHeight={groupHeaderHeight}
        rowHeight={rowHeight}
        rowsCount={dataLength}
        width={widthTable}
        height={
          rowHeight && dataLength > 3
            ? calculateTableHeight(dataLength)
            : defaultHeight
        }
        maxHeight={maxHeight}
        headerHeight={headerHeight}>
        {renderTableBody(maxbetGeneralData, filterInfo)}
      </Table>
      <div className='total'>Total records:{dataLength}</div>
    </div>
  );
};

export default withResizeDetector(TableMaxbetGeneral);
