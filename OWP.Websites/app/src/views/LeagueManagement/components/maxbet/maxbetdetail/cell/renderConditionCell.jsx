import ColumnConst from '../../../../constants/ColumnConst';
import { Cell } from 'fixed-data-table-2';
import PropTypes from 'prop-types';
import CheckIcon from 'mdi-react/CheckIcon';
import EditCell from './EditCell';
import { UncontrolledTooltip } from 'reactstrap';
import { v4 as uuidv4 } from 'uuid';

const { columnLeagueDetail } = ColumnConst;
const { condition } = columnLeagueDetail;

const HasTV = ({ hasTV, tv }) => {
  const id = `TV-${uuidv4()}`;
  return (
    <Cell className='has-tv' id={id}>
      {hasTV && (
        <CheckIcon name='iconDelete' className='icon-delete' size={13} />
      )}
      {tv}
      {tv && (
        <UncontrolledTooltip placement='top-start' target={id}>
          {tv}
        </UncontrolledTooltip>
      )}
    </Cell>
  );
};

HasTV.propTypes = {
  hasTV: PropTypes.bool,
  tv: PropTypes.string
};

const Round = ({ round }) => {
  const id = `Round-${uuidv4()}`;
  return (
    <Cell className='round' id={id}>
      {round}
      {round && (
        <UncontrolledTooltip placement='top-start' target={id}>
          {round}
        </UncontrolledTooltip>
      )}
    </Cell>
  );
};

Round.propTypes = {
  round: PropTypes.string
};

const Division = ({ division }) => {
  const id = `Division-${uuidv4()}`;
  return (
    <Cell id={id} className='division'>
      {division}
      {division && (
        <UncontrolledTooltip placement='top-start' target={id}>
          {division}
        </UncontrolledTooltip>
      )}
    </Cell>
  );
};

Division.propTypes = {
  division: PropTypes.string
};

const renderConditionCell = (data, columnKey) => {
  switch (columnKey) {
    case condition.subHeader.hasTV.columnKey:
      return <HasTV hasTV={data.HasTV} tv={data.TV} />;
    case condition.subHeader.round.columnKey:
      return <Round round={data.Round} />;
    case condition.subHeader.division.columnKey:
      return <Division division={data.Division} />;
    case condition.subHeader.edit.columnKey:
      return <EditCell leagueSettingId={data.LeagueSettingId} />;
  }
  return undefined;
};

export default renderConditionCell;
