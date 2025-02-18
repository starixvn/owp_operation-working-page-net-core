import { NextButton } from './Button';
import { saveAs } from "file-saver";
import axios, { AxiosResponse } from 'axios';

const GetTemplate = (props: TGetTemplate) => {

  const handleDownload = async () => {
    const url = "/api/ProviderVerification/get-template/ProviderVerificationUploadTemplate.xlsx";
    const fileName = "ProviderVerificationUploadTemplate.xlsx";
    const options = {
        url,
        method: 'GET',
        headers: { 'Accept': 'application/json' },
        responseType: 'blob' as const
      };
    const response: AxiosResponse<Blob> = await axios.request(options);
    const fileUrl = window.URL.createObjectURL(new Blob([response.data]));
    saveAs(fileUrl, fileName);
  }

  return (
    <>
      <div className='filter-row'>
        <button className='button-export' onClick={handleDownload}>
          Download
        </button>
      </div>
      <div className='wizard-buttons'>
        <NextButton />
      </div>
    </>
  );
};

export default GetTemplate;
