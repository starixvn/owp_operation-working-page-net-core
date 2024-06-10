import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import store, { RootState } from '../app/store';
import Command from './Command';
import DateTimePicker from './DateTimePicker';
import FilterMatch from './FilterMatch';
import MatchCategory from './FilterMatchCategory';
import HasOtherProvider from './HasOtherProvider';
import HasProvider from './HasProvider';
import League from './League';
import SelectGame from './SelectGame';
import UpdateType from './UpdateType';
import { getLeagues, getUpdateTypes } from './slice/filterThunkNative';
import Utils from '../../Shared/Utils';
import TurnoverMargin from './TurnoverMargin';
import { VIEW } from '../mode/mode.json';

const Filter = () => {
  const dispatch = useDispatch();
  const { date } = useSelector((state: RootState) => state.filter.param);
  const {
    turnOverMarginPolicy: {
      permission: { HasRead }
    },
    mode: { mode }
  } = useSelector((state: RootState) => state);

  useEffect(() => {
    dispatch(getLeagues(Utils.formatDate(date)));
  }, [date, dispatch]);

  useEffect(() => {
    dispatch(getUpdateTypes());
  }, [dispatch]);

  return (
    <>
      <DateTimePicker />
      <League />
      <SelectGame />
      <FilterMatch />
      <HasProvider />
      <HasOtherProvider />
      <MatchCategory />
      {HasRead && <TurnoverMargin />}
      <UpdateType />
      {mode === VIEW && <Command />}
    </>
  );
};
export default Filter;
