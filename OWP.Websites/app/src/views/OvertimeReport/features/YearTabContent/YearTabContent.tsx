import { memo } from 'react';
import { useSelector } from 'react-redux';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { RootState } from '../../app/storeNative';
import { exportReportByYearRoute } from '../../Const.json';
import Filter from '../filter/Filter';
import { buildRequestURL } from '../helpers/helpers';
import { getReportByYear } from '../overTimeReportThunkNative';
import { FilterModel } from '../typeNative';
import ReportTable from './table/ReportTable';

const YearTabContent = memo(() => {
  const dispatch = useStableDispatch();
  const tabStore = useSelector((state: RootState) => state.yearTab);

  const handleSearch = (filterModel: FilterModel) => {
    const { trader, location, year } = filterModel;
    const postModel = {
      trader,
      location,
      year
    };

    dispatch(getReportByYear(postModel));
  };

  const handleExport = (filterModel: FilterModel) => {
    const { trader, location, year } = filterModel;
    const url = buildRequestURL(exportReportByYearRoute);

    window.location.href = `${url}?Location=${location}&Trader=${trader}&Year=${year}`;
  };

  return (
    <>
      {tabStore.status === REQUEST_STATUS.pending && <Loading />}
      <div className='filter'>
        <Filter
          isByMonthTab={false}
          handleExport={handleExport}
          handleSearch={handleSearch}
        />
      </div>
      <div className='report-content'>
        <ReportTable data={tabStore.overtimeData} />
      </div>
    </>
  );
});

YearTabContent.displayName = 'YearTabContent';
export default YearTabContent;
