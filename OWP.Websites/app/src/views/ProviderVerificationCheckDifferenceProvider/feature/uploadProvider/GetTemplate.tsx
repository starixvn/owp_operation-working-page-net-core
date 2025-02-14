import { NextButton } from './Button';
import { exportExcel } from '../../../Shared/common/ExportLiveGameAssignment/Helper';

const GetTemplate = (props: TGetTemplate) => {

  const handleDownload = () => {
    const url = `/api/ProviderVerification/get-template/ProviderVerificationUploadTemplate.xlsx`;
    const fileName = "ProviderVerificationUploadTemplate"
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
