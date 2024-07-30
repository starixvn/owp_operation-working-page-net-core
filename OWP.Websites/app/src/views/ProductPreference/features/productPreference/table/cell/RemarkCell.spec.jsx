import RemarkCell from './RemarkCell';
import { render } from '@testing-library/react';
import { useDispatch } from 'react-redux';
import store from '../../../../app/storeNative';

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
        ActorId: 8,
        Username: 'sa'
      }
    },
    filter: {
      dropdownOptions: {
        levelOptionLiveTrading: [{ value: 1, label: 'A' }],
        levelOptionLiveAssistant: [{ value: 1, label: 'A' }],
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

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);
const defaultProps = {
  productInfo: {
    levelId: 1,
    userId: 8,
    username: 'sa'
  },
  rowIndex: 0
};

describe('LevelCell under test', () => {
  it('match snapshot', () => {
    const { container } = render(<RemarkCell {...defaultProps} />);
    expect(container.firstChild).toMatchSnapshot();
  });
});
