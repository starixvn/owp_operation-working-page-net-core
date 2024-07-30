import { useState } from 'react';
import { Modal, ModalBody } from 'reactstrap';
import PropTypes from 'prop-types';

const propTypes = {
  className: PropTypes.string,
  title: PropTypes.string.isRequired,
  children: PropTypes.node,
  size: PropTypes.string
};

const ModalCommon = props => {
  const [modalIsOpen, setModalIsOpen] = useState(false);

  return (
    <div className={props.className}>
      <label onClick={() => setModalIsOpen(true)}>{props.title}</label>
      <Modal tag='div' centered isOpen={modalIsOpen} size={props.size}>
        <ModalBody className='text-left'>
          {props.children}
          <button
            className='button-close'
            onClick={() => setModalIsOpen(false)}>
            Close
          </button>
        </ModalBody>
      </Modal>
    </div>
  );
};

ModalCommon.propTypes = propTypes;
export default ModalCommon;
