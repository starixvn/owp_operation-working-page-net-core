import { isEqual } from 'lodash';
import { Fragment, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import usePermission from '../../../Shared/customHook/usePermission';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { getTicketTypeRecipients } from '../ticketTypeThunkNative';
import { functionKey, checkLengthSubTypes } from '../../Const.json';
import Loading from '../../../components/Loading';
import {
  Recipient,
  Subtype,
  TicketTypeRecipientDataSource
} from '../TypeNative';
import { RootState } from '../../app/storeNative';
import ActionWrapper from '../cell/ActionWrapper';

const TicketTypeRecipientTable = () => {
  const permissionModel = usePermission(functionKey);
  const ticketTypeRecipient = useSelector(
    (state: RootState) => state.ticketTypeRecipient
  );
  const dispatch = useStableDispatch();

  useEffect(() => {
    dispatch(getTicketTypeRecipients());
  }, [dispatch]);

  if (
    isEqual(permissionModel.loading, REQUEST_STATUS.pending) ||
    isEqual(ticketTypeRecipient.loading, REQUEST_STATUS.pending)
  ) {
    return <Loading />;
  }

  const renderTableRow = (entity: TicketTypeRecipientDataSource) => {
    const subtypesLength = entity.subtypes.length;
    return (
      <tr key={entity.ticketType.typeId}>
        <td rowSpan={subtypesLength > 0 ? subtypesLength : 1}>
          {entity.ticketType.typeName}
        </td>
        <td className='subtype'>{entity.subtypes[0]?.subtypeName}</td>
        <td>
          {renderRecipients(entity.recipients, entity.subtypes[0]?.subtypeName)}
        </td>
        <td rowSpan={subtypesLength > 0 ? subtypesLength : 1}>
          <ActionWrapper {...entity} />
        </td>
      </tr>
    );
  };

  const renderSubtypeRows = (subtypes: Subtype[], recipients: Recipient[]) => {
    return subtypes.slice(1).map(subtype => (
      <tr key={subtype.subtypeId}>
        <td className='subtype'>{subtype.subtypeName}</td>
        <td>{renderRecipients(recipients, subtype.subtypeName)}</td>
      </tr>
    ));
  };

  const renderTable = () => {
    return ticketTypeRecipient.entities.map(entity => (
      <Fragment key={entity.ticketType.typeId}>
        {renderTableRow(entity)}
        {entity.subtypes.length >= checkLengthSubTypes &&
          renderSubtypeRows(entity.subtypes, entity.recipients)}
      </Fragment>
    ));
  };

  const renderRecipients = (recipients: Recipient[], subtypeName: string) => {
    return recipients
      .filter(item => item.subtypeName === subtypeName)
      .map(recipient => recipient.recipientName)
      .join(', ');
  };

  return (
    <table className='ticket-recipient-table'>
      <thead>
        <tr>
          <th>Ticket type</th>
          <th>Subtype</th>
          <th>Recipient (default)</th>
          <th>Action</th>
        </tr>
      </thead>
      <tbody>{renderTable()}</tbody>
    </table>
  );
};

export default TicketTypeRecipientTable;
