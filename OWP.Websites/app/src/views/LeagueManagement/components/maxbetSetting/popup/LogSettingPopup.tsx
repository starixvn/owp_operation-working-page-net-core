import { Modal, ModalBody, ModalHeader } from 'reactstrap';
import useStore, { RootState } from '../store/storeNative';
import { detectLogChanged } from '../helper/buildLogHelper';
import { LogData, LogValue } from '../typeNative';
import { unit } from '../Const.json';
const LogSettingPopup = () => {
  const {
    setCloseLogPopup,
    isOpenLogPopup,
    maxBetValue,
    logData,
    secondAcceptValues
  } = useStore((state: RootState) => state);
  const handleClosePopup = () => {
    setCloseLogPopup();
  };
  const buildLog: Array<LogValue[]> = [];
  logData.forEach((item: LogData) =>
    buildLog.push(detectLogChanged(item, secondAcceptValues))
  );
  const reduceBuildLog = buildLog.filter(item => item.length > 0);
  return (
    <Modal isOpen={isOpenLogPopup} scrollable className='log-modal' centered>
      <ModalHeader
        className='modal-log-history-header history-popup'
        toggle={handleClosePopup}>
        Log History | Main Match - MaxBet FT HDP of {maxBetValue / unit}K
      </ModalHeader>
      <ModalBody className='modal-log-history-table'>
        <div className='tableFixHead-log'>
          <table className='log-history-table'>
            <thead>
              <tr>
                <th>Product Settings</th>
                <th>Action</th>
                <th>Old Value</th>
                <th>New Value</th>
                <th>Last Modified</th>
                <th>By</th>
              </tr>
            </thead>
            <tbody>
              {reduceBuildLog.map(item => {
                return item.map((log, index) => (
                  <tr key={`${log.modifyDate}-${index}`}>
                    <td>{log.keyText}</td>
                    <td>Update</td>
                    <td>{log.valueOld}</td>
                    <td>{log.valueNew}</td>
                    <td>{log.modifyDate}</td>
                    <td>{log.modifyBy}</td>
                  </tr>
                ));
              })}
            </tbody>
          </table>
        </div>
      </ModalBody>
    </Modal>
  );
};

export default LogSettingPopup;
