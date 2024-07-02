import { useState, createRef } from 'react';
import { fireEvent, render } from '@testing-library/react';
import EditableRemark from './EditableRemark';
import { useDispatch, useSelector } from 'react-redux';

jest.mock('../../../../app/store');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));

const mockDispatch = jest.fn();
const setState = jest.fn();
const ref = { current: { value: 'Hello' } };

const setup = isExpandAll => {
  const state = {
    dayShift: {
      isExpandAll
    }
  };
  useState.mockImplementation(init => [init, setState]);
  useDispatch.mockReturnValue(mockDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
  createRef.mockReturnValue(ref);
};
const createProps = (LeagueName, PreMatchLeagueId, isExpand, MatchCount) => {
  const dataSource = [
    {
      LeagueName,
      PreMatchLeagueId,
      isExpand,
      MatchCount,
      Remark: 'test'
    }
  ];
  const rowIndex = 0;
  return { dataSource, rowIndex, columnId: 'Remark' };
};

describe('EditableRemark under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render input-field', () => {
    setup(true);
    const props = createProps('LeagueNameTest', 1, true, 1);
    const { container } = render(<EditableRemark {...props} />);
    const element = container.querySelector('.input-field');
    expect(element).toBeInTheDocument();
  });

  it('when iput out focus ,should Dispatch 1 action , ', () => {
    setup(true);
    const props = createProps('LeagueNameTest', 1, true, 1);
    const { container } = render(<EditableRemark {...props} />);
    const input = container.querySelector('input');
    fireEvent.blur(input);
    expect(mockDispatch).toHaveBeenCalledTimes(1);
  });
});
