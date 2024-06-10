import specialProductReducer from './specialProductReducer';
import {
  SPECIALPRODUCT_DELETE,
  SPECIALPRODUCT_QUERY,
  SPECIALPRODUCT_UPDATEINFO,
  ACTION_SPECIALPRODUCT,
  EXECUTE_SPECIALPRODUCT_FILTER
} from '../constants/action-type';
import { filter } from '../constants/SpecialProductConst';

describe('specialProductReducer', () => {
  const initState = {
    specialProductPending: false,
    specialProductData: [],
    specialProductError: [],
    specialProductDataSource: [],
    specialProductDuplicateMBFTHDP: null
  };

  const mockData = [
    {
      ProductId: 1,
      LastUpdate: 'sa',
      Spread_PS: 0.12,
      Spread_TB: 0.08,
      Spread_Winner: null
    }
  ];

  it('Should return status pending', () => {
    const actual = specialProductReducer(initState, {
      type: SPECIALPRODUCT_QUERY.PENDING
    });
    expect(actual).toEqual({
      ...initState,
      specialProductPending: true
    });
  });

  it('Should return error', () => {
    const actual = specialProductReducer(initState, {
      type: SPECIALPRODUCT_QUERY.ERROR,
      error: ['Failed to connect to server']
    });
    expect(actual).toEqual({
      ...initState,
      specialProductError: ['Failed to connect to server']
    });
  });

  it('Should return data', () => {
    const data = [
      {
        ProductId: 1,
        LastUpdate: 'sa',
        Spread_PS: 0.12,
        Spread_TB: 0.08,
        Spread_Winner: null
      }
    ];

    const actual = specialProductReducer(initState, {
      type: SPECIALPRODUCT_QUERY.SUCCESS,
      data
    });
    expect(actual).toEqual({
      ...initState,
      specialProductData: data,
      specialProductDataSource: data
    });
  });

  it('Should return data when update success', () => {
    const state = {
      ...initState,
      specialProductData: [
        {
          ProductId: 1,
          LastUpdate: 'sa',
          Spread_PS: 0.12,
          Spread_TB: 0.08,
          Spread_Winner: null
        }
      ],
      specialProductDataSource: mockData,
      specialProductPending: true
    };

    const actual = specialProductReducer(state, {
      type: SPECIALPRODUCT_UPDATEINFO.SUCCESS,
      data: {
        ProductId: 1,
        ErrorMessage: null
      },
      extraModel: {
        ProductId: 1,
        LastUpdate: 'sa',
        Spread_PS: 0.1,
        Spread_TB: 0.8,
        Spread_Winner: null
      }
    });
    expect(actual).toEqual({
      ...initState,
      specialProductPending: false,
      specialProductData: [
        {
          ProductId: 1,
          LastUpdate: 'sa',
          Spread_PS: 0.1,
          Spread_TB: 0.8,
          Spread_Winner: null
        }
      ],
      specialProductDataSource: [
        {
          ProductId: 1,
          LastUpdate: 'sa',
          Spread_PS: 0.1,
          Spread_TB: 0.8,
          Spread_Winner: null
        }
      ]
    });
  });

  it('Should return data when insert success', () => {
    const actual = specialProductReducer(initState, {
      type: SPECIALPRODUCT_UPDATEINFO.SUCCESS,
      data: {
        ProductId: 1,
        ErrorMessage: null
      },
      extraModel: {
        ProductId: -1,
        LastUpdate: 'sa',
        Maxbet_FTHDP: 1
      }
    });
    expect(actual).toEqual({
      ...initState,
      specialProductPending: false,
      specialProductData: [
        {
          ProductId: 1,
          LastUpdate: 'sa',
          Maxbet_FTHDP: 1
        }
      ],
      specialProductDataSource: [
        {
          ProductId: 1,
          LastUpdate: 'sa',
          Maxbet_FTHDP: 1
        }
      ]
    });
  });

  it('Should handle SPECIALPRODUCT_DELETE.SUCCESS', () => {
    const state = {
      ...initState,
      specialProductData: [
        {
          ProductId: 1,
          LastUpdate: 'sa',
          Spread_PS: 0.12,
          Spread_TB: 0.08,
          Spread_Winner: null
        }
      ],
      specialProductDataSource: mockData,
      specialProductPending: true
    };

    const actual = specialProductReducer(state, {
      type: SPECIALPRODUCT_DELETE.SUCCESS,
      data: {
        ProductId: 1
      }
    });
    expect(actual).toEqual({
      ...initState,
      specialProductPending: false,
      specialProductData: [
        {
          LastUpdate: 'sa',
          ProductId: 1,
          Spread_PS: 0.12,
          Spread_TB: 0.08,
          Spread_Winner: null
        }
      ],
      specialProductDataSource: [
        {
          LastUpdate: 'sa',
          ProductId: 1,
          Spread_PS: 0.12,
          Spread_TB: 0.08,
          Spread_Winner: null
        }
      ]
    });
  });

  it('Should handle ACTION_SPECIALPRODUCT.DELETE', () => {
    const state = {
      ...initState,
      specialProductData: [
        {
          ProductId: 1,
          LastUpdate: 'sa',
          Spread_PS: 0.12,
          Spread_TB: 0.08,
          Spread_Winner: null
        },
        {
          ProductId: -1,
          LastUpdate: 'sa',
          Spread_PS: 0.12,
          Spread_TB: 0.08,
          Spread_Winner: null
        }
      ],
      specialProductDataSource: mockData,
      specialProductPending: false
    };

    const actual = specialProductReducer(state, {
      type: ACTION_SPECIALPRODUCT.DELETE
    });

    const expected = {
      ...initState,
      specialProductData: [
        {
          ProductId: 1,
          LastUpdate: 'sa',
          Spread_PS: 0.12,
          Spread_TB: 0.08,
          Spread_Winner: null
        }
      ],
      specialProductDataSource: [
        {
          ProductId: 1,
          LastUpdate: 'sa',
          Spread_PS: 0.12,
          Spread_TB: 0.08,
          Spread_Winner: null
        }
      ]
    };
    expect(actual).toEqual(expected);
  });

  it('Should handle ACTION_SPECIALPRODUCT.ADD', () => {
    const state = {
      ...initState,
      specialProductData: [
        {
          ProductId: 1,
          LastUpdate: 'sa',
          Spread_PS: 0.12,
          Spread_TB: 0.08,
          Spread_Winner: null
        }
      ]
    };

    const actual = specialProductReducer(state, {
      type: ACTION_SPECIALPRODUCT.ADD,
      payload: {
        ProductId: -1,
        LastUpdate: 'sa',
        Spread_PS: 0.12,
        Spread_TB: 0.08,
        Spread_Winner: null
      }
    });
    expect(actual).toEqual({
      ...initState,
      specialProductData: [
        {
          ProductId: 1,
          LastUpdate: 'sa',
          Spread_PS: 0.12,
          Spread_TB: 0.08,
          Spread_Winner: null
        },
        {
          ProductId: -1,
          LastUpdate: 'sa',
          Spread_PS: 0.12,
          Spread_TB: 0.08,
          Spread_Winner: null
        }
      ]
    });
  });
});
