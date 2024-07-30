import DropdownNative from './DropdownNative';
import { render, fireEvent, screen, waitFor } from '@testing-library/react';
const mockOnclick = jest.fn();
const props = {
  items: [
    {
      title: 'abc',
      handler: mockOnclick
    }
  ]
};

describe('DropdownNative under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render item', async () => {
    const { container } = render(<DropdownNative {...props} />);
    const button = container.querySelector('.dropdown-item');
    fireEvent.click(button);
    expect(screen.getByText('abc')).toBeInTheDocument();
    await waitFor(() => {
      expect(mockOnclick.mock.calls.length).toEqual(1);
    });
  });
});
