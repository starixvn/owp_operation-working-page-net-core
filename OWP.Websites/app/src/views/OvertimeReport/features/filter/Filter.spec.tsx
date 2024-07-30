import { screen, render, fireEvent } from '@testing-library/react';
import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Loading from '../../../components/Loading';
import Filter from './Filter';
import Month from './filterField/MonthNative';
import Select from 'react-select';
import Location from './filterField/LocationNative';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));
jest.mock('react', () => ({
  ...(jest.requireActual('react') as object),
  useState: jest.fn()
}));

const setState = jest.fn();
const mockDispatch = jest.fn();
const mockHandleSearch = jest.fn();
const mockHandleExport = jest.fn();
const mockStateLocation = { label: 'All', value: '' };

const setUp = (status: string = 'idle') => {
  (useState as jest.Mock).mockImplementation(init => [init, setState]);
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn({
      filter: {
        status
      }
    })
  );
};

const props = (isByMonthTab: boolean) => {
  return {
    isByMonthTab,
    handleSearch: mockHandleSearch,
    handleExport: mockHandleExport
  };
};

describe('test Filter', () => {
  beforeEach(() => {
    setUp();
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('status=pending, should render Loading', () => {
    setUp('pending');
    const { container } = render(<Filter {...props(false)} />);
    const loadingElement = container.querySelector('.loading-process');
    expect(loadingElement).toBeInTheDocument();
  });

  it('isByMonthTab=false, should not render Month and call dispatch, mockHandleSearch', () => {
    setUp('idle');
    render(<Filter {...props(false)} />);
    const month = screen.queryByText('Month');
    expect(month).not.toBeInTheDocument();
    expect(mockHandleSearch).toHaveBeenCalledTimes(1);
    expect(mockDispatch).toHaveBeenCalledTimes(1);
  });

  it('status=idle, is, should render Month', () => {
    setUp('idle');
    render(<Filter {...props(true)} />);
    const month = screen.queryByText('Month');
    expect(month).toBeInTheDocument();
  });

  it('click submit button, should call mockHandleSearch 1 time', () => {
    setUp('idle');
    render(<Filter {...props(false)} />);
    expect(mockHandleSearch).toHaveBeenCalledTimes(1);
    const button = screen.getByText('Search');
    fireEvent.click(button);
    expect(mockHandleSearch).toHaveBeenCalledTimes(2);
  });

  it('click export button, should call mockHandleExport 1 time', () => {
    setUp('idle');
    render(<Filter {...props(false)} />);
    const button = screen.getByText('Export');
    fireEvent.click(button);
    expect(mockHandleExport).toHaveBeenCalledTimes(1);
  });

  // it('change Month, should call dispatch 1 time', () => {
  //   const component = mount(<Filter {...props(true)} />);
  //   component.find('[name="month"]').simulate('change', {
  //     value: 2,
  //     label: 'February'
  //   } );
  //   expect(setState).toHaveBeenCalledTimes(1);
  // });
});
