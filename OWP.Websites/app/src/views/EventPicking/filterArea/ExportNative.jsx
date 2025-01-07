import DropdownNative from '../../Shared/common/Dropdown/DropdownNative';
import store from '../app/store';
import { EXPORT } from './filterConst.json';
import { exportAndConfirmAllEvent, exportOnly } from '../helper/exportHelper';
import { exportExcel } from '../filterArea/slice/ExportThunkNative';

const ExportNative = () => {
  const { permission } = store.getState().pagePolicy;

  const items = [];

  if (permission.HasWrite) {
    items.push({
      title: EXPORT.EXPORT_AND_CHECK_ALL,
      handler: () => exportAndConfirmAllEvent(exportExcel)
    });
  }

  items.push({
    title: EXPORT.EXPORT_ONLY,
    handler: () => exportOnly(exportExcel)
  });

  return <DropdownNative items={items} />;
};

export default ExportNative;
