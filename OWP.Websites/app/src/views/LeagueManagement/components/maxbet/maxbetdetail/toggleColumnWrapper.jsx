import ToggleColumn from '../../../../Shared/ShowHideColumn/ToggleColumn';
import {
  buildLocalStoreKey,
  buildOptionDropdownToggleColumn
} from '../../../helper/toggleColumnHelper';
import { toggleColumn } from '../../../actions/action';

const toggleColumnWrapper = (store, dispatch) => {
  const { policyData } = store.getState().policyReducer;
  const localStoreKey = buildLocalStoreKey(policyData);
  const toggleOptions = buildOptionDropdownToggleColumn(policyData);
  const handleColumnSelected = select => {
    const columnKeys = select.map(item => item.value);
    dispatch(toggleColumn(columnKeys));
  };
  return (
    <div>
      <ToggleColumn
        onSelected={handleColumnSelected}
        localStorageKey={localStoreKey}
        options={toggleOptions}
      />
    </div>
  );
};

export default toggleColumnWrapper;
