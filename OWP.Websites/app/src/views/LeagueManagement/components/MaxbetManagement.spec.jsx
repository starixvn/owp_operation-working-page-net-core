import { render } from '@testing-library/react';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector, useStore } from 'react-redux';

import { BrowserRouter } from 'react-router-dom';
import MaxbetConst from '../constants/MaxbetConst';
import MaxbetManagement from './MaxbetManagement';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  useEffect: jest.fn(),
  createRef: jest.fn()
}));

const stableDispatch = jest.fn();
const setState = jest.fn();
const mockDispatch = jest.fn();
const mockedEffect = jest.fn();

const setup = (policyPending, navigationId = MaxbetConst.maxbetInfo.id) => {
  const state = {
    policyReducer: {
      policyPending: policyPending
    }
  };
  useState.mockImplementation(init => [navigationId, setState]);
  useDispatch.mockReturnValue(mockDispatch);
  useEffect.mockReturnValue(stableDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
  useEffect.mockImplementationOnce(mockedEffect);
};

describe('mount under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('when waiting render, should show Loading', () => {
    setup(true);
    const { container } = render(<BrowserRouter> <MaxbetManagement /></BrowserRouter>);
    const element = container.querySelectorAll('.loading-process');
    expect(element.length).toEqual(1);
  });
  it('should have 1 Switch', () => {
    setup(false);
    const { getByText } = render(<BrowserRouter> <MaxbetManagement /></BrowserRouter>);

    expect(getByText('League Details')).toBeInTheDocument();
  });
  it('should render 1 SpecialProduct, when choose SpecialLiveProduct page', () => {
    useStore.mockReturnValue({
      getState: jest.fn().mockReturnValue({})
    });
    useSelector.mockImplementation(selectorFn =>
      selectorFn({
        policyReducer: {
          policyPending: false
        }
      })
    );
    useState.mockImplementation(init => [MaxbetConst.maxbetInfo.id, setState]);
    const { getByText } = render(<BrowserRouter> <MaxbetManagement /></BrowserRouter>);
    expect(getByText('Special Live Product')).toBeInTheDocument();
  });
});
