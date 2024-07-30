import CreateTicketButton from './CreateTicketButton';
import { useSelector, useDispatch } from 'react-redux';
import { screen, render, fireEvent } from '@testing-library/react';

const mockDispatch = jest.fn();
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const setup = HasWrite => {
  const initialState = {
    createTicketButton: {
      isOpenPopup: false,
      isEventChecking: false,
      isOthers: false
    },
    policy: {
      permission: {
        HasWrite: HasWrite
      }
    }
  };

  useSelector.mockImplementation(selectorFn => selectorFn(initialState));
  useDispatch.mockReturnValue(mockDispatch);
};

describe('CreateTicketButton under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should return empty when not allowed to write', () => {
    setup(false);
    const { container } = render(<CreateTicketButton />);
    const buttonBlock = container.querySelector('.btn-block-add-new');
    expect(buttonBlock).toBeInTheDocument();
  });

  it('should dispatch openPopup others when clicked', () => {
    setup(true);
    const { container } = render(<CreateTicketButton buttonType='my-ticket' />);
    const button = container.querySelector('.button-others');
    fireEvent.click(button);
    expect(mockDispatch).toHaveBeenCalledTimes(4);
  });
  it('should dispatch openPopup support when clicked', () => {
    setup(true);
    const { container } = render(<CreateTicketButton buttonType='my-ticket' />);
    const button = container.querySelector('.button-support');
    fireEvent.click(button);
    expect(mockDispatch).toHaveBeenCalledTimes(4);
  });

  it('should dispatch openPopup event when clicked', () => {
    setup(true);
    const { container } = render(
      <CreateTicketButton buttonType='my-ticket'></CreateTicketButton>
    );
    const button = container.querySelector('.button-event');

    fireEvent.click(button);

    expect(mockDispatch).toHaveBeenCalledTimes(4);
  });
  it('should dispatch openPopup live incident when clicked', () => {
    setup(true);
    const { container } = render(
      <CreateTicketButton buttonType='my-ticket'></CreateTicketButton>
    );
    const button = container.querySelector('.button-live-incident');

    fireEvent.click(button);

    expect(mockDispatch).toHaveBeenCalledTimes(4);
  });

  it('should dispatch openPopup log work when clicked', () => {
    setup(true);
    const { container } = render(
      <CreateTicketButton buttonType='my-ticket' isCreateLogWork />
    );
    const button = container.querySelector('.button-work-log');

    fireEvent.click(button);

    expect(mockDispatch).toHaveBeenCalledTimes(2);
  });
});
