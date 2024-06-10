import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import PropTypes from 'prop-types';
import DropDownFilter from './DropDownFilter';
import HistoryTable from './Table/HistoryTable';

const HistoryModal = props => {
  const { productId, maxbetFTHDP, onToggle } = props;

  return (
    <Modal isOpen scrollable className='loghistory-modal' size='xl' centered>
      <ModalHeader toggle={onToggle}>
        History of Special Product<span>&apos;</span>s Max Bet/Spread with{' '}
        <strong>MB FT.HDP of {maxbetFTHDP}K </strong>
        <DropDownFilter productId={productId} />
      </ModalHeader>
      <ModalBody>
        <HistoryTable />
      </ModalBody>
    </Modal>
  );
};

HistoryModal.propTypes = {
  productId: PropTypes.number.isRequired,
  maxbetFTHDP: PropTypes.number.isRequired,
  onToggle: PropTypes.func.isRequired
};

export default HistoryModal;
