import FormMatchInfo from './FormMatchInfo';
import EventAssignmentService from './Service';
import {
  act,
  fireEvent,
  render,
  screen,
  waitFor
} from '@testing-library/react';

jest.mock('./Service');

const getInfoSiteMatch = jest.fn();

const matchStatusResponseData = [
  { Id: 1, Value: 'Live', RefName: 'MatchStatus' },
  { Id: 2, Value: 'In-play', RefName: 'MatchStatus' },
  { Id: 3, Value: 'Completed', RefName: 'MatchStatus' },
  { Id: 4, Value: 'Cancel', RefName: 'MatchStatus' },
  { Id: 5, Value: 'Postponed', RefName: 'MatchStatus' },
  { Id: 6, Value: 'Suspended', RefName: 'MatchStatus' }
];

const mockParentEvent = {
  OWPMatchId: 305880,
  KickOffTime: '03:00 PM'
};

const defaultProps = {
  matchStatuses: matchStatusResponseData,
  bettypeData: [
    { value: 1, label: 'No of corner' },
    { value: 2, label: 'Next corner' }
  ],
  getInfoSiteMatch
};

describe('FormMatchInfo', () => {
  it('should select Live match status on Init', () => {
    const { container, getByText } = render(
      <FormMatchInfo {...defaultProps} />
    );
    expect(getByText('Live')).toBeInTheDocument();
  });

  it('should update state when change match status', () => {
    const { container, getByText } = render(
      <FormMatchInfo {...defaultProps} />
    );
    fireEvent.focus(container.querySelector('input'));
    fireEvent.keyDown(container.querySelector('input'), {
      key: 'ArrowDown',
      code: 40
    });

    fireEvent.click(getByText('Postponed'));

    expect(getByText('Postponed')).toBeInTheDocument();
  });

  it('should invoke getInfoSiteMatch when clicking infoSite icon', () => {
    const { container } = render(<FormMatchInfo {...defaultProps} />);
    fireEvent.click(container.querySelector('.icon-getInfo'));
  });

  it('should display special event when add minor match from main match', () => {
    const props = {
      ...defaultProps,
      parentEvent: mockParentEvent
    };
    const { container } = render(<FormMatchInfo {...props} />);
    expect(container.firstChild).toBeInTheDocument();
  });

  it('should disable fields when add minor match from main match', () => {
    const props = {
      ...defaultProps,
      parentEvent: mockParentEvent
    };
    const { container } = render(<FormMatchInfo {...props} />);

    expect(container.querySelector('.input-KO-time')).toHaveAttribute(
      'readonly',
      ''
    );
    expect(container.querySelector('.input-league-name')).toHaveAttribute(
      'readonly',
      ''
    );
    expect(container.querySelector('.input-home-name')).toHaveAttribute(
      'readonly',
      ''
    );
    expect(container.querySelector('.input-away-name')).toHaveAttribute(
      'readonly',
      ''
    );
  });

  it('should not display InfoSiteId when add minor match from main match', () => {
    const props = {
      ...defaultProps,
      parentEvent: mockParentEvent
    };
    const { container } = render(<FormMatchInfo {...props} />);

    expect(container.querySelector('.infosite-id')).not.toBeInTheDocument();
  });

  it('should display special event when add minor match from main match', () => {
    const props = {
      ...defaultProps,
      parentEvent: mockParentEvent
    };
    const { getByText } = render(<FormMatchInfo {...props} />);

    expect(getByText('Special Event')).toBeInTheDocument();
  });

  it('should update state when select SpecialEvent', async () => {
    EventAssignmentService.getSpecialEventTemplate.mockReturnValue({
      LeagueName: 'h - PENALTY',
      HomeName: 'h',
      AwayName: 'h 1st PENALTY Awarded - Yes'
    });
    const props = {
      ...defaultProps,
      parentEvent: mockParentEvent
    };
    const { container, getByText } = render(<FormMatchInfo {...props} />);
    const input = container.querySelectorAll('input');
    fireEvent.focus(input[0]);
    fireEvent.keyDown(input[0], { key: 'ArrowDown', code: 13, charCode: 13 });
    waitFor(() => {
      fireEvent.click(getByText('Next corner'));
    });
    expect(getByText('Next corner')).toBeInTheDocument();
  });
});
