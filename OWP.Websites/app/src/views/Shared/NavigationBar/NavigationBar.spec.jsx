import { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { render, screen } from '@testing-library/react';
import NavigationBar from './NavigationBar';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
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

const setup = policyPending => {
  const state = {
    policyReducer: {
      policyPending: policyPending
    }
  };
  useState.mockImplementation(init => [init, setState]);
  useDispatch.mockReturnValue(mockDispatch);
  useEffect.mockReturnValue(stableDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
  useEffect.mockImplementationOnce(mockedEffect);
};

describe('mount under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should render 1 navigation bar', () => {
    setup(true);
    const props = {
      navbarList: [
        {
          id: 'create-tab',
          label: 'Create'
        }
      ],
      activeTabId: 'create-tab'
    };
    const { container } = render(<NavigationBar {...props} />);
    const element = container.querySelector('.navigation-bar-item');
    expect(element).toBeInTheDocument();
  });
});
