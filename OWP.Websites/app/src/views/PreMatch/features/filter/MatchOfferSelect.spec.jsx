import { fireEvent, render } from '@testing-library/react';
import { useState } from 'react';
import { useDispatch } from 'react-redux';
import MatchOfferSelect from './MatchOfferSelect';

jest.mock('../../app/store');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));

const mockedDispatch = jest.fn();
const mockSetState = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);
useState.mockImplementation(init => [init, mockSetState]);

describe('MatchOfferSelect under unit test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('MatchOfferSelect should render correct', () => {
    const { container } = render(<MatchOfferSelect />);
    const element = container.querySelector('.multi-select-dropdown');
    expect(element).toBeInTheDocument();
  });
});
