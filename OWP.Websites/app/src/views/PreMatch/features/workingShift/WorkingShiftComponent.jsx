import { useState } from 'react';
import { Modal, ModalBody } from 'reactstrap';
import WorkingShift from './WorkingShift';
import { useSelector } from 'react-redux';
import { getLocaleDateTime } from '../../../Shared/DateTime/dateTimeHelper';

const WorkingShiftComponent = () => {
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const filterDate = useSelector(state => state.filter.filterDate);
  const dayShiftFormatter = getLocaleDateTime(new Date(filterDate), 'll');
  return (
    <div className='working-shift'>
      <button
        className='button-working-shift'
        onClick={() => setModalIsOpen(true)}>
        Working Shift
      </button>
      <Modal tag='div' centered isOpen={modalIsOpen} size='xl'>
        <ModalBody>
          <h2>Working Shift - {dayShiftFormatter}</h2>
          <WorkingShift />
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

export default WorkingShiftComponent;
