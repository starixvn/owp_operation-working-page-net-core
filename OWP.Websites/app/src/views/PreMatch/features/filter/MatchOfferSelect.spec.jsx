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

  // it('should call dispatch, when change input', () => {
  //   const { container } = render(<MatchOfferSelect />);
  //   const element = container.querySelector('input');
  //   fireEvent.focus(element);
  //   fireEvent.change(element, { target: { value: 'abc' } });

  //   expect(element).toBeInTheDocument();
  // fireEvent.focus(container.querySelector('input'));
  //     fireEvent.keyDown(container.querySelector('input'), { key: 'ArrowDown', code: 40 });

  //     fireEvent.click(getByText('1H 1X2'));

  //     expect(getByText('1H 1X2')).toBeInTheDocument();

  // });
});
