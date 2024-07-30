import { Fragment } from 'react';
import { Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/storeNative';
import { isNull } from 'lodash';
import { System } from '../../const/ProviderArrangement.json';

type TPopupProps = {
  handleClosePopup(): void;
};

const LogHistoryPopup = ({ handleClosePopup }: TPopupProps) => {
  const { logData } = useSelector((state: RootState) => state.providerReducer);

  const buildDisplayNewData = (
    oldValue: Array<string> | null,
    newValue: Array<string>
  ) =>
    newValue.map((newItem, index) => {
      const oldPosition = oldValue?.findIndex(oldItem => oldItem === newItem);

      return (
        <Fragment key={`${newItem}-${index}`}>
          <span
            className={
              oldPosition === index || isNull(oldValue) ? '' : 'highlight-item'
            }>
            {index + 1}. {newItem}
          </span>
          <span>{index !== newValue.length - 1 && ` > `}</span>
        </Fragment>
      );
    });

  const buildDisplayOldData = (oldValue?: Array<string> | null) => {
    const element = oldValue
      ?.map((item, index) => `${index + 1}. ${item}`)
      .join(' > ');
    return <span>{element}</span>;
  };

  return (
    <Modal isOpen scrollable className='log-modal' centered>
      <ModalHeader
        className='modal-log-history-header history-popup'
        toggle={handleClosePopup}>
        Log History - Provider Arrangement
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
              {logData.map((item, index) => {
                return (
                  <tr key={`${item.modifyDate}-${index}`}>
                    <td>{item.modifyDate}</td>
                    <td>{item.modifyBy || System}</td>
                    <td className='action-column'>{item.action}</td>
                    <td>{buildDisplayOldData(item.oldValue)}</td>
                    <td>{buildDisplayNewData(item.oldValue, item.newValue)}</td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </ModalBody>
      <ModalFooter>Total Records: {logData.length}</ModalFooter>
    </Modal>
  );
};

export default LogHistoryPopup;
