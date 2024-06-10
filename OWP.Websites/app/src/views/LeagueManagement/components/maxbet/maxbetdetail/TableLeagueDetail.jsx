import { useEffect, useCallback } from 'react';
import renderTableBody from './renderTableBody';
import { Table } from 'fixed-data-table-2';
import MaxbetConst from '../../../constants/MaxbetConst';
import { useSelector, useDispatch, useStore } from 'react-redux';
import {
  selectLeagueDetail,
  selectToggleColum
} from '../../../selector/SelectorLeagueDetail';
import Loading from '../../../../components/Loading';
import { postData, queryMatchOfferLists } from '../../../actions/action';
import {
  QUERY_LEAGUE_DETAIL,
  QUERY_MATCH_OFFER
} from '../../../constants/action-type';
import { hasWriteOnPageMaxbetGeneral } from '../../../helper/commonHelper';
import { useParams } from 'react-router-dom';
import { buildQuerySimpleApi } from '../../../../Shared/Constants';
import { withResizeDetector } from 'react-resize-detector';

const TableLeagueDetail = () => {
  const { id } = useParams();
  const dispatch = useDispatch();
  const store = useStore();
  const modelToggle = useSelector(selectToggleColum);
  const modelLeague = useSelector(selectLeagueDetail);
  const { matchOffers } = store.getState().matchOffersReducer;
  const { policyData } = store.getState().policyReducer;
  const hasWriteGeneral = hasWriteOnPageMaxbetGeneral(policyData);
  const hasWriteOnCondition = policyData.conditionPolicy.HasWrite;
  const { rowHeight, maxHeight, headerHeight, groupHeaderHeight } =
    MaxbetConst.tableMaxbetGeneralInfo;
  const stableDispatch = useCallback(dispatch, [dispatch]);
  useEffect(() => {
    stableDispatch(
      postData(
        buildQuerySimpleApi(),
        {
          OWPLeagueId: parseInt(id, 10),
          apiKey: MaxbetConst.leagueManagementGetLeagueSettingAPIKey
        },
        QUERY_LEAGUE_DETAIL
      )
    );
  }, [stableDispatch, id]);

  useEffect(() => {
    if (!matchOffers) {
      stableDispatch(
        queryMatchOfferLists(MaxbetConst.getMatchOffers, QUERY_MATCH_OFFER)
      );
    }
  }, []);

  const dataLength = modelLeague.leagueDetailData.length;
  const tableHeight =
    groupHeaderHeight + rowHeight + dataLength * rowHeight + 30;
  if (modelLeague.leagueDetailPending) {
    return <Loading />;
  }
  const widthTable =
    window.innerWidth > 680 ? window.innerWidth - 110 : window.innerWidth;

  return (
    <div className='table'>
      <Table
        key={modelLeague.tableKey}
        groupHeaderHeight={groupHeaderHeight}
        rowHeight={rowHeight}
        rowsCount={dataLength}
        width={widthTable}
        height={tableHeight}
        maxHeight={maxHeight}
        headerHeight={headerHeight}>
        {renderTableBody(
          modelLeague.leagueDetailData,
          modelToggle.columnKeys,
          hasWriteGeneral,
          hasWriteOnCondition
        )}
      </Table>
    </div>
  );
};

TableLeagueDetail.displayName = 'TableLeagueDetail';
export default withResizeDetector(TableLeagueDetail);
