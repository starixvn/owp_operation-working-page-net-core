import { render, screen } from '@testing-library/react';
import TimeOfMatch from './TimeOfMatch';
type typeProps = {
  KickOffTimeDisplay: string;
  KickOffTime: string;
  InfoSiteTimeDisplay: string | null;
  InfoSiteTime: string | null;
  OtherTimeDisplay: string | null;
  OtherTime: string | null;
  rowIndex: number;
};
const props: typeProps = {
  InfoSiteTime: '18:00:00',
  InfoSiteTimeDisplay: '06:00 PM',
  KickOffTime: '17:00:00',
  KickOffTimeDisplay: '05:00 PM',
  OtherTime: '19:00:00',
  OtherTimeDisplay: '07:00 PM',
  rowIndex: 1
};

describe('Time under test', () => {
  it('Should render time', () => {
    render(<TimeOfMatch {...props} />);
    const infoSiteTime = screen.getByText('06:00 PM');
    const kickOffTime = screen.getByText('05:00 PM');
    const OtherTime = screen.getByText('07:00 PM');
    expect(infoSiteTime).toBeInTheDocument();
    expect(kickOffTime).toBeInTheDocument();
    expect(OtherTime).toBeInTheDocument();
  });
});
