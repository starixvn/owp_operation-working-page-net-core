import PropTypes from 'prop-types';
import { TooltipText } from '../../../../../Shared/common/Tooltip';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

export const LeagueCell = props => {
  const { dataSource, rowIndex } = props;
  const { league, category, noHDP } = dataSource[rowIndex];

  return (
    <div className='league-name-cell'>
      <TooltipText
        text={league}
        toolTipContent={league}
        innerClassName='league-name'
        rowIndex={rowIndex}
      />
      <div className='label'>
        {category && (
          <TooltipText
            text={<span className='category'>{category}</span>}
            toolTipContent={category}
            rowIndex={rowIndex}
            innerClassName='category'
          />
        )}
        {noHDP && (
          <TooltipText
            text={<span className='no-hdp'>{noHDP}</span>}
            toolTipContent={noHDP}
            rowIndex={rowIndex}
            innerClassName='no-hdp'
          />
        )}
      </div>
    </div>
  );
};

LeagueCell.propTypes = propTypes;
