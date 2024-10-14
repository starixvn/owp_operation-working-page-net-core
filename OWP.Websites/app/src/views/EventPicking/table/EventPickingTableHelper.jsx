import handleNoOfferAllMatch from '../helper/noOfferAllMatchHelper';
import updateConfirmAllEvent from '../helper/confirmedHelper';
const buildColumnValueByColumnName = (
  columnKey,
  isNoOfferAll,
  isConfirmAll
) => {
  const columnMappings = {
    'No #': {
      checked: isNoOfferAll,
      onCheckChange: handleNoOfferAllMatch
    },
    'Checked': {
      checked: isConfirmAll,
      onCheckChange: updateConfirmAllEvent
    }
  };

  return columnMappings[columnKey] || {};
};

export { buildColumnValueByColumnName };
