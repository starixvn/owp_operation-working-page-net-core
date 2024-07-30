import dayjs from 'dayjs';
import Spinner from '../../../components/Spinner';
import Utils from '../../Utils';
import axios from 'axios';

const queryDataExcel = async (url: string) => {
  const response = await axios.get(url, { responseType: 'blob' });
  return response.data;
};

const exportExcel = async (url: string, fileName: string) => {
  (async () => {
    Spinner.show();
    const data = await queryDataExcel(url);
    if (data) {
      const urlFake = window.URL.createObjectURL(new Blob([data]));
      const link = document.createElement('a');
      link.href = urlFake;
      link.setAttribute('download', fileName);
      document.body.appendChild(link);
      link.click();
    }
    Spinner.hide();
  })();
};

const liveGameAssignmentFileName = (date: Date) =>
  `Assignment - ${dayjs(Utils.formatDate(date)).format('YYYYMMDD')}.xlsx`;

export { exportExcel, liveGameAssignmentFileName };
