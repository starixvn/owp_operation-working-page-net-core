import { useEffect, useState } from 'react';
import { Modal, ModalBody, ModalHeader } from 'reactstrap';
import Utils from '../../../Shared/Utils';
import { TLog } from '../type/TableType.native';
import { getLogHistory } from '../../service';
import { uniqueId } from 'lodash';

type Props = {
  handleClosePopup: () => void;
  userId: number;
  userName: string;
};

const HistoryPopup = ({ handleClosePopup, userId, userName }: Props) => {
  const [logs, setLogs] = useState<Array<TLog>>([]);

  useEffect(() => {
    (async () => {
      const data = await getLogHistory(userId);
      setLogs(data);
    })();
  }, [userId]);

  return (
    <div>
      <Modal isOpen scrollable className='status-fix' centered>
        <ModalHeader toggle={handleClosePopup}>
          Target Update History - {userName}
        </ModalHeader>
        <ModalBody>
          <div className='status-fix-body'>
            <table className='target-history-table'>
              <thead>
                <tr>
                  <th>Date (GMT+8)</th>
                  <th>Modified by</th>
                  <th>Old Value</th>
                  <th>New Value</th>
                </tr>
              </thead>
              <tbody>
                {logs.map((x: TLog) => (
                  <tr key={uniqueId()}>
                    <td>{x.modifyDate}</td>
                    <td>{x.modifyBy}</td>
                    <td>{x.oldValue}</td>
                    <td>{x.newValue}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </ModalBody>
      </Modal>
    </div>
  );
};

export default HistoryPopup;
