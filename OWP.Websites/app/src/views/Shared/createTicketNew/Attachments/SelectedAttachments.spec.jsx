import SelectedAttachments from './SelectedAttachments';
import { render, fireEvent } from '@testing-library/react';
import { CloseIcon, FileOutlineIcon } from 'mdi-react';
import { TooltipText } from '../../common/Tooltip';

const mockOnRemove = jest.fn();

jest.mock('../../common/Tooltip');
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  createRef: jest.fn()
}));

const props = files => {
  return {
    files: files,
    onRemove: mockOnRemove
  };
};

describe('SelectedAttachments under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render 1 List File zone, when have files', () => {
    const { container } = render(
      <SelectedAttachments {...props([{ name: 'test', size: '1000000' }])} />
    );
    const iconFile = container.querySelector('.icon-file');
    expect(iconFile).toBeInTheDocument();
  });

  it('should not render, when files =null', () => {
    const { container } = render(<SelectedAttachments {...props()} />);
    const iconFile = container.querySelector('.icon-file');
    expect(iconFile).not.toBeInTheDocument();
  });

  it('CloseIcon should handle click', () => {
    const { container } = render(
      <SelectedAttachments {...props([{ name: 'test', size: '1000000' }])} />
    );
    const iconClose = container.querySelector('.icon-close');
    fireEvent.click(iconClose);
    expect(mockOnRemove).toHaveBeenCalledTimes(1);
  });
});
