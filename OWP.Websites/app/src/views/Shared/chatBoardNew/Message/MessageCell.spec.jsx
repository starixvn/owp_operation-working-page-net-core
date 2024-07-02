import { fireEvent, render, screen } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import { saveMessage } from '../chatBoardThunkNative';
import MessageCell from './MessageCell';
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('reactstrap');
jest.mock('../../Service/Service');
jest.mock('../chatBoardThunkNative', () => ({
  saveMessage: jest.fn().mockImplementation(() => {})
}));

global.window = Object.create(window);
const url = 'http://dummy.com';
Object.defineProperty(window, 'location', {
  value: {
    href: url
  }
});
const setUp = HasWrite => {
  saveMessage.rejected = 'Liam';
  saveMessage.payload = 'bug';
  useDispatch.mockImplementation(() => jest.fn(() => jest.fn(() => null)));
  useSelector.mockImplementation(f =>
    f({
      policy: {
        permission: { Username: 'sa', UserId: 8, HasWrite }
      }
    })
  );
};

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);
describe('test MessageCell', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should contain Linkify when fileId null and save', () => {
    setUp(true);
    const props = {
      username: 'sa',
      createdDate: new Date(
        'Fri Sep 11 2020 15:04:38 GMT+0700 (Indochina Time)'
      ),
      message: 'Hello',
      fileId: null,
      isFileAvailable: false,
      isSaved: true
    };
    const { container } = render(
      <MessageCell data={props} index={0} policy='policy' />
    );

    const save = container.querySelector('.mdi-icon');
    fireEvent.click(save);
    expect(save).toBeInTheDocument();
  });

  it('fileId valid should contain is-file class and unSave message', () => {
    setUp(true);
    const props = {
      username: 'sa',
      createdDate: new Date(
        'Fri Sep 11 2020 15:04:38 GMT+0700 (Indochina Time)'
      ),
      message: 'Hello.txt',
      fileId: 12,
      isSaved: false
    };
    const { container } = render(
      <MessageCell data={props} index={0} policy='policy' />
    );
    const element = container.querySelector('.is-file');
    expect(element).toBeInTheDocument();
    const save = container.querySelector('.mdi-icon');
    fireEvent.click(save);
    expect(save).toBeInTheDocument();
  });
  it('render no comment', () => {
    setUp(false);
    const props = {
      username: '',
      createdDate: '',
      message: '',
      fileId: null,
      isSaved: false
    };
    render(<MessageCell data={props} index={0} policy='policy' />);
    const element = screen.getByText('No comments');
    expect(element).toBeInTheDocument();
  });
  it('render ContentSaveOutlineIcon ', () => {
    setUp(false);
    const props = {
      username: 'sa',
      createdDate: new Date(
        'Fri Sep 11 2020 15:04:38 GMT+0700 (Indochina Time)'
      ),
      message: 'Hello.txt',
      fileId: 12,
      isSaved: false
    };
    const { container } = render(
      <MessageCell data={props} index={0} policy='policy' />
    );
    const element = container.querySelector('.icon-save-message');
    expect(element).toBeNull();
  });
  it('render with no permission', () => {
    setUp(false);
    const props = {
      username: 'sa',
      createdDate: new Date(
        'Fri Sep 11 2020 15:04:38 GMT+0700 (Indochina Time)'
      ),
      message: 'Hello.txt',
      fileId: 12,
      isSaved: true
    };
    const { container } = render(
      <MessageCell data={props} index={0} policy='policy' />
    );
    const element = container.querySelector('.icon-un-save-message-disable');
    expect(element).toBeNull();
  });
});
