import { useSelector } from 'react-redux';
import { DateTimeFilterCommon } from '../../ImportPrematchHelperNative';
import CollapseBtn from '../expandCollapse/CollapseBtn';
import ExpandBtn from '../expandCollapse/ExpandBtn';
import ShowUnreadMessageCheckBox from '../expandCollapse/ShowUnreadMessageCheckBox';
const EditFilterDayShift = () => {
    const filterDate = useSelector(state => state.filter.filterDate);
    return (
        <div className='left-filter-dayshift edit-mode'>
            <DateTimeFilterCommon
                defaultDate={new Date(filterDate)}
                isHideButtonSubmit isEdit
            >
                <CollapseBtn />
                <ExpandBtn />
            </DateTimeFilterCommon>
            <ShowUnreadMessageCheckBox />
        </div>
    );
};
export default EditFilterDayShift;
