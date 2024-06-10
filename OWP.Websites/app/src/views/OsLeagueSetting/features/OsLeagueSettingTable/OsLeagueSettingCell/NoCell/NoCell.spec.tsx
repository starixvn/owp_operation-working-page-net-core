import { render, screen } from '@testing-library/react';
import { CellProps } from '../../../../typing/tableType';
import NoCell from './NoCell';

const props: CellProps = {
  rowIndex: 0,
  dataSource: [
    {
      categoryName: 'Major',
      isActive: false,
      leagueSettingId: 2,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo:
        '<Offer 2 Lines/Offer 3 Lines/Offer 6 Lines>,<2 Bet Types>,<Bet Delay 4s>,Manual Update Score',
      summaryRemark:
        '<2nd>,<CMT/Venue Feed>,10:20 - 11:15,IPTV Only,<Bet delay 15s>'
    }
  ]
};

describe('LeagueCell under test', () => {
  it('Should render osLeagueName', () => {
    render(<NoCell {...props} />);
    const no = screen.getByText('1');
    expect(no).toBeInTheDocument();
  });
});
