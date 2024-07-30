import configureStore from 'redux-mock-store';
import CheckBoxFilter from './CheckBoxFilter';
import { useSelector, useDispatch, Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import PropTypes from 'prop-types';

const middlewares = [];
const mockStore = configureStore(middlewares);
const initialState = {
  eventChecking: { isChecked: false }
};
const store = mockStore(initialState);

const mockDispatch = jest.fn();
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

useSelector.mockImplementation(selectorFn => selectorFn(initialState));
useDispatch.mockReturnValue(mockDispatch);

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('ChatBoxFilter under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should call handleShowUnreadMessOnlyClick', () => {
    const { container } = render(<CheckBoxFilter />);

    const checkbox = container.querySelector(`[class='unread-checkbox']`);

    fireEvent.click(checkbox);

    expect(mockDispatch).toHaveBeenCalledTimes(1);
  });
});

const propTypes = {
  children: PropTypes.node
};

Wrapper.propTypes = propTypes;
