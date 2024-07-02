import { fireEvent, render, screen } from '@testing-library/react';
import Dialog from './Dialog';
import useStore from '../../app/zustand/ticketTypeStore';
import { useDispatch, useSelector } from 'react-redux';
jest.mock('react-redux', () => ({
  useDispatch: jest.fn(),
  useSelector: jest.fn()
}));
const mockDispatch = jest.fn();
const setRecipients = jest.fn();
useStore.setState({
  recipients: [
    {
      recipientNickname: 'sa',
      recipientId: 8,
      recipientName: 'sa',
      recipientType: 1,
      subtypeName: 'Sub1',
      ticketTypeRecipientId: 2074
    }
  ],
  subtypeSelected: { subtypeId: 85, typeId: 1, subtypeName: 'Sub1' },
  setRecipients: setRecipients
});
(useSelector as jest.Mock).mockImplementation(selectorFn =>
  selectorFn({
    policy: {
      permission: { Username: 'sa', UserId: 8, HasWrite: true, SiteId: 99 }
    }
  })
);
(useDispatch as jest.Mock).mockReturnValue(mockDispatch);
describe('Command under test', () => {
  it('should render Command', () => {
    const handleOnClose = jest.fn();
    const handleOnSave = jest.fn();
    const props = {
      onClose: handleOnClose,
      onSave: handleOnSave,
      subtypes: [{ subtypeId: 85, typeId: 1, subtypeName: 'Sub1' }],
      ticketType: {
        typeId: 0,
        typeName: ''
      },
      recipients: [
        {
          ticketTypeRecipientId: 2074,
          recipientId: 8,
          recipientType: 1,
          recipientName: 'sa',
          subtypeName: 'Sub1',
          recipientNickname: 'sa'
        }
      ]
    };
    render(<Dialog {...props} />);

    fireEvent.click(screen.getByText('Cancel'));
    fireEvent.click(screen.getByText('Save'));
    expect(handleOnClose).toBeCalled();
    expect(handleOnSave).toBeCalled();
  });
});
