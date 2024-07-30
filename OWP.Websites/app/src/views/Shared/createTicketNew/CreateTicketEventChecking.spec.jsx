import { useSelector } from 'react-redux';
import CreateTicketEventChecking from './CreateTicketEventChecking';
import { render } from '@testing-library/react';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  createRef: jest.fn()
}));
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
const setup = (isEventChecking, valueEvent, valueSearch, typeTicketText) => {
  const initialState = {
    createTicketBoard: {
      isEventChecking,
      valueEvent,
      valueSearch,
      typeTicketText
    }
  };
  useSelector.mockImplementation(selectorFn => selectorFn(initialState));
};

jest.mock('../../Shared/customHook/useStableDispatch', () => () => jest.fn())

describe('CreateTicketEventChecking under test', () => {
  it('Should render popup', async () => {
    setup(true, valueEvent, 'sad', 'Event Checking');

    const { container } = render(
      <CreateTicketEventChecking
        creatorInfo={{ ...recipientByCreatorId,
          userName:'sa'}}
        handleBuildModel={handleBuildModel}
        ticketType={2}
      />
    );

    const popup = container.querySelector('.chat-board-popup');
    expect(popup).toBeTruthy();
  });
});
