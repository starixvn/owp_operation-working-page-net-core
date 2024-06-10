import { render } from '@testing-library/react';
import MatchCell from './MatchCell';

const props = {
  leagueName: 'test',
  homeName: 'Arsenal',
  awayName: 'Liverpool',
  eventDateTime: '2020-09-17 01:14:51.750',
  isRead: true
};

describe('MatchCell under test', () => {
  it('should render match-cell', () => {
    const { container } = render(<MatchCell {...props} />);
    expect(container.querySelector('.match-info-cell')).toBeInTheDocument();
  });
});
