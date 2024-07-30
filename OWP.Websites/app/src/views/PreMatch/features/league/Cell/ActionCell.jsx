import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { TrashIcon } from 'mdi-react';
import { useSelector } from 'react-redux';
import { deleteRow } from '../shiftSlice/shiftSlice';
import ToastrWrapper from '../../../../components/ToastrWrapper';
import { isEqual } from 'lodash';
import useStableDispatch from '../../../../Shared/customHook/useStableDispatch';
import { resetCopyInfo } from '../../copy/copySlice';
import Spinner from '../../../../components/Spinner';
import {
  QUERY_CHECK_LEAGUE_CONTAINS_ASSIGNMENT,
  simpleApiQueryUrl
} from '../../../const/const';
import { post } from '../../../ImportPrematchHelperNative';

const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired
};

const ActionCell = props => {
  const { rowIndex, dataSource } = props;
  const { PreMatchLeagueId, LeagueName, InfoSiteMatchId, MatchOffer } =
    dataSource[rowIndex];
  const copyModel = useSelector(state => state.copy);
  const { highlightedCell } = copyModel;
  const stableDispatch = useStableDispatch();

  const handleDelete = async () => {
    Spinner.show();
    const result = await post({
      url: simpleApiQueryUrl,
      apiKey: QUERY_CHECK_LEAGUE_CONTAINS_ASSIGNMENT,
      PreMatchLeagueId
    });
    const isContainsAssignment = result.data[0].IsContainsAssignment;
    Spinner.hide();

    if (!isContainsAssignment) {
      const confirmMessage = `Delete League ${LeagueName} ?`;
      const confirmResult = await ToastrWrapper.confirmYesNo(confirmMessage);

      if (confirmResult.value) {
        stableDispatch(
          deleteRow({
            preMatchLeagueId: PreMatchLeagueId
          })
        );
        highlightedCell.PreMatchLeagueId.some(id => PreMatchLeagueId === id) &&
          stableDispatch(resetCopyInfo());
      }
    } else {
      await ToastrWrapper.showError('Please remove all assignment');
    }
  };

  if (InfoSiteMatchId) {
    return (
      <Cell className='action-no'>
        <div className='match-offer'>
          {MatchOffer && isEqual(MatchOffer, 'Opened') && (
            <span className='icon-completed' />
          )}
          {MatchOffer && !isEqual(MatchOffer, 'Opened') && (
            <span>{MatchOffer}</span>
          )}
        </div>
      </Cell>
    );
  }

  return (
    <Cell className='header-Assignment'>
      <TrashIcon onClick={handleDelete} className='icon-trash' />
    </Cell>
  );
};
ActionCell.propTypes = propTypes;

export default ActionCell;
