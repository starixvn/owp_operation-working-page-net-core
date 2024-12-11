import { useDispatch, useStore } from 'react-redux';
import { changeMode } from '../edit/editSlice';
import { resetCopyInfo } from '../copy/copySlice';
import { reloadDataAfterEdit } from '../filter/filterSlice';
import {
    clearStateDayShift,
    setIsShowLeague
} from '../league/shiftSlice/shiftSlice';

import { setAutoMode, setAssignments } from '../auto/autoSlice';

import Spinner from '../../../components/Spinner';
import { filterPrematchUpdated } from '../../helper/preMatchHelper';
import SaveComponent from './SaveComponent';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { getLastUpdate } from '../header/headerSlice';
import { LAST_UPDATED, URL, simpleApiQueryUrl } from '../../const/const';
import { post } from '../../ImportPrematchHelperNative';

const DayShiftSaver = () => {
    const dispatch = useDispatch();
    const store = useStore();

    const commonDispatchAction = () => {
        dispatch(resetCopyInfo());
        dispatch(clearStateDayShift());
        dispatch(changeMode({ isEdit: false }));
        dispatch(reloadDataAfterEdit());
        dispatch(setIsShowLeague());
        dispatch(setAutoMode(false));
        dispatch(setAssignments([]));
    };

    const handleCancel = e => {
        e.preventDefault();

        commonDispatchAction();
    };

    const createSaveModel = () => {
        const { dayShift, filter } = store.getState();

        const { entities, prematchIdDeleted, prematchIdUpdated } = dayShift;
        const { filterDate } = filter;
        const PrematchUpdated = filterPrematchUpdated(entities, prematchIdUpdated);
        const postModel = {
            url: URL.SAVE_PREMATCH_ASSIGNMENT,
            PrematchIdDeleted: prematchIdDeleted,
            PrematchUpdated,
            Date: filterDate
        };

        return postModel;
    };

    const handleSave = async event => {
        event.preventDefault();

        const { auto, filter } = store.getState();
        Spinner.show();

        if (auto.autoMode) {
            const autoModel = {
                date: filter.filterDate,
                assignments: auto.assignments,
                url: '/api/PreMatchAssignment/submit-auto-assign'
            };
            await post(autoModel);
        } else {
            const postModel = createSaveModel();
            await post(postModel);
        }

        Spinner.hide();

        ToastrWrapper.showInfo('The assignment has been updated successfully!');

        const model = {
            url: simpleApiQueryUrl,
            apiKey: LAST_UPDATED.apiKey,
            isDay: true
        };
        dispatch(getLastUpdate(model));
        commonDispatchAction();
    };

    return <SaveComponent onSave={handleSave} onCancel={handleCancel} />;
};

export default DayShiftSaver;
