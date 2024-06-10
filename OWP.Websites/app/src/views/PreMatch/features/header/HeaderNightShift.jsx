import PropTypes from 'prop-types';
import useGetHeaderInfo from '../customHook/useGetHeaderInfo';
import HeaderCommon from '../share/HeaderCommon';
import ViewChangeLog from '../changeLog/ViewChangeLog';
import ViewDeletedLeagueCommon from '../share/ViewDeletedLeagueCommon';
import { buildHeaderTitle } from '../../helper/preMatchHelper';
import DeletedLeagueNightShift from '../deletedLeague/DeletedLeagueNightShift';
import { convertFrom24hTo12h, getLocaleDateTime } from '../../../Shared/DateTime/dateTimeHelper';
const propTypes = {
  isEdit: PropTypes.bool.isRequired
};

const HeaderNightShift = ({ isEdit }) => {
  const headerInfo = useGetHeaderInfo();
  const { headerState, shiftTime, updateTime, updateBy } = headerInfo;
  const title = `NIGHTSHIFT ${getLocaleDateTime(new Date(shiftTime), 'LL')}`;
  const startTime = headerState.startTimeOriginal ?? '';

  const renderByMode = isEditMode => {
    if (isEditMode) {
      return (
        <div className='btn-right'>
          <ViewDeletedLeagueCommon>
            <DeletedLeagueNightShift />
          </ViewDeletedLeagueCommon>
          <ViewChangeLog isDay={false} />
        </div>
      );
    }
    return (
      <div className='take-over'>
        Take Over(GMT+8):
        <span>{` ${convertFrom24hTo12h(startTime) || ''}`}</span>
      </div>
    );
  };

  return (
    <HeaderCommon
      title={title}
      lastUpdate={buildHeaderTitle(updateTime, updateBy)}>
      {renderByMode(isEdit)}
    </HeaderCommon>
  );
};

HeaderNightShift.propTypes = propTypes;
export default HeaderNightShift;
