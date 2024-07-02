import { fireEvent, render } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import ConfirmIcon from './ConfirmIcon';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);

const setUp = (hasWrite = true) => {
  useSelector.mockImplementation(selectorFn =>
    selectorFn({
      note: {
        notes: [
          {
            PreMatchNoteId: 123,
            PreMatchNoteMessageId: 111,
            Username: 'sa',
            LAST_UPDATED: new Date(),
            Note: 'test note abc'
          },
          {
            PreMatchNoteId: 123,
            PreMatchNoteMessageId: 222,
            Username: 'starix1',
            LAST_UPDATED: new Date(),
            Note: 'test222222'
          }
        ]
      },
      policy: {
        permission: {
          HasWrite: hasWrite
        }
      }
    })
  );
};

const mockSetIsEditing = jest.fn();
const getProps = {
  row: {
    PreMatchNoteId: 123,
    PreMatchNoteMessageId: 222,
    Username: 'starix1',
    LAST_UPDATED: new Date(),
    Note: 'test222222'
  },
  model: {
    InfoSiteMatchId: 123,
    LeagueId: 222
  },
  setIsEditing: mockSetIsEditing,
  editNoteRef: {
    current: {
      value: ''
    }
  }
};

describe('test NotePopover', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('click icon-cancel, should call setIsEditing', () => {
    setUp();
    const { container } = render(<ConfirmIcon {...getProps} />);
    const cancelIcon = container.querySelector('.icon-cancel');
    fireEvent.click(cancelIcon);
    expect(mockSetIsEditing).toHaveBeenCalledTimes(1);
  });

  it('click icon-save, should call setIsEditing and dispatch', () => {
    setUp();
    const { container } = render(<ConfirmIcon {...getProps} />);
    const saveIcon = container.querySelector('.icon-save');
    fireEvent.click(saveIcon);
    expect(mockSetIsEditing).toHaveBeenCalledTimes(1);
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });
});
