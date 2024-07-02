import { useState } from 'react';
import ActionWrapper from './ActionWrapper';
import { useDispatch, useSelector } from 'react-redux';
import { fireEvent, render, screen } from '@testing-library/react';
import Spinner from '../../../components/Spinner';
const mockDispatch = jest.fn();

jest.mock('react-redux', () => ({
  useDispatch: jest.fn(),
  useSelector: jest.fn()
}));

jest.mock('../../../components/Spinner');

Spinner.show = jest.fn().mockReturnValue('OK');

const ticketType = {
  typeId: 1,
  typeName: 'sa'
};

const recipients = [];

const subtypes = [
  { typeId: 1, subtypeId: 0, subtypeName: 'sa1' },
  { typeId: 1, subtypeId: 61, subtypeName: 'sa2' }
];
const props = {
  ticketType,
  recipients,
  subtypes
};

const setup = () => {
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn({
      policy: {
        permission: { Username: 'sa', UserId: 8, HasWrite: true, SiteId: 99 }
      }
    })
  );
};

describe('ActionCell under test', () => {
  beforeEach(() => {
    setup();
  });
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should match snapshot', () => {
    const { container } = render(<ActionWrapper {...props} />);
    expect(container).toMatchSnapshot();
    const element = container.querySelector(`[class="icon-edit"]`);
    expect(element).toBeInTheDocument();
  });

  it('should open Popup', async () => {
    const { container } = render(<ActionWrapper {...props} />);
    const element = container.querySelector(`[class="icon-edit"]`);
    fireEvent.click(element!);
    const button = await screen.findByText('Save');
    fireEvent.click(button!);
    expect(element).toBeInTheDocument();
    expect(useDispatch).toBeCalled();
    expect(Spinner.show).toBeCalled();
  });
});
