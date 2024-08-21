import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { RootState } from '../app/store';
import { VIEW } from '../mode/mode.json';
import Command from './Command';
import DateTimePicker from './DateTimePicker';
import FilterMatch from './FilterMatch';
import MatchCategory from './FilterMatchCategory';
import HasOtherProvider from './HasOtherProvider';
import HasProvider from './HasProvider';
import SelectGame from './SelectGame';
import { getUpdateTypes } from './slice/filterThunkNative';
import TurnoverMargin from './TurnoverMargin';
import UpdateType from './UpdateType';

const Filter = () => {
  const dispatch = useDispatch();
  const {
    turnOverMarginPolicy: {
      permission: { HasRead }
    },
    mode: { mode }
  } = useSelector((state: RootState) => state);

  useEffect(() => {
    dispatch(getUpdateTypes());
  }, [dispatch]);

  return (
    <>
      <DateTimePicker />
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
