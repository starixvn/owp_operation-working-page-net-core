import { isEqual } from 'lodash';
import PropTypes from 'prop-types';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Loading from '../../../components/Loading';
import { pageSizeOptions, REQUEST_STATUS } from '../../../Shared/Constants';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import Pagination from '../../../Shared/Paging/Pagination';
import { URL_QUERY_CHANGE_LOG } from '../../const/const';
import { changeLog } from './ChangeLogSlice';
import ChangeLogTable from './ChangeLogTable';
import update from 'immutability-helper';

const propTypes = {
  isDay: PropTypes.bool.isRequired
};

const ChangeLog = ({ isDay }) => {
  const stableDispatch = useStableDispatch();
  const model = useSelector(state => state.changedLog);

  const [pageInfo, setPageInfo] = useState({
    currentPage: 1,
    pageSize: pageSizeOptions[0],
    pageSizeOptions
  });

  const handlePageChange = pageIndex => {
    const newPageInfo = update(pageInfo, {
      currentPage: { $set: pageIndex.currentPage }
    });
    setPageInfo(newPageInfo);
  };

  const handleSizeChange = pageSize => {
    const newPageInfo = update(pageInfo, {
      currentPage: { $set: 1 },
      pageSize: { $set: pageSize }
    });
    setPageInfo(newPageInfo);
  };

  useEffect(() => {
    const filterChangeLogModel = {
      url: URL_QUERY_CHANGE_LOG,
      pageSize: pageInfo.pageSize.value,
      pageNumber: pageInfo.currentPage,
      isDay
    };
    stableDispatch(changeLog(filterChangeLogModel));
  }, [isDay, pageInfo, stableDispatch]);

  const { Logs } = model.dataChangeLog;

  if (isEqual(model.loading, REQUEST_STATUS.loading) || Logs === null) {
    return <Loading />;
  }

  return (
    <>
      <ChangeLogTable logs={Logs || []} />
      <div className='paging-group'>
        <div className='pagination'>
          <Pagination
            isLastPage={model.dataChangeLog.IsLastPage}
            currentPage={pageInfo.currentPage}
            onPageChanged={handlePageChange}
            onPageSizeChange={handleSizeChange}
            pageSizeOptions={pageSizeOptions}
          />
        </div>
      </div>
    </>
  );
};
ChangeLog.propTypes = propTypes;
export default ChangeLog;
