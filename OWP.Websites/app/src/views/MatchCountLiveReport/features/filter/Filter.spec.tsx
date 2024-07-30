import { useState } from 'react';
import Filter from './Filter';
import { fireEvent, render } from '@testing-library/react';
import { useDispatch } from 'react-redux';
import { toDateDefault, fromDateDefault } from '../../helper/helper';
import dayjs from 'dayjs';
import ToastrWrapper from '../../../components/ToastrWrapper';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useRef: jest.fn(),
  useState: jest.fn()
}));
jest.mock('../../../components/ToastrWrapper');
ToastrWrapper.showError = jest.fn();

const mockDispatch = jest.fn();
const mockSetState = jest.fn();

const setUp = () => {
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useState as jest.Mock).mockImplementation(init => [init, mockSetState]);
};

describe('test for Filter component', () => {
  beforeEach(() => {
    setUp();
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('render filter correct', () => {
    const { getByText } = render(<Filter />);
    expect(getByText('From Date')).toBeInTheDocument();
    expect(getByText('To Date')).toBeInTheDocument();
  });

  it('should handle open FromDatePicker when click icon', () => {
    const { container } = render(<Filter />);
    const fromDateIcon = container.querySelector('#icon-calendar-from-date');
    expect(fireEvent.click(fromDateIcon!)).toBeInTheDocument;
  });

  it('should handle open toDatePicker when click icon', () => {
    const { container } = render(<Filter />);
    const toDateIcon = container.querySelector('#icon-calendar-to-date');
    expect(toDateIcon).toBeInTheDocument();
    fireEvent.click(toDateIcon!);
  });

  it('should change value when change from date', () => {
    const { container } = render(<Filter />);
    const component = container.querySelector('.from-date-picker');
    const newDate = '2020-05-24';
    fireEvent.change(component!, { target: { value: newDate } });
    const selectedDate = dayjs(
      new Date(component!.getAttribute('value')!)
    ).format('MM/DD/YYYY');

    expect(mockSetState).toBeCalledTimes(1);
    expect(selectedDate).toEqual(dayjs(newDate).format('MM/DD/YYYY'));
  });

  it('should change value when change to date', () => {
    const { container } = render(<Filter />);
    const component = container.querySelector('.to-date-picker');
    const newDate = new Date(new Date().setDate(new Date().getDate() + 5));
    fireEvent.change(component!, { target: { value: newDate } });
    const selectedDate = dayjs(
      new Date(component!.getAttribute('value')!)
    ).format('MM/DD/YYYY');

    expect(mockSetState).toBeCalledTimes(1);
    expect(selectedDate).toEqual(dayjs(newDate).format('MM/DD/YYYY'));
  });

  it('should handle search  when click search button', () => {
    const { container } = render(<Filter />);
    const button = container.querySelector('.button-submit');
    fireEvent.click(button!);
    expect(mockDispatch).toHaveBeenCalledTimes(2);
  });

  it('should handle export  when click export button', () => {
    const { container } = render(<Filter />);
    const button = container.querySelector('.button-export');
    global.window = Object.create(window);
    const url = `undefined/api/MatchCountLiveReport/export?FromDate=${dayjs(
      fromDateDefault
    ).format('YYYY-MM-DD')}&ToDate=${dayjs(toDateDefault).format(
      'YYYY-MM-DD'
    )}`;
    Object.defineProperty(window, 'location', {
      value: {
        href: url
      }
    });
    expect(window.location.href).toEqual(url);
    expect(fireEvent.click(button!)).toHaveLength;
  });
});
