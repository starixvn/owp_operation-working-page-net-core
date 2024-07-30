import BlacklistCell from './BlacklistCell';
import { render } from '@testing-library/react';

describe('Spread under test', () => {
  it('should render cell has data successfully', () => {
    const props = {
      dataSource: [
        {
          Blacklist: true
        }
      ],
      rowIndex: 0
    };
    const { container } = render(<BlacklistCell {...props} />);
    const cell = container.querySelector('.header-Blacklist');
    expect(cell).toBeInTheDocument();
  });

  it('should render empty cell when InfoSiteMatchId  has value', () => {
    const props = {
      dataSource: [
        {
          Blacklist: true,
          InfoSiteMatchId: 12
        }
      ],
      rowIndex: 0
    };
    const { container } = render(<BlacklistCell {...props} />);
    const cell = container.querySelector('.empty-cell');
    expect(cell).toBeInTheDocument();
  });
});
