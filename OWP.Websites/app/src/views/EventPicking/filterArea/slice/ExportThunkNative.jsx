import {
  exportExcelUrl,
  exportExcelLSTUrl,
  queryDataExcel
} from '../../UtilsNative';
import { createAsyncThunk } from '@reduxjs/toolkit';
import dayjs from 'dayjs';

const exportExcel = createAsyncThunk(
  'EventPicking/export',
  async modelFilter => {
    const { data } = await queryDataExcel(exportExcelUrl, modelFilter);
    const currentDateGmt8 = new Date().toLocaleString('en-US', {
      timeZone: 'Asia/Hong_Kong'
    });

    if (data) {
      const urlFake = window.URL.createObjectURL(new Blob([data]));
      const link = document.createElement('a');
      link.href = urlFake;
      link.setAttribute(
        'download',
        `Miscmatch v2.0 - ${dayjs(modelFilter.date).format(
          'YYYYMMDD'
        )} - Export time ${dayjs(currentDateGmt8).format(
          'YYYYMMDD hh:mm A'
        )}.xlsx`
      );
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }
  }
);

const exportLSTExcel = createAsyncThunk(
  'EventPicking/LSTexport',
  async modelFilter => {
    const { data } = await queryDataExcel(exportExcelLSTUrl, modelFilter);
    const currentDateGmt8 = new Date().toLocaleString('en-US', {
      timeZone: 'Asia/Hong_Kong'
    });

    if (data) {
      const urlFake = window.URL.createObjectURL(new Blob([data]));
      const link = document.createElement('a');
      link.href = urlFake;
      link.setAttribute(
        'download',
        `Miscmatch - LST format - ${dayjs(currentDateGmt8).format(
          'hh:mm_A-YYYYMMDD'
        )}.xlsx`
      );
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }
  }
);

export { exportExcel, exportLSTExcel };
