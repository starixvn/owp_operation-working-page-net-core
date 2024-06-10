import { useState } from 'react';
import Filter from './Filter';
import ReportTable from './Table/ReportTable';
import { useSelector } from 'react-redux';
import { reportSelector } from '../selector/reportSelector';
import Loading from '../../components/Loading';

const LAReportManagement = () => {
  const [columnSelected, setcolumnSelected] = useState([]);
  const model = useSelector(reportSelector);

  const props = {
    columnSelected,
    data: model.laInfo,
    orderType: model.orderType,
    columnId: model.columnId
  };

  return (
    <>
      <Filter onShowColumnChange={setcolumnSelected} />
      {model.pending && <Loading />}
      {model.isDisplayTable && <ReportTable {...props} />}
    </>
  );
};

export default LAReportManagement;
