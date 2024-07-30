import { fireEvent, render } from '@testing-library/react';
import Command from './Command';

describe('Command under test', () => {
  it('should render Command', () => {
    const handleOnClose = jest.fn();
    const handleOnSave = jest.fn();
    const props = { onClose: handleOnClose, onSave: handleOnSave };
    const { container } = render(<Command {...props} />);
    fireEvent.click(container.querySelector(`[class="button button-cancel"]`)!);
    expect(handleOnClose).toBeCalled();
    fireEvent.click(container.querySelector(`[class="button button-save"]`)!);
    expect(handleOnSave).toBeCalled();
  });
});
