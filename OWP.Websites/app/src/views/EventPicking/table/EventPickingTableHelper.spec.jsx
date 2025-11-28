import { buildColumnValueByColumnName } from './EventPickingTableHelper';
import handleNoOfferAllMatch from '../helper/noOfferAllMatchHelper';
import updateConfirmAllEvent from '../helper/confirmedHelper';

jest.mock('../helper/noOfferAllMatchHelper');
jest.mock('../helper/confirmedHelper');

describe('EventPickingTableHelper under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should return correct column value for "No #" column', () => {
    const isNoOfferAll = true;
    const isConfirmAll = false;

    const result = buildColumnValueByColumnName(
      'No #',
      isNoOfferAll,
      isConfirmAll
    );

    expect(result).toEqual({
      checked: true,
      onCheckChange: handleNoOfferAllMatch
    });
  });

  it('should return correct column value for "Checked" column', () => {
    const isNoOfferAll = false;
    const isConfirmAll = true;

    const result = buildColumnValueByColumnName(
      'Checked',
      isNoOfferAll,
      isConfirmAll
    );

    expect(result).toEqual({
      checked: true,
      onCheckChange: updateConfirmAllEvent
    });
  });

  it('should return empty object for unknown column key', () => {
    const result = buildColumnValueByColumnName('UnknownColumn', false, false);

    expect(result).toEqual({});
  });

  it('should return correct values when both flags are true', () => {
    const resultNoOffer = buildColumnValueByColumnName('No #', true, true);
    const resultChecked = buildColumnValueByColumnName('Checked', true, true);

    expect(resultNoOffer.checked).toBe(true);
    expect(resultChecked.checked).toBe(true);
  });

  it('should return correct values when both flags are false', () => {
    const resultNoOffer = buildColumnValueByColumnName('No #', false, false);
    const resultChecked = buildColumnValueByColumnName('Checked', false, false);

    expect(resultNoOffer.checked).toBe(false);
    expect(resultChecked.checked).toBe(false);
  });
});
