import { useEffect } from 'react';
import { Modal, ModalBody } from 'reactstrap';
import EditRecipient from '../editRecipientPopup/EditRecipient';
import Subtype from '../subType/SubTypeManagement';
import { TicketTypeRecipientDataSource } from '../TypeNative';
import Command from './Command';
import useStore from '../../app/zustand/ticketTypeStore';
interface Props extends TicketTypeRecipientDataSource {
  onClose: () => void;
  onSave: () => Promise<void>;
}

const Dialog = (props: Props) => {
  const { onClose, onSave, ...rest } = props;
  const { ticketType } = rest;

  useEffect(() => {
    useStore.setState({
      subtypeSelected: rest.subtypes[0],
      recipients: rest.recipients,
      subtypes: rest.subtypes
    });
    return () =>
      useStore.setState({
        subtypeSelected: {
          subtypeId: 0,
          subtypeName: '',
          typeId: 0
        },
        recipients: [],
        subtypes: []
      });
  }, [rest.recipients, rest.subtypes]);

  return (
    <Modal isOpen scrollable className='recipient-modal' centered>
      <ModalBody>
        <h2 className='title'>
          <b>{`Edit Ticket: ${ticketType.typeName}`}</b>
        </h2>
        <Subtype ticketTypeId={rest.ticketType.typeId} />
        <EditRecipient />

        <Command onClose={onClose} onSave={onSave} />
      </ModalBody>
    </Modal>
  );
};

export default Dialog;
