import { useEffect, useState } from 'react';
import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import PropTypes from 'prop-types';
import { get } from '../../Shared/Service/Service';
import { netCoreURL, netFwURL, getLogsHistory } from '../const/const.json';
import dayjs from 'dayjs';

const resolveUrl = () =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${getLogsHistory}`
    : `${netFwURL}/${getLogsHistory}`;

const HistoryModal = props => {
  const { infoSiteMatchId, header, onToggle } = props;

  const [logHistory, setLogHistory] = useState([]);

  useEffect(() => {
    (async () => {
      const url = resolveUrl();
      const response = await get({
        url,
        infoSiteMatchId
      });
      setLogHistory(response.data);
    })();
  }, [infoSiteMatchId]);

  const renderBody = () => {
    return logHistory.map((log, index) => {
      const { date, modifiedBy, action, oldValue, newValue } = log;
      return (
        <tr key={index}>
          <td>
            {dayjs(date, 'YYYY-MM-DDTHH:mm:ss').format('MM/DD/YYYY hh:mm A')}
          </td>
          <td>{modifiedBy}</td>
          <td>
            <span dangerouslySetInnerHTML={{ __html: action }} />
          </td>
          <td>{oldValue}</td>
          <td>{newValue}</td>
        </tr>
      );
    });
  };
  return (
    <Modal isOpen scrollable className='log-history-modal' size='xl' centered>
      <ModalHeader toggle={onToggle}>{header}</ModalHeader>
      <ModalBody className='modal-log-history-table'>
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
          <tbody>{renderBody()}</tbody>
        </table>
      </ModalBody>
    </Modal>
  );
};

HistoryModal.propTypes = {
  infoSiteMatchId: PropTypes.number.isRequired,
  onToggle: PropTypes.func.isRequired,
  header: PropTypes.string.isRequired
};

export default HistoryModal;
