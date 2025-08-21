import { useCallback } from 'react';
import MultipleSelection from '../../../../Shared/MultipleSelection/MultipleSelection';
import { IMultipleSelectProps, Option } from '../../../../Shared/MultipleSelection/type';
import { setLeagueHeaderFiler } from '../table/leagueInfoTableSlice/reducer';
import { useAppDispatch, useAppSelector } from '../app/hooks';

interface MultipleSelectionTemplateProps extends IMultipleSelectProps {
  filterKey: string;
}

const MultipleSelectionTemplate = (props: MultipleSelectionTemplateProps) => {
  const dispatch = useAppDispatch();
  const { leagueFilterHeader } = useAppSelector(state => state.leagueInfoTableSlice);
  const handleChange = (event: Option[]) => {
    dispatch(setLeagueHeaderFiler({
      ...leagueFilterHeader,
      [props.filterKey]: event.map(opt => opt.value)
    }));
  };

  return (
    <MultipleSelection
      className={props.className}
      label={props.label}
      options={props.options}
      value={props.value}
      disabled={props.disabled}
      disableSearch={props.disableSearch}
      onChange={handleChange} />
  );
};

export default MultipleSelectionTemplate;