import { memo } from 'react';
import { useSelector } from 'react-redux';
import Loading from '../../../components/Loading';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { RootState } from '../../app/storeNative';
import { exportReportByMonthRoute } from '../../Const.json';
import Filter from '../filter/Filter';
import { buildRequestURL } from '../helpers/helpers';
import { FilterModel } from '../typeNative';
import { getReportByMonth } from './Helper/byMonthSlice';
import ReportTable from './Table/ReportTable';

const MonthTabContent = memo(() => {
  const model = useSelector((state: RootState) => state.monthOTReducer);
  const dispatch = useStableDispatch();
  const { data, isFulfill, isLoading } = model;

  const handleSearch = (filterModel: FilterModel) => {
    dispatch(getReportByMonth(filterModel));
  };

  const handleExport = (filterModel: FilterModel) => {
    const { trader, location, year, month } = filterModel;
    const url = buildRequestURL(exportReportByMonthRoute);

    window.location.href = `${url}?Location=${location}&Trader=${trader}&Month=${month}&Year=${year}`;
  };

  return (
    <>
      <div className='filter'>
        <Filter
          isByMonthTab
          handleExport={handleExport}
          handleSearch={handleSearch}
        />
      </div>
      {isLoading && <Loading />}
      {!isLoading && isFulfill &&
        <ReportTable data={data} />}
    </>
  );
});

MonthTabContent.displayName = 'MonthTabContent';
export default MonthTabContent;
