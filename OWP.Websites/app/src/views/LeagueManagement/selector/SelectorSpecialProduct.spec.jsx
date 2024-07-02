import { selectSpecialProductData } from './SelectorSpecialProduct';

describe('SpecialProductSelector', () => {
  it('select specialProductData ', () => {
    const state = {
      specialProductReducer: {
        specialProductPending: false,
        specialProductData: [],
        specialProductError: [],
        specialProductDuplicateMBFTHDP: null
      }
    };
    const expected = state.specialProductReducer;
    expect(selectSpecialProductData(state)).toEqual(expected);
  });
});
