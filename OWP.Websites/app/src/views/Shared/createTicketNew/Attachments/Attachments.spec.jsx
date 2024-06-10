import { createRef } from 'react';
import Attachments from './Attachments';
import { render, screen, fireEvent } from '@testing-library/react';
import ToastrWrapper from '../../../components/ToastrWrapper';

const setSelectedFilesMock = jest.fn();
const showErrorMock = jest.fn();
const mockClickRef = jest.fn();
const ref = { current: { click: mockClickRef } };

jest.mock('../../../components/ToastrWrapper');
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  createRef: jest.fn()
}));
ToastrWrapper.showError.mockImplementation(() => showErrorMock());
const props = () => {
  return {
    setSelectedFiles: setSelectedFilesMock,
    selectedFiles: [{ name: 'test', size: '1000000' }]
  };
};

describe('Attachments under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('onchange input file, valid file.name, file.size, should call setSelectedFiles', done => {
    render(<Attachments {...props()} />);
    const file = new File([new ArrayBuffer(6000)], 'file.doc');
    const input = screen.getByLabelText('file-input');
    fireEvent.change(input, { target: { files: [file] } });
    process.nextTick(async () => {
      expect(setSelectedFilesMock).toHaveBeenCalledTimes(1);
      await done();
    });
  });

  it('onchange input file, invalid file.name, file.size, should  show error, not call setSelectedFiles', done => {
    render(<Attachments {...props()} />);
    const file = new File([new ArrayBuffer(6000)], 'file.txt');
    const input = screen.getByLabelText('file-input');
    fireEvent.change(input, { target: { files: [file] } });
    process.nextTick(async () => {
      expect(setSelectedFilesMock).toHaveBeenCalledTimes(0);
      expect(showErrorMock).toHaveBeenCalledTimes(1);
      await done();
    });
  });

  it('SelectedAttachments should handle click remove', () => {
    createRef.mockReturnValue(ref);
    const { container } = render(<Attachments {...props()} />);
    const button = container.querySelector('.icon-close');
    fireEvent.click(button);
    expect(setSelectedFilesMock).toHaveBeenCalledTimes(1);
  });

  it('DragAndDrop should handle drag', () => {
    createRef.mockReturnValue(ref);
    const { container } = render(<Attachments {...props()} />);
    const file = new File([new ArrayBuffer(500)], 'file.doc');
    const dropZone = container.querySelector('.drop-zone');

    fireEvent.drop(dropZone, {
      dataTransfer: {
        files: [file]
      },
      preventDefault: jest.fn()
    });
    expect(setSelectedFilesMock).toHaveBeenCalledTimes(1);
  });
});
