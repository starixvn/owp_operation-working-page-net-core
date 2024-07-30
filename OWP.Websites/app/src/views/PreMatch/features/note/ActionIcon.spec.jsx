import { fireEvent, render } from '@testing-library/react';
import { useDispatch, useSelector, useStore } from 'react-redux';
import ToastrWrapper from '../../../components/ToastrWrapper';
import ActionIcon from './ActionIcon';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));
jest.mock('../../../components/ToastrWrapper');

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);

const noteData = [
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
];
const setUp = (hasWrite = true, note = noteData, toastrWrapperValue = true) => {
  const object = { value: toastrWrapperValue };
  ToastrWrapper.confirm.mockImplementation(() => object);
  useSelector.mockImplementation(selectorFn =>
    selectorFn({
      note: {
        notes: note
      },
      policy: {
        permission: {
          HasWrite: hasWrite
        }
      }
    })
  );
  useStore.mockImplementation(() => {
    return {
      getState: jest.fn().mockImplementation(() => {
        return {
          note: {
            notes: note
          }
        };
      })
    };
  });
};

const mockSetIsEditing = jest.fn();

const getProps = (InfoSiteMatchId = 123) => {
  return {
    row: {
      PreMatchNoteId: 123,
      PreMatchNoteMessageId: 222,
      Username: 'starix1',
      LAST_UPDATED: new Date(),
      Note: 'test222222'
    },
    model: {
      InfoSiteMatchId,
      LeagueId: 222
    },
    setIsEditing: mockSetIsEditing
  };
};

describe('test NotePopover', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('click icon-edit, should call setIsEditing', () => {
    setUp();
    const { container } = render(<ActionIcon {...getProps()} />);
    const editIcon = container.querySelector('.icon-edit');
    fireEvent.click(editIcon);
    expect(mockSetIsEditing).toHaveBeenCalledTimes(1);
  });

  it('click icon-delete, note length = 0,click Yes ToastrWrapper, should call dispatch 1 times', async () => {
    setUp(true, []);
    const { container } = render(<ActionIcon {...getProps()} />);
    const deleteIcon = container.querySelector('.icon-delete');
    await fireEvent.click(deleteIcon);
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });

  it('click icon-delete, note length != 0,click Yes ToastrWrapper, should call dispatch 1 times', async () => {
    setUp();
    const { container } = render(<ActionIcon {...getProps(null)} />);
    const deleteIcon = container.querySelector('.icon-delete');
    await fireEvent.click(deleteIcon);
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });
  it('click icon-delete, and click cancel ToastrWrapper, nothing happen', async () => {
    setUp(true, noteData, false);
    const { container } = render(<ActionIcon {...getProps(null)} />);
    const deleteIcon = container.querySelector('.icon-delete');
    await fireEvent.click(deleteIcon);
    expect(mockedDispatch).toHaveBeenCalledTimes(0);
  });
});
