import { fireEvent, render } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import NotePopover from './NotePopover';

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

const mockSetIsOpening = jest.fn();
const getProps = () => {
  return {
    model: {
      PreMatchNoteId: 1
    },
    setIsOpening: mockSetIsOpening
  };
};

describe('test NotePopover', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('click close button, should call mockSetIsOpening', () => {
    const { container } = render(<NotePopover {...getProps()} />);
    const button = container.querySelector('.close-popup');

    fireEvent.click(button);

    expect(mockSetIsOpening).toHaveBeenCalledTimes(1);
  });
});
