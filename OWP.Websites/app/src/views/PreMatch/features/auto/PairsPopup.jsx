import { useState } from 'react';
import PropTypes from 'prop-types';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from 'reactstrap';
import { useDispatch, useSelector } from 'react-redux';
import { setAutoAssign, setAutoMode, setAssignments } from './autoSlice';
import { changeMode } from '../edit/editSlice';

import MajorPair from './MajorPair';
import MainPair from './MainPair';
import EarlyPair from './EarlyPair';
import ToastrWrapper from '../../../components/ToastrWrapper';
import Loading from '../../../components/Loading';
import { EARLY_OFFERED, MATCH_OFFER } from '../../const/const';

import { post } from '../../../Shared/Service/Service';

const PairsPopup = props => {
    const [isSubmitting, setIsSubmitting] = useState(false);
    const dispatch = useDispatch();
    const { isAuto, majorPairs, majorTakeOverPairs, earlyPairs, mainPairs } = useSelector(state => state.auto);
    const filter = useSelector(state => state.filter);

    const getPairs = pairs => {
        const result = [];

        pairs.forEach(item => {
            const pair = item.filter(x => x.userId > 0);
            if (pair.length > 0) {
                result.push(pair);
            }
        });

        return result;
    };

    const handleClosePopup = () => dispatch(setAutoAssign(false));

    const handleAutoAssignment = async () => {
        const tmpMajorPairs = getPairs(majorPairs);
        const tmpMajorTakeOverPairs = getPairs(majorTakeOverPairs);
        const tmpEarlyPairs = getPairs(earlyPairs);
        const tmpMainPairs = getPairs(mainPairs);
        if (tmpMajorPairs.length === 0 && tmpMajorTakeOverPairs.length === 0 && tmpEarlyPairs.length === 0 && tmpMainPairs.length === 0) {
            ToastrWrapper.showError('Please input at least a pairs.');
            return;
        }

        const modelFilter = {
            date: filter.filterDate,
            matchOffers: [...MATCH_OFFER.map(m => m.value)],
            earlyOffered: [...EARLY_OFFERED.map(m => m.value)],
            majorPairs: tmpMajorPairs,
            majorTakeOverPairs: tmpMajorTakeOverPairs,
            earlyPairs: tmpEarlyPairs,
            mainPairs: tmpMainPairs,
            url: '/api/PreMatchAssignment/auto-assign'
        };
        setIsSubmitting(true);
        const response = await post(modelFilter);
        setIsSubmitting(false);

        dispatch(setAutoMode(true));
        dispatch(setAssignments(response.data));
        dispatch(changeMode({ isEdit: true }));

        handleClosePopup();
    };

    return (
        <>
            {isSubmitting && <Loading />}
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
        </>
    );
};

PairsPopup.propTypes = {
    hasWrite: PropTypes.bool.isRequired
};
export default PairsPopup;