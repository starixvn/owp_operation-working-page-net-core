import { useState, useEffect } from 'react';
import { fireEvent, render } from '@testing-library/react';
import LeagueCell from './LeagueCell';
import { useDispatch, useSelector } from 'react-redux';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  useEffect: jest.fn()
}));
jest.mock('../../../../app/store');

const mockDispatch = jest.fn();
const setState = jest.fn();

const setup = isExpandAll => {
  const state = {
    dayShift: {
      isExpandAll
    }
  };
  useState.mockImplementation(init => [init, setState]);
  useDispatch.mockReturnValue(mockDispatch);
  useEffect.mockReturnValue(setState);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
};
const createProps = (LeagueName, PreMatchLeagueId, isExpand, MatchCount) => {
  const dataSource = [
    {
      LeagueName,
      PreMatchLeagueId,
      isExpand,
      MatchCount
    }
  ];
  const rowIndex = 0;
  return { dataSource, rowIndex };
};

describe('LeagueCell under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render league-row', () => {
    setup(true);
    const props = createProps('LeagueNameTest', 1, true, 1);
    const { container } = render(<LeagueCell {...props} />);
    const element = container.querySelector('.league-row');
    expect(element).toBeInTheDocument();
  });

  it('when isExpand = false, should render icon expand `►` ', () => {
    setup(true);
    const props = createProps('LeagueNameTest', 1, false, 1);
    const { container } = render(<LeagueCell {...props} />);
    const icon = container.querySelector('.icon-expand');
    expect(icon.textContent).toEqual('►');
  });

  it('when isExpand = true, should render icon expand `▼` ', () => {
    setup(false);
    const props = createProps('LeagueNameTest', 1, true, 1);
    const { container } = render(<LeagueCell {...props} />);
    const icon = container.querySelector('.icon-expand');
    expect(icon.textContent).toEqual('▼');
  });

  it('when isExpand = true, should dispatch action collapseOneLeague  ', () => {
    setup(false);
    const props = createProps('LeagueNameTest', 1, true, 1);
    const { container } = render(<LeagueCell {...props} />);
    const icon = container.querySelector('.icon-expand');
    fireEvent.click(icon);
    const dispatchAction = { payload: 1, type: 'dayShift/collapseOneLeague' };
    expect(mockDispatch).toHaveBeenCalledTimes(1);
    expect(mockDispatch).toBeCalledWith(dispatchAction);
  });

  it('when isExpand = false, should dispatch action expandOneLeague  ', () => {
    setup(false);
    const props = createProps('LeagueNameTest', 1, false, 1);
    const { container } = render(<LeagueCell {...props} />);
    const icon = container.querySelector('.icon-expand');
    fireEvent.click(icon);
    const dispatchAction = { payload: 1, type: 'dayShift/expandOneLeague' };
    expect(mockDispatch).toBeCalledTimes(1);
    expect(mockDispatch).toBeCalledWith(dispatchAction);
  });
});
