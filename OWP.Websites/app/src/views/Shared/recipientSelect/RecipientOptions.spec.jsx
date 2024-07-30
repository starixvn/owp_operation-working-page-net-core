import { fireEvent, render, screen } from '@testing-library/react';
import { RecipientOptions } from './RecipientOptions';

describe('RecipientOptions component', () => {
  it('should render correct when non options', () => {
    const { container } = render(<RecipientOptions />);

    expect(
      container.querySelector('.MuiAutocomplete-noOptions')
    ).toBeInTheDocument();
  });

  it('should render correct when has options', () => {
    const options = [
      {
        groupName: null,
        nickName: 'sa_nickname',
        recipientId: 8,
        recipientType: 1,
        userName: 'sa'
      }
    ];
    render(<RecipientOptions options={options} />);
    expect(screen.getByText('sa (sa_nickname)')).toBeInTheDocument();
  });
  it('should render correct when has options without nickname', () => {
    const options = [
      {
        groupName: null,
        nickName: null,
        recipientId: 8,
        recipientType: 1,
        userName: 'sa'
      }
    ];
    render(<RecipientOptions options={options} />);
    expect(screen.getByText('sa')).toBeInTheDocument();
  });

  it('should render correct when options include recipient group', () => {
    const options = [
      {
        groupName: 'sas',
        nickName: null,
        recipientId: 5,
        recipientType: 2,
        userName: null
      }
    ];
    render(<RecipientOptions options={options} />);
    expect(screen.getByText('sas')).toBeInTheDocument();
  });

  it('should call handleClick when click button add', () => {
    const options = [
      {
        groupName: 'sas',
        nickName: null,
        recipientId: 5,
        recipientType: 2,
        userName: null
      }
    ];
    const mockAddItem = jest.fn();
    render(<RecipientOptions options={options} onAddItem={mockAddItem} />);
    const button = screen.getByText('Add');

    fireEvent.click(button);

    expect(mockAddItem).toBeCalled();
  });
});
