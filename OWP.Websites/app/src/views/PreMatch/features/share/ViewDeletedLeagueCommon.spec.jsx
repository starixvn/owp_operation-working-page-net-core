import { useState } from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import { useSelector } from 'react-redux';
import ViewDeletedLeagueCommon from './ViewDeletedLeagueCommon';
import DeletedLeagueDayShift from '../deletedLeague/DeletedLeagueDayShift';
import { REQUEST_STATUS } from '../../../Shared/Constants';

const createProps = isDay => {
  return { isDay, children: <DeletedLeagueDayShift /> };
};
jest.mock('../../app/store');
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
const setState = jest.fn();

const setup = requestState => {
  const stateStore = {
    filter: {
      filterDate: new Date()
    },
    deletedLeague: {
      loading: requestState
    }
  };
  useState.mockImplementation(init => [init, setState]);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...stateStore }));
};
describe('ViewDeletedLeagueCommon under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render ModalCommon', () => {
    setup(REQUEST_STATUS.idle);
    const props = createProps(true);
    const { container } = render(<ViewDeletedLeagueCommon {...props} />);
    const element = container.querySelector('.deleted-league');
    expect(element).toBeInTheDocument();
  });
  it('should handle render icon Loading', () => {
    setup(REQUEST_STATUS.pending);
    const props = createProps(true);
    const { container } = render(<ViewDeletedLeagueCommon {...props} />);
    const element = container.querySelector('.loading-process');
    expect(element).toBeInTheDocument();
  });
});
