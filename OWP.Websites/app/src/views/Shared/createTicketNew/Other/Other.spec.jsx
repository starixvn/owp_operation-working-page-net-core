import { fireEvent, render } from '@testing-library/react';
import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Other from './Other';

const setState = jest.fn();
const mockDispatch = jest.fn();
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  createRef: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));

const setup = (valueOther = 'johnny') => {
  const initialState = {
    createTicketBoard: { valueOther: valueOther }
  };
  useState.mockImplementation(init => [init, setState]);
  useSelector.mockImplementation(selectorFn => selectorFn(initialState));
  useDispatch.mockReturnValue(mockDispatch);
};
describe('Other under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should render event from API', () => {
    setup();
    const { container } = render(<Other />);
    let event = {
      preventDefault: jest.fn(),
      target: {
        value: 'test'
      }
    };

    fireEvent.change(container.querySelector('#subject'), event);
    expect(mockDispatch).toHaveBeenCalledTimes(2);
  });
});
