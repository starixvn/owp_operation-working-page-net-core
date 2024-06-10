import { render, fireEvent, screen } from '@testing-library/react';
import Dialog from './Dialog';

const renderBody = jest.fn();
const onClose = jest.fn();
const onSave = jest.fn();
const defaultProps = {
  titleHeader: 'Dialog',
  onClose,
  onSave,
  renderBody,
  hasWrite: true,
  size: 'sm',
  className: 'dialog'
};

describe('Dialog', () => {
  it('Should render dialog successfully', () => {
    const { getByText } = render(<Dialog {...defaultProps} />);
    expect(getByText('Dialog')).toBeInTheDocument();
  });

  it('RenderBody should to be call', () => {
    render(<Dialog {...defaultProps} />);
    expect(renderBody).toBeCalled();
  });

  it('Onclose should to be call when close modal', () => {
    const { getByText } = render(<Dialog {...defaultProps} />);
    const closeButton = getByText('Cancel');
    fireEvent.click(closeButton);

    expect(onClose).toBeCalled();
  });

  it('On save should to be call when save', () => {
    const { getByText } = render(<Dialog {...defaultProps} />);
    const saveButton = getByText('Save');
    fireEvent.click(saveButton);

    expect(onSave).toBeCalled();
  });
});
