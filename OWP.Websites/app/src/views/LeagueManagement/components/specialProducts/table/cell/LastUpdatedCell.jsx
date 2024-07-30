import { useState } from 'react';
import PropTypes from 'prop-types';
import HistoryModal from '../../historyTable/HistoryModal.native';
import HistoryIcon from 'mdi-react/HistoryIcon';

const propTypes = {
  updatedBy: PropTypes.string,
  productId: PropTypes.number.isRequired,
  maxbetFTHDP: PropTypes.number
};

const LastUpdatedCell = ({ updatedBy, productId, maxbetFTHDP }) => {
  const [isopen, setisopen] = useState(false);

  const handleToggle = () => {
    setisopen(!isopen);
  };

  return (
    <>
      <span>{updatedBy}</span>
      {productId > 0 && (
        <HistoryIcon className='loghistory-button' onClick={handleToggle} />
      )}
      {isopen && (
        <HistoryModal
          productId={productId}
          maxbetFTHDP={maxbetFTHDP}
          onToggle={handleToggle}
        />
      )}
    </>
  );
};

LastUpdatedCell.propTypes = propTypes;

export default LastUpdatedCell;
