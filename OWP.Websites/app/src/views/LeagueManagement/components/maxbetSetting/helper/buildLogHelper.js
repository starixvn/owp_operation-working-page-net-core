import {
  formatMaxbetValueForLog,
  isSecondAcceptValue,
  formatSpreadValue,
  isSpreadValue
} from './helper';
import { isEmpty } from 'lodash';
const detectLogChanged = (item, secondAccepts) => {
  const logChanged = [];

  for (const key in item.oldValue) {
    if (item.oldValue[key] !== item.newValue[key]) {
      logChanged.push({
        modifyBy: item.modifyBy,
        modifyDate: item.modifyDate,
        keyText: buildNameOfProductSetting(key),
        valueNew: buildTextForSecondAccept(
          key,
          item.newValue[key],
          secondAccepts
        ),
        valueOld: buildTextForSecondAccept(
          key,
          item.oldValue[key],
          secondAccepts
        )
      });
    }
  }
  return logChanged;
};

const buildTextForSecondAccept = (key, value, secondAccepts) => {
  if (isSecondAcceptValue(key)) {
    if (isEmpty(value)) {
      return 'None';
    }
    const matchedItem = secondAccepts.find(x => x.value === Number(value));
    return matchedItem?.label ?? '';
  }

  if (isSpreadValue(key)) {
    return formatSpreadValue(value);
  }

  return formatMaxbetValueForLog(value);
};

const buildNameOfProductSetting = key => {
  const product = {
    'fT_HDP': 'Maxbet - Main Match - FT HDP',
    'fT_OU': 'Maxbet - Main Match - FT OU',
    'firstHalf_HDP': 'Maxbet - Main Match - 1H HDP',
    'firstHalf_OU': 'Maxbet - Main Match - 1H OU',
    'nextBooking_FT_HDP': 'Maxbet - Next Booking - FT HDP',
    'nextCorner_FT_HDP': 'Maxbet - Next Corner - FT HDP',
    'noOfCorner_1H_HDP': 'Maxbet - No. Of Corners - FT HDP',
    'noOfCorner_1H_OU': 'Maxbet - No. Of Corners - FT OU',
    'noOfCorner_FT_HDP': 'Maxbet - No. Of Corners - 1H HDP',
    'noOfCorner_FT_OU': 'Maxbet - No. Of Corners - 1H OU',
    'secondAccept_Corner': 'Second Accept Value - Corner',
    'secondAccept_Fantasy': 'Second Accept Value - Fantasy',
    'secondAccept_Main': 'Second Accept Value - Main',
    'secondAccept_TotalBooking': 'Second Accept Value - Total Bookings',
    'totalBooking_1H_HDP': 'Maxbet - Total Bookings - 1H HDP',
    'totalBooking_1H_OU': 'Maxbet - Total Bookings - 1H OU',
    'totalBooking_FT_HDP': 'Maxbet - Total Bookings - FT HDP',
    'totalBooking_FT_OU': 'Maxbet - Total Bookings - FT OU',
    'noOfCorner_FT_Spread_HDP': 'Spread - No. Of Corners - FT HDP',
    'noOfCorner_FT_Spread_OU': 'Spread - No. Of Corners - FT OU',
    'nextCorner_FT_Spread_HDP': 'Spread - Next Corner - FT HDP',
    'totalBooking_FT_Spread_HDP': 'Spread - Total Bookings - FT HDP',
    'totalBooking_FT_Spread_OU': 'Spread - Total Bookings - FT OU',
    'nextBooking_FT_Spread_HDP': 'Spread - Next Booking - FT HDP'
  };
  return product[key];
};

export { detectLogChanged };
