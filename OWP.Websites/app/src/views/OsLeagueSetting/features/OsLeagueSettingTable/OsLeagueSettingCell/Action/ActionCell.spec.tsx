import React from 'react';
import '@testing-library/jest-dom';
import { fireEvent, render, screen } from '@testing-library/react';
import ActionCell from './ActionCell';
import useStore from '../../../../app/store';

const mockGetLogSettingQuery = jest.fn();

const dataSource = [
  {
    categoryName: 'Major',
    isActive: false,
    isActiveMaxBet: false,
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
  const originalError = console.error;

  beforeEach(() => {
    useStore.setState({
      logData: [],
      getLogSettingQuery: mockGetLogSettingQuery
    });
    mockGetLogSettingQuery.mockClear();

    // Suppress known reactstrap prop-types warnings
    console.error = jest.fn((message) => {
      if (message?.includes?.('transition.timeout')) {
        return;
      }
      originalError(message);
    });
  });

  afterEach(() => {
    console.error = originalError;
  });

  //TODO: Fix this test
  it('should open Popup', async () => {
    const { container } = render(<ActionCell {...props} />);
    const element = container.querySelector(`[class="action-cell"]`);
    expect(element).toBeInTheDocument();
    if (element) {
      fireEvent.click(element);
    }
    const league = await screen.findByText('Log History - ABC');
    expect(league).toBeInTheDocument();
    expect(mockGetLogSettingQuery).toHaveBeenCalledWith(1);
  });
});
