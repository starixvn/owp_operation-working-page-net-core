import { updateFilterState } from './filterSlice';
import { useAppDispatch } from '../app/hooks';
import SearchEmployee from '../../Shared/SearchEmployee/SearchEmployee';
import { Option } from '../../Shared/SearchEmployee/type';

const UserSuggestionFilter = () => {
  const dispatch = useAppDispatch();
  const handleSelectedUserIds = (selected: Array<Option>) => {
    const userIds = selected.map(x => x.value).join();
    const userNames = selected.map(x => x.key).join();
    dispatch(
      updateFilterState({
        key: 'userIds',
        value: userIds
      })
    );
    dispatch(
      updateFilterState({
        key: 'userNames',
        value: userNames
      })
    );
  };
  return (
    <div className='filter-control filter-employee'>
      <SearchEmployee onChangeOptions={handleSelectedUserIds} />
    </div>
  );
};
export default UserSuggestionFilter;
