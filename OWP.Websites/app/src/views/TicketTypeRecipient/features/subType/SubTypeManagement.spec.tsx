import SubTypeManagement from './SubTypeManagement';
import { fireEvent, render } from '@testing-library/react';
import useStore from '../../app/zustand/ticketTypeStore';

const setSubtypeSelected = jest.fn();
const setSubtypes = jest.fn();
const setRecipients = jest.fn();
useStore.setState({
  subtypes: [
    { subtypeId: 85, typeId: 1, subtypeName: 'Sub1' },
    { subtypeId: 86, typeId: 1, subtypeName: 'Sub2' }
  ],
  setSubtypes: setSubtypes,
  setSubtypeSelected: setSubtypeSelected,
  setRecipients: setRecipients,
  recipients: [
    {
      recipientNickname: 'sa',
      recipientId: 8,
      recipientName: 'sa',
      recipientType: 1,
      subtypeName: 'Sub1',
      ticketTypeRecipientId: 2074
    },
    {
      recipientNickname: 'sa2',
      recipientId: 124,
      recipientName: 'sa2',
      recipientType: 1,
      subtypeName: 'Sub2',
      ticketTypeRecipientId: 2075
    }
  ]
});
const subtypesDefault = [{ typeId: 1, subtypeId: 0, subtypeName: 'sa1' }];
const ticketTypeId = 1;
const props = {
  subtypesDefault,
  ticketTypeId
};

describe('SubTypeManagement under test', () => {
  it('should render', () => {
    const { container } = render(<SubTypeManagement {...props} />);
    const input = container.querySelector(`[class="subtype-text-input"]`);
    fireEvent.change(input!, { target: { value: '12345' } });
    fireEvent.focusOut(input!);
    const submit = container.querySelector(`[class="button button-add"]`);
    fireEvent.click(submit!);
    expect(input).toBeInTheDocument();
    expect(submit).toBeInTheDocument();
  });
  it('should handle update', () => {
    const props = {
      ticketTypeId: 1
    };

    const { container } = render(<SubTypeManagement {...props} />);
    const input = container.querySelectorAll(`[class="subtype-text-input"]`);
    fireEvent.change(input[1]!, { target: { value: '12345' } });
    fireEvent.focusOut(input[1]!);
  });

  it('should not handle update', () => {
    const props = {
      ticketTypeId: 1
    };

    const { container } = render(<SubTypeManagement {...props} />);
    const input = container.querySelectorAll(`[class="subtype-text-input"]`);
    fireEvent.change(input[1]!, { target: { value: 'Sub2' } });
    fireEvent.focusOut(input[1]!);
  });

  it('should handle change subtype', () => {
    const props = {
      ticketTypeId: 1
    };

    const { container } = render(<SubTypeManagement {...props} />);
    const input = container.querySelectorAll(`[class="subtype-text-input"]`);
    fireEvent.click(input[2]!);
  });
});
