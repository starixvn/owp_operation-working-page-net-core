import TicketTypeFilter from './TicketTypeFilter';
import { render, screen, fireEvent } from '@testing-library/react';

const props = {
  onSelectedChange: jest.fn(),
  defaultValue: [1, 2, 3],
  options: [{ value: 1, title: 'others' }]
};

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  createRef: jest.fn()
}));

describe('test TicketTypeFilter', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render option ticketType', () => {
    const { container } = render(<TicketTypeFilter {...props} />);
    const li = container.querySelector(`[role="menuitem"]`);
    expect(li).toBeInTheDocument();
    expect(screen.queryByText('1')).toBeInTheDocument();
    expect(screen.queryAllByRole('menuitem')).toHaveLength(2);
  });
});
