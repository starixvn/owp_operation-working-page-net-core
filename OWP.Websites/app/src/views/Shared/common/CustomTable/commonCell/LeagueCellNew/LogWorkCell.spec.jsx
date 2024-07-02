import LogWorkCell from './LogWorkCell';
import { fireEvent, render } from '@testing-library/react';

const handleOnclick = jest.fn();
describe('LogWorkCell under test', () => {
  it('should render MatchCell', () => {
    const data = {
      leagueName: 'test',
      homeName: 'Arsenal',
      awayName: 'Liverpool',
      eventDateTime: '2020-09-17 01:14:51.750',
      isRead: true
    };
    const { container } = render(
      <LogWorkCell data={data} onClick={handleOnclick} />
    );
    const leagueName = container.querySelector('.ticket-title-link');
    fireEvent.click(leagueName);
    expect(handleOnclick).toBeCalled();
    expect(leagueName).toBeInTheDocument();
  });

  it('should render OtherIssueCell', () => {
    const data = {
      subject: 'test subject',
      isRead: true
    };
    const { container } = render(
      <LogWorkCell data={data} onClick={handleOnclick} />
    );
    const subject = container.querySelector('.new-add-league-name');
    fireEvent.click(subject);
    expect(handleOnclick).toBeCalled();
    expect(subject).toBeInTheDocument();
  });
});
