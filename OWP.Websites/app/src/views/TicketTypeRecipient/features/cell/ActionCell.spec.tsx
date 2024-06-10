import { fireEvent, render, screen } from '@testing-library/react';
import ActionCell from './../cell/ActionCell';

const props = {
  onOpenPopup: jest.fn()
};

describe('ActionCell under test', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should match snapshot', () => {
    const { container } = render(<ActionCell {...props} />);
    expect(container).toMatchSnapshot();
  });

  it('should setState when click edit', () => {
    const { container } = render(<ActionCell {...props} />);
    const element = container.querySelector(`[class="icon-edit"]`);
    fireEvent.click(element!);
    expect(props.onOpenPopup).toHaveBeenCalledTimes(1);
  });
});
