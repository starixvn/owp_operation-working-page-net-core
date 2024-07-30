import { fireEvent, render } from '@testing-library/react';
import NoteCell from './NoteCell';
import { useSelector, useDispatch } from 'react-redux';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockedDispatch = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);
useSelector.mockImplementation(selectorFn =>
  selectorFn({
    note: {
      notes: []
    },
    filter: {
      filterDate: new Date()
    },
    policy: {
      permission: {
        HasWrite: true
      }
    }
  })
);

const handleShowNote = jest.fn();

const getProps = (hasNotes, infoSiteMatchId = 123, isOpenNote = false) => {
  return {
    dataSource: [
      {
        isOpenNote: isOpenNote,
        InfoSiteMatchId: infoSiteMatchId,
        HasNotes: hasNotes
      }
    ],
    rowIndex: 0,
    handleShowNote: handleShowNote
  };
};

describe('test NoteCell', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('HasNotes=true, should render CommentIcon', () => {
    const { container } = render(<NoteCell {...getProps(true)} />);
    const element = container.querySelector('.icon-comment');
    expect(element).toBeInTheDocument();
  });

  it('HasNotes=false, should render EditIcon', () => {
    const { container } = render(<NoteCell {...getProps(false)} />);
    const element = container.querySelector('.icon-edit');
    expect(element).toBeInTheDocument();
  });

  it('should not render NotePopover when isOpenNote=false', () => {
    const { container } = render(<NoteCell {...getProps(true, 123, false)} />);
    const element = container.querySelector('.note-popover');
    expect(element).not.toBeInTheDocument();
  });

  it('should render NotePopover when isOpenNote=true', () => {
    const { container } = render(<NoteCell {...getProps(true, 123, true)} />);
    const element = container.querySelector('.note-popover');
    expect(element).toBeInTheDocument();
  });

  it('click icon comment, infoSiteMatchId=null, should call dispatch 2 times', () => {
    const { container } = render(<NoteCell {...getProps(true, null)} />);
    const element = container.querySelector('.icon-comment');
    expect(mockedDispatch).toHaveBeenCalledTimes(0);
    fireEvent.click(element);
    expect(mockedDispatch).toHaveBeenCalledTimes(3);
  });

  it('click icon comment, infoSiteMatchId is valid, should call dispatch 2 times', () => {
    const { container } = render(<NoteCell {...getProps(true, 123)} />);
    const element = container.querySelector('.icon-comment');
    expect(mockedDispatch).toHaveBeenCalledTimes(0);
    fireEvent.click(element);
    expect(mockedDispatch).toHaveBeenCalledTimes(3);
  });
});
