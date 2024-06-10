import { render, fireEvent, screen } from '@testing-library/react';
import Comment from './Comment';

describe('Comment', () => {
  it('renders the Comment component correctly', () => {
    const comment = 'This is a test comment';
    const handleOnChange = jest.fn();

    render(<Comment comment={comment} handleOnChange={handleOnChange} />);

    const textarea = screen.getByText('This is a test comment');
    expect(textarea).toBeInTheDocument();
  });

  it('calls handleOnChange when the comment input value changes', () => {
    const comment = 'This is a test comment';
    const handleOnChange = jest.fn();

    const { container } = render(
      <Comment comment={comment} handleOnChange={handleOnChange} />
    );

    const textarea = container.querySelector('.input-field');
    const newComment = 'Updated comment';

    textarea && fireEvent.change(textarea, { target: { value: newComment } });

    expect(handleOnChange).toHaveBeenCalledTimes(1);
    expect(handleOnChange).toHaveBeenCalledWith(newComment);
  });
});
