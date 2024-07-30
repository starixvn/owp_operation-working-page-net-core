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
      <LogWorkCell data={data}/>
    );
    const leagueName = container.querySelector('.ticket-title-link');

    expect(leagueName).toBeInTheDocument();
  });

  it('should render OtherIssueCell', () => {
    const data = {
      subject: 'test subject',
      isRead: true
    };
    const { container } = render(
      <LogWorkCell data={data} />
    );
    const subject = container.querySelector('.new-add-league-name');
   
    expect(subject).toBeInTheDocument();
  });
});
