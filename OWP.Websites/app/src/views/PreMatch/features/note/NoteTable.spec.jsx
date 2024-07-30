import { fireEvent, render } from '@testing-library/react';
import { useSelector, useDispatch, useStore } from 'react-redux';
import NoteTable from './NoteTable';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));

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
const setUp = (hasWrite = true, newCreatedNoteId = false, note = noteData) => {
  useSelector.mockImplementation(selectorFn =>
    selectorFn({
      note: {
        notes: note,
        newCreatedNoteId: newCreatedNoteId
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
const mockState = {
  note: noteData
};
useStore.getState = () => mockState;

const mockSetIsOpening = jest.fn();
const getProps = (
  hasNote = true,
  preMatchNoteId = 1,
  infoSiteMatchId = 123
) => {
  return {
    model: {
      PreMatchNoteId: preMatchNoteId,
      HasNotes: hasNote,
      InfoSiteMatchId: infoSiteMatchId
    },
    setIsOpening: mockSetIsOpening
  };
};

describe('test NotePopover', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('hasWrite=true, should render create-note', () => {
    setUp();
    const { container } = render(<NoteTable {...getProps(true)} />);
    const element = container.querySelector('.create-note');
    expect(element).toBeInTheDocument();
  });

  it('hasWrite=false, should not render create-note', () => {
    setUp(false);
    const { container } = render(<NoteTable {...getProps(true)} />);
    const element = container.querySelector('.create-note');
    expect(element).toBeNull();
  });

  it('hasNote=false, should not render no-data', () => {
    setUp(true, false, []);
    const { container } = render(<NoteTable {...getProps(false)} />);
    const element = container.querySelector('.no-data');
    expect(element).toBeInTheDocument();
  });

  it('hasNote=true, should not render create-note', () => {
    setUp();
    const { container } = render(<NoteTable {...getProps(true)} />);
    const element = container.querySelector('.note-list');
    expect(element).toBeInTheDocument();
  });

  it('addNoteIcon=true, InfoSiteMatchId is valid, should not render create-note', () => {
    setUp(true, true);
    render(<NoteTable {...getProps(true)} />);
    expect(mockedDispatch).toHaveBeenCalledTimes(2);
  });

  it('addNoteIcon=true, InfoSiteMatchId is invalid, should not render create-note', () => {
    setUp(true, true);
    render(<NoteTable {...getProps(true, 1, null)} />);
    expect(mockedDispatch).toHaveBeenCalledTimes(2);
  });

  it('press Enter, value is empty, should not call dispatch', () => {
    setUp();
    const { container } = render(<NoteTable {...getProps(true)} />);
    const element = container.querySelector('.note-textinput');
    fireEvent.keyPress(element, { key: 'Enter' });
    expect(mockedDispatch).toHaveBeenCalledTimes(0);
  });

  it('press Enter, value is valid, should call dispatch', () => {
    setUp();
    const { container } = render(<NoteTable {...getProps(true)} />);
    const element = container.querySelector('.note-textinput');

    fireEvent.change(element, { target: { value: 'abc' } });
    fireEvent.keyPress(element, { key: 'Enter', keyCode: 13 });

    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });

  it('press Enter, value is empty, should not call dispatch', () => {
    setUp();
    const { container } = render(<NoteTable {...getProps(true, null)} />);
    const element = container.querySelector('.note-textinput');

    fireEvent.keyPress(element, { key: 'Enter', keyCode: 13 });

    expect(mockedDispatch).toHaveBeenCalledTimes(0);
  });

  it('press any key, should not call dispatch', () => {
    setUp();
    const { container } = render(<NoteTable {...getProps(true, null)} />);
    const element = container.querySelector('.note-textinput');

    fireEvent.keyPress(element, { key: 'ESC' });

    expect(mockedDispatch).toHaveBeenCalledTimes(0);
  });
});
