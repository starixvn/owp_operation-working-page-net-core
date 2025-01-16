import PropTypes from 'prop-types';
import useGetHeaderInfo from '../customHook/useGetHeaderInfo';
import HeaderCommon from '../share/HeaderCommon';
import ViewChangeLog from '../changeLog/ViewChangeLog';
import ViewDeletedLeagueCommon from '../share/ViewDeletedLeagueCommon';
import DeletedLeagueDayShift from '../deletedLeague/DeletedLeagueDayShift';
import { buildHeaderTitle } from '../../helper/preMatchHelper';
import { getLocaleDateTime } from '../../../Shared/DateTime/dateTimeHelper';

const propTypes = {
    isEdit: PropTypes.bool.isRequired
};

const HeaderDayShift = ({ isEdit }) => {
    const headerInfo = useGetHeaderInfo(true);
    const { shiftTime, updateTime, updateBy } = headerInfo;
    const title = `DAYSHIFT ${getLocaleDateTime(new Date(shiftTime), 'LL')}`;
    const renderByMode = isEditMode => {
        if (isEditMode) {
            return (
                <div className='btn-right'>
                    <ViewDeletedLeagueCommon isDay>
                        <DeletedLeagueDayShift />
                    </ViewDeletedLeagueCommon>
                    <ViewChangeLog isDay />
                </div>
            );
        }
        return null;
    };

    return (
        <HeaderCommon
            title={title}
            lastUpdate={buildHeaderTitle(updateTime, updateBy)}>
            {renderByMode(isEdit)}
        </HeaderCommon>
    );
};

HeaderDayShift.propTypes = propTypes;
export default HeaderDayShift;
