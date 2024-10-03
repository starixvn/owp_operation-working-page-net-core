
import PropTypes from 'prop-types';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from 'reactstrap';
import { useDispatch, useSelector } from 'react-redux';
import { setAutoAssign } from './autoSlice';
import MajorPair from './MajorPair';
import MainPair from './MainPair';
import EarlyPair from './EarlyPair';

const PairsPopup = props => {
  const dispatch = useDispatch();
  const isAuto = useSelector(state => state.auto.isAuto);

  const renderAssignment =()=>{
    return(
      <div className='assignment'>
        <div className='hdp__wrapper'>
        <label className='hdp'>HDP</label>
        {renderInputAssignment()}
        </div>
        <div className='ou__wrapper'>
        <label className='ou'>OU</label>
        {renderInputAssignment()}
        </div>
       
       
      </div>
    );
  };

  const renderBody = ()=>{
    return (
      <div className='body'>
        <MajorPair/>
        <EarlyPair/>
        <MainPair/>
        </div>
    );
  };

  const handleClosePopup = ()=> dispatch(setAutoAssign(false));
  const handleAutoAssignment=()=>{
    // TODO
  };

  return (
    <Modal
    tag='div'
    size='lg'
    isOpen={isAuto}
    centered>
    <ModalHeader>
      <div className='sun__icon'/>
      <label className='title'>Day Shift Pairs</label>
    </ModalHeader>
    <ModalBody>{renderBody()}</ModalBody>
    <ModalFooter>
      <Button className='btn-cancel btn' onClick={handleClosePopup}>
      Cancel
      </Button>
      {props.hasWrite && (
        <Button className='btn-add btn' onClick={handleAutoAssignment}>
          Run Auto Assignment
        </Button>
      )}
    </ModalFooter>
  </Modal>
  );
};

PairsPopup.propTypes = {
  hasWrite: PropTypes.bool.isRequired
};
export default PairsPopup;