import { render } from '@testing-library/react';
import MaxbetOuCell from './MaxbetOUCell';

describe('Spread under test', () => {
  it('should render cell has data successfully', () => {
    const props = {
      dataSource: [{}],
      rowIndex: 0
    };
    const { container } = render(<MaxbetOuCell {...props} />);
    const cell = container.querySelector('.header-Maxbet');
    expect(cell).toContainHTML('-');
  });

  it('should render empty cell when InfoSiteMatchId  has value', () => {
    const props = {
      dataSource: [
        {
          MaxBetOu: { Maxbet: 10000, Spread: 0.13 },
          InfoSiteMatchId: 12
        }
      ],
      rowIndex: 0
    };
    const { container } = render(<MaxbetOuCell {...props} />);
    const cell = container.querySelector('.header-Maxbet div div div div');
    expect(cell).toContainHTML('<div>10K(13%)</div>');
  });
});
