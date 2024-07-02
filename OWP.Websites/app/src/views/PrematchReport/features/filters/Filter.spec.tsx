import { useState } from 'react';
import Filter from './Filter';
import { useDispatch } from 'react-redux';
import { toDateDefault, fromDateDefault } from '../helpers/helpers';
import dayjs from 'dayjs';
import { fireEvent, render, screen } from '@testing-library/react';
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
    const { container } = render(<Filter />);
    const datePicker = container.getElementsByClassName('icon-calendar');
    expect(datePicker.length).toEqual(2);
  });

  it('should handle open FromDatePicker when click icon', () => {
    const { container } = render(<Filter />);
    const fromDateIcon = container.querySelector(
      '[id="icon-calendar-from-date"]'
    );
    expect(fireEvent.click(fromDateIcon!)).toHaveLength;
  });

  it('should handle change FromDatePicker when change date', () => {
    render(<Filter />);
    const { container } = render(<Filter />);
    const datePicker = container.querySelector('.from-date-picker');
    const newDate = new Date(2020, 1, 1);

    fireEvent.change(datePicker!, { target: { value: newDate } });
    const selectedDate = dayjs(
      new Date(datePicker!.getAttribute('value')!)
    ).format('MM/DD/YYYY');

    expect(mockSetState).toBeCalledTimes(1);
    expect(selectedDate).toEqual(dayjs(newDate).format('MM/DD/YYYY'));
  });

  it('should handle setInput set when change input', () => {
    const utils = render(<Filter />);
    const input = utils.getByPlaceholderText('Username or Nickname');
    fireEvent.change(input, { target: { value: 'second' } });
    expect(mockSetState).toBeCalledTimes(1);
  });

  it('should handle open toDatePicker when click icon', () => {
    const { container } = render(<Filter />);
    const toDateIcon = container.querySelector('[id="icon-calendar-to-date"]');
    expect(fireEvent.click(toDateIcon!)).toHaveLength;
  });

  it('should handle changeToDatePicker when change date', () => {
    render(<Filter />);
    const { container } = render(<Filter />);
    const datePicker = container.querySelector('.to-date');
    const tomorrow = new Date(new Date().setDate(new Date().getDate() + 5));

    fireEvent.change(datePicker!, { target: { value: tomorrow } });
    const selectedDate = dayjs(
      new Date(datePicker!.getAttribute('value')!)
    ).format('MM/DD/YYYY');

    expect(mockSetState).toBeCalledTimes(1);
    expect(selectedDate).toEqual(dayjs(tomorrow).format('MM/DD/YYYY'));
  });

  it('should handle search  when click search button', () => {
    render(<Filter />);
    fireEvent.click(screen.getByText('Search'));
    expect(mockDispatch).toHaveBeenCalledTimes(2);
  });

  it('should handle export  when click export button', () => {
    render(<Filter />);
    global.window = Object.create(window);
    const url = `undefined/api/PrematchReport/export-prematch-report?FromDate=${dayjs(
      fromDateDefault
    ).format('YYYY-MM-DD')}&ToDate=${dayjs(toDateDefault).format(
      'YYYY-MM-DD'
    )}&KeyWord=`;
    Object.defineProperty(window, 'location', {
      value: {
        href: url
      }
    });

    expect(window.location.href).toEqual(url);
    fireEvent.click(screen.getByText('Export'));
  });
});
