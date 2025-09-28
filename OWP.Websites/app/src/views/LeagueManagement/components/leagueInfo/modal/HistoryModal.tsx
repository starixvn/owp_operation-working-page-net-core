import { useEffect, useState } from 'react';
import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import { get } from '../../../../Shared/Service/Service';
import dayjs from 'dayjs';

interface FieldChange {
  field: string;
  oldValue: string;
  newValue: string;
}

interface LogEntry {
  logId: number;
  settingId: number;
  createdDate: string;
  modifiedBy: string;
  details: FieldChange[];
}

interface Props {
  leagueSettingId: number;
  leagueName: string;
  onToggle: () => void;
}

const HistoryModal = ({ leagueSettingId, leagueName, onToggle }: Props) => {
  const [logHistory, setLogHistory] = useState<LogEntry[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const fetchLogHistory = async () => {
    try {
      setIsLoading(true);
      setError(null);

      const url = 'api/NewLeagueSetting/get-logs';
      const response = await get({
        url,
        leagueSettingId
      });

      setLogHistory(response.data || []);
    } catch (err) {
      setError('Failed to load log history');
      console.error('Error fetching log history:', err);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    if (leagueSettingId) {
      fetchLogHistory();
    }
  }, [leagueSettingId]);

  const formatDate = (dateString: string): string => {
    return dayjs(dateString, 'YYYY-MM-DDTHH:mm:ss').format(
      'MM/DD/YYYY hh:mm A'
    );
  };

  const renderTableHeader = () => (
    <thead>
      <tr>
        <th>Date (GMT+8)</th>
        <th>Modified by</th>
        <th>Action</th>
        <th>Field</th>
        <th>Old Value</th>
        <th>New Value</th>
      </tr>
    </thead>
  );

  const renderTableBody = () => {
    if (isLoading) {
      return (
        <tbody>
          <tr>
            <td colSpan={6} className='text-center'>
              Loading...
            </td>
          </tr>
        </tbody>
      );
    }

    if (error) {
      return (
        <tbody>
          <tr>
            <td colSpan={6} className='text-center text-danger'>
              {error}
            </td>
          </tr>
        </tbody>
      );
    }

    if (logHistory.length === 0) {
      return (
        <tbody>
          <tr>
            <td colSpan={6} className='text-center'>
              No log history found
            </td>
          </tr>
        </tbody>
      );
    }

    return (
      <tbody>
        {logHistory.map(
          logEntry =>
            logEntry.details?.map((fieldChange, index) => (
              <tr key={`${logEntry.logId}-${index}`}>
                {index === 0 && (
                  <>
                    <td rowSpan={logEntry.details.length}>
                      {formatDate(logEntry.createdDate)}
                    </td>
                    <td rowSpan={logEntry.details.length}>
                      {logEntry.modifiedBy}
                    </td>
                    <td rowSpan={logEntry.details.length}>
                      Updated the setting
                    </td>
                  </>
                )}
                <td>{fieldChange.field}</td>
                <td>{fieldChange.oldValue}</td>
                <td>{fieldChange.newValue}</td>
              </tr>
            )) || []
        )}
      </tbody>
    );
  };

  return (
    <Modal
      isOpen
      scrollable
      className='league-setting-modal'
      size='xl'
      centered>
      <ModalHeader toggle={onToggle}>Log History - {leagueName}</ModalHeader>
      <ModalBody className='league-setting-modal-body'>
        <table className='league-setting-log-history-table'>
          {renderTableHeader()}
          {renderTableBody()}
        </table>
      </ModalBody>
    </Modal>
  );
};

export default HistoryModal;
