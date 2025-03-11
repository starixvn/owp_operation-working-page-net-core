import { NextButton } from './Button';
import { TGetTemplate } from '../../type/typeNative';
import { exportExcel } from '../../../Shared/common/ExportExcel/Helper';

const GetTemplate = (props: TGetTemplate) => {

  const handleDownload = async () => {
    const url = "/api/ProviderVerification/get-template/ProviderVerificationUploadTemplate.xlsx";
    const fileName = "ProviderVerificationUploadTemplate.xlsx";
    exportExcel(url, fileName);
  };

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
