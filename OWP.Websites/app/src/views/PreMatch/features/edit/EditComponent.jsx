import { useDispatch, useSelector } from 'react-redux';
import { changeMode } from './editSlice';
import dayjs from 'dayjs';
import ToastrWrapper from './../../../components/ToastrWrapper';
import {
    UncontrolledButtonDropdown,
    DropdownMenu,
    DropdownItem,
    DropdownToggle
} from 'reactstrap';
import { setAutoAssign } from '../auto/autoSlice';

const EditComponent = () => {
    const dispatch = useDispatch();
    const { filterDate } = useSelector(state => state.filter);

    const handleManualAssign = () => {
        const validEditedDate = dayjs(new Date()).add(-3, 'days');

        if (dayjs(filterDate).isBefore(validEditedDate)) {
            return ToastrWrapper.showError('You cannot edit any assignments that were created more than three days ago.');
        }

        return dispatch(changeMode({ isEdit: true }));
    };

    const handleAutoAssign = () => {
        const validEditedDate = dayjs(new Date()).add(-3, 'days');

        if (dayjs(filterDate).isBefore(validEditedDate)) {
            return ToastrWrapper.showError('You cannot edit any assignments that were created more than three days ago.');
        }

        return dispatch(setAutoAssign(true));
    };

    return (
        <UncontrolledButtonDropdown>
            <DropdownToggle caret className='button-submit'>
                Edit
            </DropdownToggle>
            <DropdownMenu>
                <DropdownItem key='manual-assign' onClick={handleManualAssign}>
                    <span className='icon-tap' /> Manual Assign
                </DropdownItem>
                <DropdownItem key='auto-assign' onClick={handleAutoAssign}>
                    <span className='icon-refresh-setting' /> Auto Assign
                </DropdownItem>
            </DropdownMenu>
        </UncontrolledButtonDropdown>
    );
};

export default EditComponent;
