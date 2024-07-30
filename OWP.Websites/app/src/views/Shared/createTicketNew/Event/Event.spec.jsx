import { useState } from 'react';
import InputEvent from './Event';
import { useSelector, useDispatch } from 'react-redux';
import { render } from '@testing-library/react';
import Service from '../Service';

const setState = jest.fn();
const mockDispatch = jest.fn();
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  createRef: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));
jest.mock('lodash/debounce', () => jest.fn(fn => fn));
jest.mock('../Service');
const fetchData = {
  data: [
    {
      owpLeagueId: 32459,
      leagueName: '*UEFA EUROPA LEAGUE',
      homeId: 123,
      awayId: 141,
      homeName: 'A',
      awayName: 'B',
      eventDateTimeGmt8: '2021-10-01T00:45:00',
      isLeague: false
    },
    {
      owpLeagueId: 12312,
      leagueName: '*UEFA EUROPA LEAGUE',
      homeId: 0,
      awayId: 0,
      homeName: null,
      awayName: null,
      eventDateTimeGmt8: '2021-10-01T00:45:00',
      isLeague: true
    }
  ]
};
const setup = (valueSearch = 'sa') => {
  const initialState = {
    createTicketBoard: { valueSearch: valueSearch }
  };
  useState.mockImplementation(init => [init, setState]);
  useSelector.mockImplementation(selectorFn => selectorFn(initialState));
  useDispatch.mockReturnValue(mockDispatch);
};
describe('Event under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should render event from API', () => {
    setup();
    const props = {
      typeTicketText: 'Event Checking',
      ticketType: 1
    };
    Service.event.mockResolvedValue(fetchData);
    const { container } = render(<InputEvent {...props} />);
    expect(container.querySelector('#tags-filled')).toBeInTheDocument();
  });
});
