import { nomalizeMaxbetSpreadResponse } from './nomalizeResponseHelper';

describe('nomalizeResponseHelper', () => {
  it('Should nomalizeMaxbetSpreadResponse correct', () => {
    const dataSource = [
      {
        GroupValue: null,
        GroupLabel: null,
        MinSpread: 0.1,
        MaxSpread: 0,
        MinMaxbetWD: 11000,
        MaxMaxbetWD: 11000,
        MinMaxbetWK: 11000,
        MaxMaxbetWK: 0,
        MinFT1X2: 11000,
        MaxFT1X2: 11000
      }
    ];
    const expected = [
      {
        GroupValue: 0,
        GroupLabel: '',
        MinSpread: '10%',
        MaxSpread: '0%',
        MinMaxbetWD: '11K',
        MaxMaxbetWD: '11K',
        MinMaxbetWK: '11K',
        MaxMaxbetWK: '0K',
        MinFT1X2: '11K',
        MaxFT1X2: '11K'
      }
    ];
    expect(nomalizeMaxbetSpreadResponse(dataSource)).toEqual(expected);
  });
});
