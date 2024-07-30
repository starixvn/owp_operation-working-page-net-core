import PropTypes from 'prop-types';
import { useEffect, useState } from 'react';
import { Modal, ModalBody, ModalHeader } from 'reactstrap';
import Utils from '../../../Shared/Utils';
import { getStatusHistory } from '../../services/service';

const propTypes = {
  ticketId: PropTypes.number.isRequired,
  handleClosePopup: PropTypes.func.isRequired
};

const StatusPopup = props => {
  const { ticketId, handleClosePopup } = props;
  const [data, setData] = useState([]);

  useEffect(() => {
    (async () => {
      const dataResponse = await getStatusHistory(ticketId);
      setData(dataResponse);
    })();
  }, [ticketId]);

  return (
    <Modal isOpen scrollable className='recipient-modal' centered>
      <ModalHeader toggle={handleClosePopup}>
        Status History - #{ticketId}
      </ModalHeader>
      <ModalBody>
        <div className='tableFixHead'>
          <table className='status-history-table'>
            <thead>
              <tr>
                <th>Date (GMT+8)</th>
                <th>Modified by</th>
                <th>Old Value</th>
                <th>New Value</th>
              </tr>
            </thead>
            <tbody>
              {data.map(x =>
                <tr key={x.logId}>
                  <td>{Utils.formatDateTimeString(x.logDate)}</td>
                  <td>{x.modifiedBy}</td>
                  <td>{x.oldValue}</td>
                  <td>{x.newValue}</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </ModalBody>
    </Modal>
  );
};

StatusPopup.propTypes = propTypes;
export default StatusPopup;