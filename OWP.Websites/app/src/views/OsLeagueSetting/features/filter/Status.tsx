import { useEffect } from 'react';
import { Option } from '../../typing/type';
import MultipleSelection from '../shared/MultipleSelection';
import useStore from '../../app/store';
import { joinKey } from '../../Utils';
import { statusOptions, filter } from '../../osLeague.json';
import { shallow } from 'zustand/shallow';

const Status = () => {
  const { setFilter } = useStore(
    state => ({
      setFilter: state.setFilter
    }),
    shallow
  );

  useEffect(() => {
    const options: Option[] = [];
    statusOptions.forEach(option =>
      options.push({
        value: option.value,
        label: option.label
      })
    );
    setFilter({
      key: filter.status,
      value: joinKey(statusOptions)
    });
    /* eslint-disable no-warning-comments */
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const props = {
    label: 'Status',
    disabled: false,
    options: statusOptions,
    value: statusOptions,
    hasSelectAll: true,
    labelledBy: 'Select...',
    disableSearch: true,
    onChange: (selected: Option[]) =>
      setFilter({
        key: filter.status,
        value: joinKey(selected)
      })
  };

  return (
    <div className='filter-control status'>
      <MultipleSelection {...props} />
    </div>
  );
};

export default Status;
