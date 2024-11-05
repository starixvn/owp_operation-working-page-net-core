
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

    const handleClosePopup = () => dispatch(setAutoAssign(false));
    const handleAutoAssignment = () => {
        // TODO
    };

    return (
        <Modal tag='div' size='lg' className='pairs-modal-dialog'
            isOpen={isAuto} centered
        >
            <ModalHeader>
                <div className='sun__icon' />
                <label className='title'>Day Shift Pairs</label>
            </ModalHeader>
            <ModalBody>
                <MajorPair />
                <EarlyPair />
                <MainPair />
            </ModalBody>
            <ModalFooter>
                <Button className='btn-cancel btn' onClick={handleClosePopup}>
                    Cancel
                </Button>
                {
                    props.hasWrite &&
                    <Button className='btn-add btn' onClick={handleAutoAssignment}>
                        Run Auto Assignment
                    </Button>
                }
            </ModalFooter>
        </Modal>
    );
};

PairsPopup.propTypes = {
    hasWrite: PropTypes.bool.isRequired
};
export default PairsPopup;