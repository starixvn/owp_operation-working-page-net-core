import { fireEvent, render, screen } from '@testing-library/react';
import GroupPopup from '../groupPopup/GroupPopup';
import Filter from './Filter';

jest.mock('../groupPopup/GroupPopup', () => {
  return jest.fn(() => 'liam');
});

describe('test Filter', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render Filter and Add button', () => {
    const { container } = render(<Filter />);
    expect(container.querySelector('.filter')).toBeInTheDocument();
    expect(container.querySelector('.button-submit')).toBeInTheDocument();
  });

  it('isOpen=false, should render Filter and button, not render GroupPopup', () => {
    const { container } = render(<Filter />);
    expect(container.querySelector('.filter')).toBeInTheDocument();
    expect(container.querySelector('.button-submit')).toBeInTheDocument();
    expect(GroupPopup).toBeCalledTimes(0);
  });

  it('click Add button, should call setState', async () => {
    const { container } = render(<Filter />);
    const element = container.querySelector('.button-submit');
    fireEvent.click(element!);
    await screen.findByText('liam');
    expect(GroupPopup).toBeCalled();
  });
});
