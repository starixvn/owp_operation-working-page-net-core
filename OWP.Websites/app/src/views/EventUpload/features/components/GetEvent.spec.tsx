import { act, fireEvent, render } from '@testing-library/react';
import { createRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import GetEvent from './GetEvent';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));

const mockDispatch = jest.fn();
const mockSetState = jest.fn();
const ref = { current: { value: 'Hello' } };

const setup = () => {
  const state = {
    uploadReducer: {
      request: {
        isUploadFMAndAIO: false,
        eventDate: 'Nov 11, 2022'
      }
    }
  };
  (useState as jest.Mock).mockImplementation(init => [init, mockSetState]);
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useSelector as jest.Mock).mockImplementation(selectorFn => selectorFn({ ...state }));
  (createRef as jest.Mock).mockReturnValue(ref);
};

describe('GetEvent under test', () => {
  beforeEach(() => {
    setup();
  })
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render icon-calendar', () => {
    const { container } = render(<GetEvent />);
    const element = container.querySelector('.icon-calendar');
    expect(element).toBeInTheDocument();
  });

  it('when click icon-calendar button, should call mockSetState', () => {
    const { getByText } = render(<GetEvent />);
    global.window = Object.create(window);
    Object.defineProperty(window, 'location', { value: { href: '' } });
    fireEvent.click(getByText('Download'));

    var url = 'TaskScheduling/EventAssignment/DownloadAssignment?date=Nov 11, 2022';
    expect(window.location.href).toEqual(url);
  });
});
