import { useState } from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';

import ToastrWrapper from '../../components/ToastrWrapper';
import Filter from './Filter';

jest.mock('react-redux', () => ({
  useDispatch: jest.fn(),
  useSelector: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));
jest.mock('../../components/ToastrWrapper');

ToastrWrapper.showError = jest.fn().mockReturnValue('OK');
const state = {
  filter: {
    param: {
      date: 'Nov 11, 2022'
    },
    leagues: [{ value: 1, label: 'a' }],
    updateTypes: [{ value: 1, label: 'a' }]
  }
};
const mockedDispatch = jest.fn();
const mockSetState = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);
useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
useState.mockImplementation(init => [init, mockSetState]);

describe('test FilterTicketCenter', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('show list Filter ', async () => {
    const { container } = render(<Filter />);

    expect(container.querySelector('.calendar')).toBeInTheDocument();
    expect(screen.queryByText(/Select Game/i)).toBeInTheDocument();
    expect(screen.queryByText(/Filter Match/i)).toBeInTheDocument();
  });

  it('click Search, should dispatch', async () => {
    render(<Filter />);
    await fireEvent.click(screen.queryByText(/Search/i));
    expect(mockedDispatch).toHaveBeenCalledTimes(3);
  });
});
