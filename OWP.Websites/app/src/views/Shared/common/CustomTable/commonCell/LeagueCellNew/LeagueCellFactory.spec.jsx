import LeagueCellFactory from './LeagueCellFactory';
import { render, screen } from '@testing-library/react';
import { useSelector } from 'react-redux';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  createRef: jest.fn()
}));

useSelector.mockImplementation(selectorFn => selectorFn({
  pagePolicy:{
    permission:{
      ActorId: 8
    }
  }
}));
 

describe('LeagueCellFactory under test', () => {
  it('should render OtherIssueCell case (eventType = 3)', () => {
    const data = {
      eventType: 3,
      subject: 'ABC',
      isRead: true
    };
    render(<LeagueCellFactory data={data} />);
    expect(screen.getByText(data.subject)).toBeInTheDocument();
  });
  it('should render MatchCell case (eventType = 2)', () => {
    const data = {
      eventType: 2,
      homeName: 'team A',
      awayName: 'team B',
      koTime: '20-6-2022',
      owpLeagueName: 'EPL'
    };
    render(<LeagueCellFactory data={data} />);
    expect(
      screen.getByText(data.homeName + ' -vs- ' + data.awayName)
    ).toBeInTheDocument();
  });
  it('should render LeagueCell case (eventType = 1)', () => {
    const data = {
      eventType: 1,
      owpLeagueName: 'EPL',
      isRead: true
    };
    render(<LeagueCellFactory data={data} />);
    expect(screen.getByText(data.owpLeagueName)).toBeInTheDocument();
  });
});
