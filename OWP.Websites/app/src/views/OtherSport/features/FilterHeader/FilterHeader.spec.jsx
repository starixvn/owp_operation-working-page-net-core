import dayjs from 'dayjs';
import FilterHeader from './FilterHeader';
import { useState, useDispatch } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';

import '@testing-library/jest-dom';

jest.mock('react-redux', () => ({
  useState: jest.fn(),
  useDispatch: jest.fn()
}));

jest.mock('../../otherSport.json', () => ({
  OtherSport: {
    rootUrl: 'abc',
    download: 'abcd'
  }
}));

const assignMock = jest.fn();

delete window.location;
window.location = { assign: assignMock };

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);

const mockSetState = jest.fn();
useState.mockImplementation(init => [init, mockSetState]);

const props = {
  policy: {
    Username: 'sa'
  }
};

describe('FilterHeader under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
    assignMock.mockClear();
  });

  it('should render from date filter with current date', () => {
    const { container } = render(<FilterHeader {...props} />);
    const datePicker = container.querySelector('.from-date');

    const expected = dayjs(new Date()).format('MM/DD/YYYY');
    const selectedDate = dayjs(
      new Date(datePicker.getAttribute('value'))
    ).format('MM/DD/YYYY');

    expect(selectedDate).toEqual(expected);
  });

  it('should update state when change from date filters', () => {
    const { container } = render(<FilterHeader {...props} />);
    const datePicker = container.querySelector('.from-date');
    const today = new Date();
    const tomorrow = new Date(new Date().setDate(today.getDate() + 2));

    fireEvent.change(datePicker, { target: { value: tomorrow } });
    const selectedDate = dayjs(
      new Date(datePicker.getAttribute('value'))
    ).format('MM/DD/YYYY');

    expect(selectedDate).toEqual(dayjs(tomorrow).format('MM/DD/YYYY'));
  });

  it('should render to date filter with current date + 2', () => {
    const { container } = render(<FilterHeader {...props} />);
    const datePicker = container.querySelector('.to-date');

    const expected = dayjs()
      .add(2, 'day')
      .format('MM/DD/YYYY');
    const selectedDate = dayjs(datePicker.getAttribute('value')).format(
      'MM/DD/YYYY'
    );

    expect(selectedDate).toEqual(expected);
  });

  it('should update state when change to date filters', () => {
    const { container } = render(<FilterHeader {...props} />);
    const datePicker = container.querySelector('.to-date');
    const today = new Date();
    const tomorrow = new Date(new Date().setDate(today.getDate() + 2));

    fireEvent.change(datePicker, { target: { value: tomorrow } });
    const selectedDate = dayjs(datePicker.getAttribute('value')).format(
      'MM/DD/YYYY'
    );

    expect(selectedDate).toEqual(dayjs(tomorrow).format('MM/DD/YYYY'));
  });

  it('should render employee filter correct', () => {
    const { getByText } = render(<FilterHeader {...props} />);

    expect(getByText('Employee')).toBeInTheDocument();
  });

  it('should dispatch query when clicking submit', () => {
    const { container } = render(<FilterHeader {...props} />);
    const employee = container.querySelector('.input-field');
    fireEvent.change(employee, { target: { value: 'sa' } });
    const submitButton = container.querySelector('.button-submit');
    fireEvent.click(submitButton);

    expect(useDispatch).toHaveBeenCalled();
  });

  it('should dispatch query when clicking download', () => {
    const { container } = render(<FilterHeader {...props} />);
    const today = dayjs(new Date()).format('YYYY-MM-DD');
    const submitButton = container.querySelectorAll('.button-submit')[1];
    fireEvent.click(submitButton);
    expect(window.location.href).toContain(today.toString());
  });
});
