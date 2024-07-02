import { useState, createRef } from 'react';
import { fireEvent, render } from '@testing-library/react';
import OrderHeader from './OrderHeader';
import { useDispatch, useSelector } from 'react-redux';
import { get } from '../../ImportPrematchHelperNative';

jest.mock('../../app/store');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));
jest.mock('../../ImportPrematchHelperNative');

const mockDispatch = jest.fn();
const setState = jest.fn();
const ref = { current: { value: 'Hello' } };
const mockCallBack = jest.fn();
const setup = isExpandAll => {
  const state = {
    dayShift: {
      isExpandAll,
      leagues: []
    },
    policy: {
      permission: {
        ActorId: 8
      }
    }
  };
  useState.mockImplementation(init => [init, setState]);
  useDispatch.mockReturnValue(mockDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));

  createRef.mockReturnValue(ref);
  get.mockResolvedValue({
    LeagueIds: 1,
    TodayLeagueCount: 3,
    UnSupportedLeagueNames: 'Liam'
  });
};
const createProps = (
  columnKey,
  columnSorting,
  orderType = 'ORDER_DESC',
  isRedirect = false
) => {
  return {
    columnKey: columnKey,
    headerName: 'LeagueName',
    color: 'none',
    onOrder: { mockCallBack },
    sortState: {
      columnKey: columnSorting,
      orderType: orderType
    },
    isRedirect
  };
};

describe('OrderHeader under test', () => {
  beforeEach(() => {});
  it('should render sort-icon with order desc', () => {
    setup('ORDER_DESC');
    const props = createProps();
    const { container } = render(<OrderHeader {...props} />);
    const element = container.querySelector('.sort-icon');
    expect(element).toBeInTheDocument();
  });

  it('when click cell with order asc, should Dispatch 1 action , ', () => {
    setup(true);
    const props = createProps('LeagueName', 'LeagueName', 'ORDER_ASC', 1);
    const { container } = render(<OrderHeader {...props} />);
    const input = container.querySelector('.sort-icon');
    fireEvent.click(input);
    expect(mockDispatch).toHaveBeenCalledTimes(2);
  });
  it('when click cell, should Dispatch 1 action , ', () => {
    setup(true);
    const props = createProps('LeagueNameTest', 1, true, 1);
    const { container } = render(<OrderHeader {...props} />);
    const input = container.querySelector('.sort-icon');
    fireEvent.click(input);
    expect(mockDispatch).toHaveBeenCalledTimes(4);
  });

  it('should render sort-icon, highlight state sorting  if column is sorting ORDER_DESC', () => {
    setup();
    const props = createProps('No', 'No');
    const { container } = render(<OrderHeader {...props} />);
    const element = container.querySelector('.sort-icon');
    expect(element.className).toEqual('sort-icon ORDER_DESC');
  });

  it('should render sort-icon, highlight state sorting  if column is sorting ORDER_ASC', () => {
    setup();
    const props = createProps('No', 'No', 'ORDER_ASC');
    const { container } = render(<OrderHeader {...props} />);
    const element = container.querySelector('.sort-icon');
    expect(element.className).toEqual('sort-icon ORDER_ASC');
  });
  it('should render sort-icon, highlight state sorting  if column is sorting ORDER_DESC', () => {
    setup();
    const props = createProps('No', 'No', 'ORDER_DESC');
    const { container } = render(<OrderHeader {...props} />);
    const element = container.querySelector('.sort-icon');
    expect(element.className).toEqual('sort-icon ORDER_DESC');
  });
  it('should render sort-icon, do not highlight state sorting  if column is not sorting ', () => {
    setup();
    const props = createProps('No', 'League');
    const { container } = render(<OrderHeader {...props} />);
    const element = container.querySelector('.sort-icon');
    expect(element.className).toEqual('sort-icon ');
  });
  it('should render hyperlink open prematch trading page', () => {
    setup();
    const props = createProps('No', 'League', 'ORDER_DESC', true);
    const { container } = render(<OrderHeader {...props} />);
    const element = container.querySelector('.link-prematch');
    expect(element.className).toEqual('link-prematch');
  });

  it('should not render hyperlink open prematch trading page', () => {
    setup();
    const props = createProps('No', 'League', 'ORDER_DESC', false);
    const { container } = render(<OrderHeader {...props} />);
    const element = container.querySelector('.header-name');
    expect(element.className).toEqual('header-name');
  });
});
