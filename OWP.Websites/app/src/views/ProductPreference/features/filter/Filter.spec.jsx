import { render } from '@testing-library/react';
import { Provider } from 'react-redux';
import store from '../../app/storeNative';
import Filter from './Filter';

const mockState = {
  store: {
    policy: {
      permission: {
        HasWrite: true,
        Username: 'sa',
        ActorId: 8
      }
    },
    filter: {
      dropdownOptions: {
        multipleMaxGamesOption: [{ value: 1, label: 'A' }],
        locationOption: [{ value: 1, label: 'A' }],
        roleOption: [{ value: 1, label: 'A' }],
        sectionOption: [{ value: 1, label: 'A' }]
      }
    }
  },
  dispatchReturnValue: {
    isSuccessful: true
  },
  selectorReturnValue: {
    validation: {
      validationResult: {},
      loading: true
    }
  }
};

store.getState = () => mockState.store;

describe('Filter component', () => {
  let mockDispatch;
  let mockUseSelector;

  beforeEach(() => {
    mockDispatch = jest.fn();
    mockUseSelector = jest.fn();
    jest.mock('../../../Shared/customHook/useStableDispatch');
    jest.mock('react-redux', () => ({
      useSelector: mockUseSelector,
      useDispatch: jest.fn(() => mockDispatch)
    }));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render correctly', () => {
    mockUseSelector.mockReturnValueOnce({ filterInfo: {} });

    const { container } = render(
      <Provider store={store}>
        <Filter />
      </Provider>
    );
    expect(container).toMatchSnapshot();
  });
});
