import { useState } from 'react';
import EditIcon from 'mdi-react/EditIcon';
import Condition from '../../../condition/round/Condition';
import { Cell } from 'fixed-data-table-2';
import PropTypes from 'prop-types';

const EditCell = ({ leagueSettingId }) => {
  const [diplayModal, setDisplayModal] = useState(false);
  const renderModalCondition = () => {
    return (
      <Condition
        leagueSettingId={leagueSettingId}
        onClose={() => setDisplayModal(false)}
      />
    );
  };

  return (
    <Cell className='edit-condition'>
      <EditIcon
        name='iconEdit'
        className='icon-edit'
        size={13}
        onClick={() => setDisplayModal(true)}
      />
      {diplayModal && renderModalCondition()}
    </Cell>
  );
};
EditCell.propTypes = {
  leagueSettingId: PropTypes.number.isRequired
};

export default EditCell;
