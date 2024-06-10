import DropdownNative from '../../Shared/common/Dropdown/DropdownNative';
import { EXPORT } from './filterConst.json';
import { exportAndConfirmAllEvent, exportOnly } from '../helper/exportHelper';
import {
  exportExcel,
  exportLSTExcel
} from '../filterArea/slice/ExportThunkNative';

const ExportNative = () => {
  const items = [
    {
      title: EXPORT.EXPORT_AND_CHECK_ALL,
      handler: () => exportAndConfirmAllEvent(exportExcel)
    },
    {
      title: EXPORT.EXPORT_ONLY,
      handler: () => exportOnly(exportExcel)
    },
    {
      title: EXPORT.EXPORT_LST_FORMAT_CHECK_ALL,
      handler: () => exportAndConfirmAllEvent(exportLSTExcel)
    },
    {
      title: EXPORT.EXPORT_LST_FORMAT,
      handler: () => exportOnly(exportLSTExcel)
    }
  ];
  return <DropdownNative items={items} />;
};

export default ExportNative;
