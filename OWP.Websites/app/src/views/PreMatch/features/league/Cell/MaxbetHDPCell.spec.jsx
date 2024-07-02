import MaxbetCell from './MaxbetHDPCell';
import { render, screen } from '@testing-library/react';

describe('Maxbet under test', () => {
  it('should render cell has data successfully', () => {
    const props = {
      dataSource: [
        {
          MaxBetHdp: '11k'
        }
      ],
      rowIndex: 0
    };
    const { container } = render(<MaxbetCell {...props} />);
    const cell = container.querySelector('.header-Maxbet div');
    expect(cell).toContainHTML('-');
  });

  it('should render empty cell when InfoSiteMatchId  has value', () => {
    const props = {
      dataSource: [
        {
          MaxBetHdp: { Maxbet: 1000, Spread: 0.13 },
          InfoSiteMatchId: 12
        }
      ],
      rowIndex: 0
    };
    const { container } = render(<MaxbetCell {...props} />);
    const cell = container.querySelector('.header-Maxbet div');
    expect(cell).toContainHTML('1K(13%)');
  });
});
