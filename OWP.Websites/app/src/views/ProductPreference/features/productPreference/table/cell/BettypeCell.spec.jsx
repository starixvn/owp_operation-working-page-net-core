import BettypeCell from './BettypeCell';
import { useDispatch } from 'react-redux';
import store from '../../../../app/storeNative';
import { render } from '@testing-library/react';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));

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
        mainBettypeOptionLiveTrading: [
          { resourceId: 1, value: '2', name: 'Section' }
        ],
        mainBettypeOptionLiveAssistant: [
          { resourceId: 15, value: '6min', name: 'Section' }
        ]
      }
    },
    productAssignment: {
      dropdownOptions: {
        betTypeOption: [{ value: 1, label: 'A' }]
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

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);

const defaultProps = {
  productInfo: {
    username: 'sa',
    sectionId: 3,
    ActorId: 123,
    betTypePreferencesValues: [{ value: 1, label: 'A' }],
    userId: 8
  },
  rowIndex: 0,
  onShowDropdownListOption: jest.fn()
};

describe('BettypeCell under test', () => {
  it('match snapshot', () => {
    const { getByText } = render(<BettypeCell {...defaultProps} />);
    expect(getByText('All')).toBeInTheDocument();
  });
});
