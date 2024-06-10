import { Fragment } from 'react';
import { Modal, ModalBody, ModalHeader } from 'reactstrap';
import { LogHistoryProps } from '../../../../../typing/tableType';
import { LogValue } from '../../../../../typing/osLeagueStateType';
import useStore from '../../../../../app/store';
import {
  detectLogChanged,
  buildLogOnCreated
} from '../../../../../helper/OsLeagueSettingHelper';
import { actionId } from '../../../../../osLeague.json';
const LogHistory = (props: LogHistoryProps) => {
  const { logData } = useStore(state => state);
  const { leagueName, handleClosePopup } = props;
  const buildLog: Array<LogValue[]> = [];
  logData.forEach(item =>
    item.action === actionId
      ? buildLog.push(buildLogOnCreated(item))
      : buildLog.push(detectLogChanged(item))
  );

  return (
    <Modal isOpen scrollable className='log-modal' centered>
      <ModalHeader
        className='modal-log-history-header history-popup'
        toggle={handleClosePopup}>
        Log History - {leagueName}
      </ModalHeader>
      <ModalBody className='modal-log-history-table'>
        <div className='tableFixHead-log'>
          <table className='log-history-table'>
            <thead>
              <tr>
                <th>Date (GMT+8)</th>
                <th>Modified by</th>
                <th>Action</th>
                <th>Old Value</th>
                <th>New Value</th>
              </tr>
            </thead>
            <tbody>
              {buildLog.map((item, index) => {
                return (
                  item.length > 0 && (
                    <Fragment key={`${item[0].modifyDate}-${index}`}>
                      <tr>
                        <td rowSpan={item.length + 1}>{item[0].modifyDate}</td>
                        <td rowSpan={item.length + 1}>{item[0].modifyBy}</td>
                        <td>{item[0].title}:</td>
                        <td></td>
                        <td></td>
                      </tr>
                      {item.map((logItem: LogValue) => (
                        <tr key={`${logItem.keyText}-${item[0].modifyDate}`}>
                          <td className='property'>
                            {logItem.keyText.charAt(0).toUpperCase() +
                              logItem.keyText.slice(1)}
                          </td>
                          <td>{logItem.valueOld}</td>
                          <td>{logItem.valueNew}</td>
                        </tr>
                      ))}
                    </Fragment>
                  )
                );
              })}
            </tbody>
          </table>
        </div>
      </ModalBody>
    </Modal>
  );
};

export default LogHistory;
