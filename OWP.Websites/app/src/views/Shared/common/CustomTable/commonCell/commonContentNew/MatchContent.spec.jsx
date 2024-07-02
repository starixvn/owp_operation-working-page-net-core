import { render, screen } from '@testing-library/react';
import MatchContent from './MatchContent';

const props = {
  evenDate: 'evenDate',
  koTime: 'koTime',
  leagueName: 'leagueName',
  matchName: {
    homeName: 'HomeName',
    awayName: 'AwayName'
  }
};

describe('MatchContent under test', () => {
  it('should render Event Date', () => {
    const { container } = render(<MatchContent {...props} />);
    expect(container.querySelector('.match-info-date')).toBeInTheDocument();
    expect(screen.getByText(props.evenDate)).toBeInTheDocument();
  });
  it('should render K.O:', () => {
    const { container } = render(<MatchContent {...props} />);
    expect(container.querySelector('.match-info-KO')).toBeInTheDocument();
    expect(screen.getByText('K.O: ' + props.koTime)).toBeInTheDocument();
  });
  it('should render matchName', () => {
    const { container } = render(<MatchContent {...props} />);
    expect(
      container.querySelector('.match-info-match-name')
    ).toBeInTheDocument();
    expect(
      screen.getByText(
        props.matchName.homeName + ' -vs- ' + props.matchName.awayName
      )
    ).toBeInTheDocument();
  });
});
