import { fireEvent, render, screen } from '@testing-library/react';
import ActionCell from './ActionCell';

const dataSource = [
  {
    categoryName: 'Major',
    isActive: false,
    leagueSettingId: 1,
    maxBetHDP: 1,
    maxBetOU: 1,
    osLeagueName: 'ABC',
    roundName: 'Pre Season',
    spreadHDP: 1,
    spreadOU: 1,
    summaryInfo: [
      {
        text: '<Offer 2 Lines/Offer 3 Lines/Offer 6 Lines>',
        style: 'color'
      }
    ],
    summaryRemark: [
      {
        text: '<2nd>,<All/CMT Venue/Feed TV/Feed>',
        style: 'color'
      }
    ]
  }
];
const props = {
  dataSource,
  rowIndex: 0
};

describe('ActionCell under test', () => {
  it('should open Popup', async () => {
    const { container } = render(<ActionCell {...props} />);
    const element = container.querySelector(`[class="action-cell"]`);
    fireEvent.click(element!);
    const league = await screen.findByText('Log History - ABC');
    expect(league).toBeInTheDocument();
  });
});
