import { render } from '@testing-library/react';
import { RecipientIcon } from './RecipientIcon';

describe('RecipientIcon', () => {
  it('should render correct PersonIcon', () => {
    const { container } = render(<RecipientIcon type={1} />);
    expect(container.querySelector('.MuiSvgIcon-root')).toBeInTheDocument();
  });

  it('should render correct GroupIcon', () => {
    const { container } = render(<RecipientIcon type={2} />);
    expect(container.querySelector('.MuiSvgIcon-root')).toBeInTheDocument();
  });
});
