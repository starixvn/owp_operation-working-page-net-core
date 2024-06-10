import { createRef, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { RootState } from '../../app/storeNative';
import { locationOptionAll } from '../../Const.json';
import { getLocationOptions } from '../overTimeReportThunkNative';
import { LocationOption, SelectOption } from '../typeNative';
import Location from './filterField/LocationNative';
import Month from './filterField/MonthNative';
import Trader from './filterField/TraderNative';
import Year from './filterField/Year';

type PropsType = {
  isByMonthTab: boolean;
  handleSearch: Function;
  handleExport: Function;
};

const Filter = (props: PropsType) => {
  const dispatch = useStableDispatch();
  const date = new Date();
  const [selectedLocation, setLocation] = useState<LocationOption>(locationOptionAll);
  const [selectedMonth, setMonth] = useState<number>(date.getMonth() + 1);
  const [selectedYear, setYear] = useState<number>(date.getFullYear());
  const traderRef = createRef<HTMLInputElement | HTMLTextAreaElement>();
  const store = useSelector((state: RootState) => state.filter);

  useEffect(() => {
    dispatch(getLocationOptions());
    handleSearch();
  }, []);

  const handleChangeLocation = (event: LocationOption) => setLocation(event);
  const handleChangeMonth = (event: SelectOption) => setMonth(event.value);
  const handleChangeYear = (event: SelectOption) => setYear(event.value);

  const handleSearch = () =>
    props.handleSearch({
      trader: traderRef.current?.value,
      location: selectedLocation.value,
      month: selectedMonth,
      year: selectedYear
    });

  const handleExport = () =>
    props.handleExport({
      trader: traderRef.current?.value,
      location: selectedLocation.value,
      month: selectedMonth,
      year: selectedYear
    });

  return (
    <>
      {store.status === REQUEST_STATUS.pending && <Loading />}
      <Trader ref={traderRef} />
      <Location
        handleChange={handleChangeLocation}
        selectedLocation={selectedLocation}
      />
      {props.isByMonthTab && <Month handleChange={handleChangeMonth} />}
      <Year handleChange={handleChangeYear} />
      <button className='btn btn-search' onClick={handleSearch}>
        Search
      </button>
      <button className='btn btn-excel' onClick={handleExport}>
        Export
      </button>
    </>
  );
};

export default Filter;
