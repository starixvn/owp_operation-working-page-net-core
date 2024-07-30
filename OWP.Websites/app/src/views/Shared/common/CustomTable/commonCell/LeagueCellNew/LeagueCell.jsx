import { Cell } from 'fixed-data-table-2';
import { nanoid } from '@reduxjs/toolkit';
import LeagueContent from '../commonContentNew/LeagueContent';
import PropTypes from 'prop-types';
const propTypes = {
  leagueName: PropTypes.string.isRequired,
  isRead: PropTypes.bool.isRequired,
  children: PropTypes.node.isRequired
};

const LeagueCell = ({ leagueName = '', isRead = false, children }) => {
  return (
    <Cell
      key={nanoid()}
      className={`league-info-cell conversation-cell ${
        !isRead ? 'has-unread' : 'cell'
      }`}>
      <LeagueContent leagueName={leagueName} className='conversation-cell' />
      {children}
    </Cell>
  );
};

LeagueCell.propTypes = propTypes;

export default LeagueCell;
