import axios, { AxiosResponse } from 'axios';
import { saveAs } from 'file-saver';
import { getValueFilter, urlExport } from '../utils';

const ExportButton = () => {
  const handleExport = async () => {
    const { user, roleId, sectionId, departmentId, categoryId, locationCode } =
      getValueFilter();
    const url = `${urlExport}?User=${user}&CategoryId=${categoryId}&DepartmentId=${departmentId}&LocationCode=${locationCode}&RoleId=${roleId}&SectionId=${sectionId}`;

    const instance = axios.create({
      baseURL: '/'
    });

    const options = {
      url,
      method: 'GET',
      headers: { 'Accept': 'application/json' },
      responseType: 'blob' as const
    };
    const response: AxiosResponse<Blob> = await instance.request(options);
    const filename = response.headers?.['content-disposition']
      ?.split(';')
      ?.find(n => n.includes('filename='))
      ?.replace('filename=', '')
      ?.trim();
    const fileUrl = window.URL.createObjectURL(new Blob([response.data]));
    saveAs(fileUrl, filename);
  };

  return (
    <div className='filter-control btn-block'>
      <button className='button-export' onClick={handleExport}>
        Export Excel
      </button>
    </div>
  );
};

export default ExportButton;
