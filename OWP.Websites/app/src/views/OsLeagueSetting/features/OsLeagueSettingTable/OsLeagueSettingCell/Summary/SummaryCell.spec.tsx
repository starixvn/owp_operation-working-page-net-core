import { render, screen } from '@testing-library/react';
import { CellProps } from '../../../../typing/tableType';
import SummaryCell from './SummaryCell';

const props: CellProps = {
  rowIndex: 0,
  dataSource: [
    {
      categoryName: 'Major',
      isActive: false,
      leagueSettingId: 1,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo: [{ text: 'Manual Update Score', style: 'manual' }],
      summaryRemark: [{ text: '23', style: 'IpTv' }]
    }
  ]
};

describe('SummaryCell under test', () => {
  it('Should render info', () => {
    render(<SummaryCell {...props} />);
    const info = screen.getByText('Manual Update Score');
    expect(info).toBeInTheDocument();
  });
});
