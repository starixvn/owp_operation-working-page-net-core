import { render, screen, fireEvent } from '@testing-library/react';
import { RecipientsSelectedList } from './RecipientsSelectedList';

describe('RecipientsSelectedList component', () => {
  const items = [
    {
      groupName: 'sas',
      nickName: null,
      recipientId: 5,
      recipientType: 2,
      userName: null
    },
    {
      groupName: null,
      nickName: null,
      recipientId: 31,
      recipientType: 1,
      userName: 'lucas'
    }
  ];

  it('should render correct with non props', () => {
    const { container } = render(<RecipientsSelectedList />);

    expect(container.querySelector('.selected-container')).toBeInTheDocument();
  });

  it('should render correct ', () => {
    const mockOnToggleList = jest.fn();
    const { container } = render(
      <RecipientsSelectedList
        onToggleList={mockOnToggleList}
        collapsed={true}
        items={items}
      />
    );
    expect(screen.getByText('sas')).toBeInTheDocument();
    const expand = container.querySelector('.MuiChip-clickable');
    fireEvent.click(expand);
    expect(mockOnToggleList).toBeCalled();
  });

  it('should handle delete item ', () => {
    const mockDeleteItem = jest.fn();

    const { container } = render(
      <RecipientsSelectedList items={items} onDeleteItem={mockDeleteItem} />
    );

    const icon = container.querySelector('.MuiChip-deleteIcon');
    fireEvent.click(icon);
    expect(mockDeleteItem).toBeCalled();
  });
});
