import { useEffect } from 'react';
import Loading from '../../../components/Loading';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import { Option } from '../../typing/type';
import MultipleSelection from '../shared/MultipleSelection';
import { simpleApiKey, localStoreKey, filter, optionValueNone } from '../../osLeague.json';
import { joinKey } from '../../Utils';
import useStore from '../../app/store';
import { shallow } from 'zustand/shallow';

const { roundOption } = localStoreKey;
const { round } = simpleApiKey;

const Round = () => {
  const { loading, options } = useQueryDropdownOption({
    apiKey: round,
    localStorageKey: roundOption
  });

  const { setFilter } = useStore(
    state => ({
      setFilter: state.setFilter
    }),
    shallow
  );

  useEffect(() => {
    if (options) {
      setFilter({
        key: filter.round,
        value: joinKey(options)
      });
    }
  }, [options, setFilter]);

  if (loading) {
    return <Loading />;
  }

  options.unshift({ label: 'None', value: optionValueNone });

  const props = {
    label: 'Round',
    options,
    value: options,
    disabled: false,
    onChange: (selected: Option[]) =>
      setFilter({
        key: filter.round,
        value: joinKey(selected)
      })
  };

  return (
    <div className='filter-control round'>
      <MultipleSelection {...props} />
    </div>
  );
};

export default Round;
