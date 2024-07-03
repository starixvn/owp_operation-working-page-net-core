import Filter from './Filter';
import Service from '../ResourceManagement/Service';
import Utils from '../Shared/Utils';
import { act, fireEvent, render, waitFor } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
const middleWares = [];
const mockStore = configureStore(middleWares);
const store = mockStore({
  chatBoard: { modelByRowIndex: {} },
  createTicketBoard: {
    selectedRecipients: {},
    subTypeSelected: null,
    isOpenCreateTicketInLGA: true
  },
  tableReducer: {
    dataEvent: []
  }
});
jest.mock('../ResourceManagement/Service');
jest.mock('../Shared/Utils');

const responseData = [
  { Id: 1, Value: 'Live', RefName: 'MatchStatus' },
  { Id: 2, Value: 'In-play', RefName: 'MatchStatus' },
  { Id: 3, Value: 'Completed', RefName: 'MatchStatus' },
  { Id: 4, Value: 'Cancel', RefName: 'MatchStatus' },
  { Id: 5, Value: 'Postponed', RefName: 'MatchStatus' },
  { Id: 6, Value: 'Suspended', RefName: 'MatchStatus' }
];
const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('Filter', () => {
  const onSubmit = jest.fn();
  const onChangeDate = jest.fn();
  const defaultProps = {
    submit: onSubmit,
    onChangeDate: onChangeDate,
    isHostEventPicking: true,
    lastUpdated: 'mock',
    isAllowAddEvent: true,
    onToggleModal: jest.fn(),
    matchStatuses: [
      { RefName: 'MatchStatus', Id: 4, Value: 'Cancel' },
      { RefName: 'MatchStatus', Id: 3, Value: 'Completed' },
      { RefName: 'MatchStatus', Id: 2, Value: 'In-play' },
      { RefName: 'MatchStatus', Id: 1, Value: 'Live' },
      { RefName: 'MatchStatus', Id: 5, Value: 'Postponed' },
      { RefName: 'MatchStatus', Id: 6, Value: 'Suspended' }
    ],
    hasWrite: true,
    offVRs:[
      {
        Value: 'off'
      }
    ]
  };
  Utils.buildMatchStatusOption.mockReturnValue([
    { label: 'In-play', value: 2 },
    { label: 'Live', value: 1 }
  ]);
  Utils.buildOffVROption.mockReturnValue([
    { label: 'In-play', value: 2 },
    { label: 'Live', value: 1 }
  ]);
  Utils.buildSelectedMatchStatus.mockReturnValue([1, 2]);

  it('should invoke submit when component loaded', () => {
    onSubmit.mockReset();
    Service.getByRefName.mockReturnValue(responseData);
    render(
      <Wrapper>
        <Filter {...defaultProps} />
      </Wrapper>
    );
    expect(onSubmit).toBeCalledTimes(1);
  });

  it('should display Match Status when component loaded', () => {
    const { container } = render(
      <Wrapper>
        <Filter {...defaultProps} />
      </Wrapper>
    );
    expect(container.getElementsByClassName('multiselect').length).toEqual(4);
  });

  it('should update state when component loaded', () => {
    Service.getByRefName.mockReturnValue(responseData);
    const { getAllByText } = render(
      <Wrapper>
        <Filter {...defaultProps} />
      </Wrapper>
    );
    expect(getAllByText(/All items are selected/i)).toBeTruthy();
  });

  it('should display Match Status when component loaded', () => {
    const { container } = render(
      <Wrapper>
        <Filter {...defaultProps} />
      </Wrapper>
    );
    expect(container.getElementsByClassName('multiselect').length).toEqual(4);
  });

  it('should update state when select match status', () => {
    const { container, getByText } = render(
      <Wrapper>
        <Filter {...defaultProps} />
      </Wrapper>
    );

    fireEvent.click(container.querySelector('.dropdown-heading-value'));
   
    waitFor(() => {
      expect(getByText('Live')).toBeInTheDocument();
    });
  });

  it('should update match status when change event date', async () => {
    Service.getByRefName.mockReturnValue(responseData);
    Utils.isSameOrAfter.mockReturnValue(true);
    Utils.handleOnDateChange.mockReturnValue([1, 2]);

    const { container, getByText } = render(
      <Wrapper>
        <Filter {...defaultProps} />
      </Wrapper>
    );

    fireEvent.click(container.querySelector('.dropdown-heading-value'));
  
    fireEvent.click(container.querySelector('.icon-calendar'));
   
    await act(async () => {
      await Promise.resolve();
    });
    expect(getByText('All')).toBeInTheDocument();
  });
});
