import PropTypes from 'prop-types';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from 'reactstrap';

const Dialog = props => {
  const {
    titleHeader,
    onClose,
    onSave,
    renderBody,
    hasWrite,
    size,
    className,
    commandCancelTitle = 'Cancel'
  } = props;

  return (
    <Modal
      tag='div'
      size={`${size ?? 'lg'}`}
      isOpen
      centered
      modalClassName={className}>
      <ModalHeader toggle={onClose}>{titleHeader}</ModalHeader>
      <ModalBody>{renderBody()}</ModalBody>
      <ModalFooter>
        <Button className='btn-cancel btn' onClick={onClose}>
          {commandCancelTitle}
        </Button>
        {hasWrite && (
          <Button className='btn-add btn' onClick={onSave}>
            Save
          </Button>
        )}
      </ModalFooter>
    </Modal>
  );
};

Dialog.propTypes = {
  titleHeader: PropTypes.string.isRequired,
  onClose: PropTypes.func.isRequired,
  onSave: PropTypes.func,
  renderBody: PropTypes.func.isRequired,
  hasWrite: PropTypes.bool,
  size: PropTypes.string,
  className: PropTypes.string,
  commandCancelTitle: PropTypes.string
};

export default Dialog;
