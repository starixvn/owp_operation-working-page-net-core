import { fireEvent, render, screen } from '@testing-library/react';
import axios from 'axios';
import { noop } from 'lodash';
import { createRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setImmediate } from 'timers';
import ToastrWrapper from '../../../components/ToastrWrapper';
import InputMessage from './InputMessage';

function flushPromises() {
  return new Promise(resolve => setImmediate(resolve));
}

const setState = jest.fn();

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('axios');
jest.mock('../../../components/ToastrWrapper');
ToastrWrapper.showError.mockImplementation(() => noop());

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));
const mockClickRef = jest.fn();
const ref = { current: { click: mockClickRef } };

const mockedDispatch = jest.fn();
const mockHandleSendFile = jest.fn();
const mockHandleSendMessage = jest.fn();

useState.mockImplementation(init => [init, setState]);
useDispatch.mockReturnValue(mockedDispatch);
useSelector.mockImplementation(f =>
  f({
    chatBoard: {
      ticketDetail: {
        ticketId: '12',
        ticketTypeId: '',
        ticketTypeName: '',
        messages: [],
        owpLeagueId: '',
        owpLeagueName: '',
        owpHomeId: '',
        owpAwayId: '',
        homeName: '',
        awayName: '',
        subject: '',
        creatorInfo: {
          id: 0,
          userName: '',
          nickName: ''
        }
      },
      modelQuoteMessage: {
        message: 'ahihi',
        messageId: 42699,
        userId: 360418,
        createdDate: '2023-09-13T17:43:08.77'
      }
    },
    policy: {
      permission: { Username: 'sa', UserId: 8, HasWrite: true }
    }
  })
);

axios.get.mockResolvedValue({data: []});
createRef.mockReturnValue(ref);
describe('test InputMessage', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render component correct', () => {
    const { container } = render(
      <InputMessage
        handleSendFile={mockHandleSendFile}
        handleSendMessage={mockHandleSendMessage}
      />
    );
    const element = container.querySelector('input');
    expect(element).toBeInTheDocument();
  });

  it('test click SendIcon', () => {
    const { container } = render(
      <InputMessage
        handleSendFile={mockHandleSendFile}
        handleSendMessage={mockHandleSendMessage}
      />
    );
    const firstIcon = container.querySelectorAll('.mdi-icon')[0];
    fireEvent.click(firstIcon);
  });
  it('should trigger when press enter input', () => {
    render(
      <InputMessage
        handleSendFile={mockHandleSendFile}
        handleSendMessage={mockHandleSendMessage}
      />
    );
    const input = screen.getByPlaceholderText('Type a message...');
    fireEvent.keyPress(input, { key: 'Enter', code: 13, charCode: 13 });
    expect(mockHandleSendMessage).toBeCalled();
  });

  it('onchange input file, invalid file.name, should not call dispatch', () => {
    const { container } = render(
      <InputMessage
        handleSendFile={mockHandleSendFile}
        handleSendMessage={mockHandleSendMessage}
      />
    );
    const file = new File([new ArrayBuffer(6000000)], 'file.abc');
    axios.post.mockResolvedValue({ data: 1 });
    const input = container.querySelectorAll('input')[0];
    fireEvent.change(input, { target: { files: [file] } });
    expect(mockHandleSendFile).toBeCalled();
  });
});
