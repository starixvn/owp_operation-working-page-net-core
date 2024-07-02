import { fireEvent, render, screen } from '@testing-library/react';
import NotePopover from './NotePopover';

describe('NotePopover', () => {
  const defaultProps = {
    policy: { HasRead: true, HasWrite: true },
    event: {
      OWPMatchId: 1,
      Notes: [
        {
          MatchNoteId: 1,
          Note: 'ahihi',
          Username: 'sa'
        }
      ]
    },
    onShowNote: jest.fn()
  };

  it('should render component without popover', () => {
    const { container } = render(<NotePopover {...defaultProps} />);
    expect(container.querySelector('.is-show')).not.toBeInTheDocument();
  });

  it('should show popover when click on icon', () => {
    const { container } = render(<NotePopover {...defaultProps} />);

    fireEvent.click(container.querySelector('.icon-comment'));
    expect(container.querySelector('.is-show')).toBeInTheDocument();
  });

  it('should hide popover when click on icon after popover is shown', async () => {
    const { container } = render(<NotePopover {...defaultProps} />);
    fireEvent.click(container.querySelector('.icon-comment'));
    expect(container.querySelector('.is-show')).toBeInTheDocument();
    fireEvent.click(container.querySelector('.icon-comment'));
    expect(container.querySelector('.is-show')).not.toBeInTheDocument();
  });
});
