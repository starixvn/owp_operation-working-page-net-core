import { useSelector } from 'react-redux';
import CreateTicketLiveIncident from './CreateTicketLiveIncident';
import { render } from '@testing-library/react';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  createRef: jest.fn()
}));

jest.mock('../../Shared/customHook/useStableDispatch', () => () => jest.fn())

const valueEvent = {
  owpMatchId: 869324,
  homeName: 'B',
  awayName: 'C',
  leagueName: 'A'
};
const recipientByCreatorId = {
  ticketTypeRecipientId: 1,
  recipientId: 1,
  recipientType: 1,
  recipientName: 'sa',
  recipientNickname: 'sa_nick'
};
const handleBuildModel = jest.fn();
const setup = (isLiveIncident, valueEvent, valueSearch, typeTicketText) => {
  const initialState = {
    createTicketBoard: {
      isLiveIncident,
      valueEvent,
      valueSearch,
      typeTicketText
    }
  };
  useSelector.mockImplementation(selectorFn => selectorFn(initialState));
};

describe('CreateTicketLiveIncident under test', () => {
  it('Should render popup', async () => {
    setup(true, valueEvent, 'sad', 'Event Checking');

    const { container } = render(
      <CreateTicketLiveIncident
        creatorInfo={{
          ...recipientByCreatorId,
          userName:'sa'
        }}
        handleBuildModel={handleBuildModel}
        ticketType={4}
      />
    );

    const popup = container.querySelector('.chat-board-popup');
    expect(popup).toBeTruthy();
  });
});
