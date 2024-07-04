import {
  render,
  screen,
  fireEvent,
  waitForElementToBeRemoved,
  waitFor
} from '@testing-library/react';
import { useDispatch } from 'react-redux';
import FilterTicketCenter from './Filter';
import { getTicketTypeOption } from './filterThunkNative';
import { getEventTickets } from '../../table/slice/ticketSliceThunkNative';
import '@testing-library/jest-dom/extend-expect';
import ToastrWrapper from '../../../../components/ToastrWrapper';
import TicketType from './TicketType';
import {useAppSelector, useAppDispatch} from '../../../app/hooksNative'

jest.mock('axios');
jest.mock('./filterThunkNative');
jest.mock('../../../../components/ToastrWrapper');
jest.mock('../../table/slice/ticketSliceThunkNative',()=>({
  getEventTickets: jest.fn()
}));
getEventTickets.mockReturnValue(fn=>({}))
jest.mock('react-redux', () => ({
  useDispatch: jest.fn(),
  createSelectorHook: jest.fn()
}));
jest.mock('../../../app/hooksNative', () => ({
  useDispatch: jest.fn(),
  useAppSelector: jest.fn(),
  useAppDispatch: jest.fn()
}));

jest.mock('./TicketType', () => {
  return jest.fn(() => null);
});

ToastrWrapper.showError = jest.fn().mockReturnValue('OK');

const mockedDispatch = jest.fn();
useAppSelector.mockReturnValue(fn=>({
  filter:{
    ticketTypeOptionReady: true
  }
}))
useDispatch.mockReturnValue(mockedDispatch);
const props = {
  defaultValue: { ActorId: 8, MarketKey: 1, TicketStatuses: 'liam' }
};
describe('test FilterTicketCenter', () => {
  beforeEach(() => {
    getTicketTypeOption.mockResolvedValueOnce({
      parents: [1, 2, 3],
      children: [26, 7, 8]
    });
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render Loading', async () => {
    const { container } = render(<FilterTicketCenter {...props} />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    await waitFor(() => expect(icon).toBeInTheDocument());
  });

  it('show list Filter ', async () => {
    const { container } = render(<FilterTicketCenter {...props} />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    await waitForElementToBeRemoved(icon);

    expect(screen.queryByText(/Event/i)).toBeInTheDocument();
    expect(screen.queryByText(/Ticket ID/i)).toBeInTheDocument();
    expect(screen.queryByText(/Ticket Type - Subtype/i)).toBeInTheDocument();
    expect(screen.queryByText(/Status/i)).toBeInTheDocument();
    expect(screen.queryByText(/Search/i)).toBeInTheDocument();
  });

  it('click Search, should call  dispatch 4 times', async () => {
    const { container } = render(<FilterTicketCenter {...props} />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    expect(mockedDispatch).toHaveBeenCalledTimes(0);
    await waitForElementToBeRemoved(icon);
    const input = await container.querySelector(
      `[class="input-text-ticketId"]`
    );
    const inputEvent = await container.querySelector(
      `[class="input-text-event"]`
    );
    fireEvent.change(inputEvent, { target: { value: 'ac' } });
    fireEvent.change(input, { target: { value: '1' } });

    expect(mockedDispatch).toHaveBeenCalledTimes(2);
    await fireEvent.click(screen.queryByText(/Search/i));
    expect(MarketFilter).toHaveBeenCalled();
    expect(TicketType).toHaveBeenCalled();
    expect(mockedDispatch).toHaveBeenCalledTimes(4);
  });

  it('click Search, show  showError select at least one option Status', async () => {
    const props = {
      defaultValue: { ActorId: 8, MarketKey: 1, TicketStatuses: null }
    };
    const { container } = render(<FilterTicketCenter {...props} />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    expect(mockedDispatch).toHaveBeenCalledTimes(0);
    await waitForElementToBeRemoved(icon);
    expect(mockedDispatch).toHaveBeenCalledTimes(2);
    await fireEvent.click(screen.queryByText(/Search/i));
    expect(mockedDispatch).toHaveBeenCalledTimes(2);
    expect(ToastrWrapper.showError).toBeCalledTimes(1);
  });

  it('click Search, show  showError at least one option for Ticket Type or Subtype', async () => {
    getTicketTypeOption.mockRestore();
    getTicketTypeOption.mockResolvedValueOnce({
      parents: [],
      children: []
    });
    const { container } = render(<FilterTicketCenter {...props} />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    expect(mockedDispatch).toHaveBeenCalledTimes(0);
    await waitForElementToBeRemoved(icon);
    expect(mockedDispatch).toHaveBeenCalledTimes(2);
    await fireEvent.click(screen.queryByText(/Search/i));
    expect(mockedDispatch).toHaveBeenCalledTimes(2);
    expect(ToastrWrapper.showError).toBeCalledTimes(1);
  });
});
