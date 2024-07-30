import { useDispatch } from 'react-redux';
import store from '../../../../app/storeNative';
import MultipleGameCell from './MultipleGameCell';
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
        multipleMaxGamesOption: [{ value: 1, label: 'A' }]
      }
    },
    productAssignment: {
      dropdownOptions: {
        multipleGamesOption: [{ value: 1, label: 'A' }]
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
    multipleGamesId: 1,
    userId: 8,
    username: 'sa',
    ActorId: 123
  },
  rowIndex: 0
};

describe('MultipleGameCell under test', () => {
  it('match snapshot', () => {
    const { container } = render(<MultipleGameCell {...defaultProps} />);
    expect(container.firstChild).toMatchSnapshot();
  });
});
