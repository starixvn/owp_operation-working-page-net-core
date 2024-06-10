import { useEffect } from 'react';
import Loading from '../../../components/Loading';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import { Option } from '../../typing/type';
import MultipleSelection from '../shared/MultipleSelection';
import useStore from '../../app/store';
import { joinKey } from '../../Utils';
import { filter, localStoreKey, simpleApiKey, optionValueNone } from '../../osLeague.json';

const { category } = simpleApiKey;
const { categoryOption } = localStoreKey;

const Category = () => {
  const { loading, options } = useQueryDropdownOption({
    apiKey: category,
    localStorageKey: categoryOption
  });

  const setFilter = useStore(state => state.setFilter);
  useEffect(() => {
    if (options.length) {
      setFilter({
        key: filter.category,
        value: joinKey(options)
      });
    }
  }, [options, setFilter]);

  if (loading) {
    return <Loading />;
  }

  options.unshift({ label: 'None', value: optionValueNone });

  const props = {
    label: 'Category',
    options,
    value: options,
    hasSelectAll: true,
    labelledBy: 'Select...',
    disableSearch: true,
    disabled: false,
    onChange: (selected: Option[]) =>
      setFilter({
        key: filter.category,
        value: joinKey(selected)
      })
  };

  return (
    <div className='filter-control category'>
      <MultipleSelection {...props} />
    </div>
  );
};

export default Category;
