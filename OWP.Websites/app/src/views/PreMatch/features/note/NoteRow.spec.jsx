import { fireEvent, render } from '@testing-library/react';
import { useSelector, useDispatch, useStore } from 'react-redux';
import NoteRow from './NoteRow';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));

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

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);

const setUp = (hasWrite = true, note = noteData) => {
  useSelector.mockImplementation(selectorFn =>
    selectorFn({
      note: {
        notes: note
      },
      filter: {
        filterDate: new Date()
      },
      policy: {
        permission: {
          HasWrite: hasWrite
        }
      }
    })
  );
};

const mockSetIsOpening = jest.fn();
const getProps = (hasWrite = true, createdDate = new Date()) => {
  return {
    row: {
      PreMatchNoteId: 123,
      PreMatchNoteMessageId: 222,
      Username: 'starix1',
      CreatedDate: createdDate,
      Note: 'test222222'
    },
    setIsOpening: mockSetIsOpening,
    permission: {
      HasWrite: hasWrite
    },
    model: {
      InfoSiteMatchId: 123,
      LeagueId: 222
    }
  };
};
const mockState = {
  note: noteData
};
useStore.getState = () => mockState;

describe('test NotePopover', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('render NoteRow, hasWrite=true, should render ActionIcon', () => {
    setUp(true);
    const { container } = render(<NoteRow {...getProps(true)} />);
    expect(container.querySelector('.content')).toBeInTheDocument();
    expect(container.querySelector('.icon-edit')).toBeInTheDocument();
  });

  it('hasWrite=false, should not render button action', () => {
    setUp();
    const { container } = render(<NoteRow {...getProps(false)} />);

    expect(container.querySelector('.icon-edit')).not.toBeInTheDocument();
  });

  it('click icon-edit, should render content-edit and ConfirmIcon', () => {
    setUp();
    const { container } = render(<NoteRow {...getProps(true)} />);
    const iconEdit = container.querySelector('.icon-edit');
    fireEvent.click(iconEdit);
    expect(container.querySelector('.content-edit')).toBeInTheDocument();

    expect(container.querySelector('.icon-save')).toBeInTheDocument();
  });
});
