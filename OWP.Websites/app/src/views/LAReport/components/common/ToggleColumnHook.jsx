// eslint-disable-next-line no-warning-comments
/* eslint-disable react-hooks/exhaustive-deps */
import { useState, useEffect, useCallback } from 'react';
import { MultiSelect } from 'react-multi-select-component';
import PropTypes from 'prop-types';
import Utils from '../../../Shared/Utils';
import md5 from 'md5';
import { isEqual } from 'lodash';
const propTypes = {
  disableSearch: PropTypes.bool,
  options: PropTypes.array.isRequired,
  localStorageKey: PropTypes.string,
  onSelected: PropTypes.func.isRequired
};

const ToggleColumnHook = ({
  options,
  localStorageKey,
  onSelected,
  disableSearch = true
}) => {
  const [selected, setSelected] = useState([]);
  const localStorageKeyMd5 = md5(localStorageKey);
  const statableOnSelected = useCallback(onSelected, []);
  const handleSelectedChanged = e => {
    setSelected(e);
    const selectedColumns = options.filter(option =>
      e.some(item => isEqual(item.value, option.value))
    );
    localStorage.setItem(localStorageKeyMd5, JSON.stringify(selectedColumns));
    onSelected(selectedColumns);
  };

  useEffect(() => {
    const selectedColumns = Utils.convertJsonToObject(
      Utils.getDataFromLocalStore(localStorageKeyMd5)
    );
    const columnsValid = Array.isArray(selectedColumns)
      ? selectedColumns
      : options;
    setSelected(columnsValid);
    statableOnSelected(columnsValid);
  }, [statableOnSelected, localStorageKeyMd5]);

  return (
    <MultiSelect
      options={options}
      onChange={handleSelectedChanged}
      value={selected}
      disableSearch={disableSearch}
      ClearSelectedIcon={null}
    />
  );
};

ToggleColumnHook.propTypes = propTypes;

export default ToggleColumnHook;
