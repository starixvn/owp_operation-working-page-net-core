import { detectLogChanged } from './buildLogHelper';

describe('test helper', () => {
  it('detectLogChanged', () => {
    const logValue = {
      action: 128,
      oldValue: {
        fT_HDP: null,
        fT_OU: null,
        firstHalf_HDP: '20000',
        firstHalf_OU: null,
        noOfCorner_FT_HDP: null,
        noOfCorner_FT_OU: null,
        noOfCorner_1H_HDP: null,
        noOfCorner_1H_OU: null,
        nextCorner_FT_HDP: null,
        totalBooking_FT_HDP: null,
        totalBooking_FT_OU: null,
        totalBooking_1H_HDP: null,
        totalBooking_1H_OU: null,
        nextBooking_FT_HDP: null,
        secondAccept_Main: null,
        secondAccept_Corner: null,
        secondAccept_TotalBooking: '1',
        secondAccept_Fantasy: ''
      },
      newValue: {
        fT_HDP: null,
        fT_OU: null,
        firstHalf_HDP: '12000',
        firstHalf_OU: null,
        noOfCorner_FT_HDP: null,
        noOfCorner_FT_OU: null,
        noOfCorner_1H_HDP: null,
        noOfCorner_1H_OU: null,
        nextCorner_FT_HDP: null,
        totalBooking_FT_HDP: null,
        totalBooking_FT_OU: null,
        totalBooking_1H_HDP: null,
        totalBooking_1H_OU: null,
        nextBooking_FT_HDP: null,
        secondAccept_Main: null,
        secondAccept_Corner: null,
        secondAccept_TotalBooking: '3',
        secondAccept_Fantasy: '2'
      },
      modifyDate: '08/30/2022 03:56 AM',
      modifyBy: 'sa'
    };
    const secondAccepts = [
      { value: null, label: '-- None --' },
      { value: 1, label: '10s Accept' },
      { value: 2, label: 'Any' },
      { value: 3, label: 'KO+15' }
    ];
    const result = detectLogChanged(logValue, secondAccepts);
    const expected = [
      {
        modifyBy: 'sa',
        modifyDate: '08/30/2022 03:56 AM',
        keyText: 'Maxbet - Main Match - 1H HDP',
        valueNew: '12K',
        valueOld: '20K'
      },
      {
        keyText: 'Second Accept Value - Total Bookings',
        modifyBy: 'sa',
        modifyDate: '08/30/2022 03:56 AM',
        valueNew: 'KO+15',
        valueOld: '10s Accept'
      },
      {
        keyText: 'Second Accept Value - Fantasy',
        modifyBy: 'sa',
        modifyDate: '08/30/2022 03:56 AM',
        valueNew: 'Any',
        valueOld: 'None'
      }
    ];
    expect(expected).toEqual(result);
  });
});
