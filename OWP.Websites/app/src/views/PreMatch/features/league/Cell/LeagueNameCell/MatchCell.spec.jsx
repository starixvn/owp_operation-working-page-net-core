import { render } from '@testing-library/react';
import MatchCell from './MatchCell';

const createProps = (HomeName, AwayName, MatchStatus) => {
  const dataSource = [
    {
      HomeName,
      AwayName,
      EventDateTime: new Date(),
      MatchStatus
    }
  ];
  const rowIndex = 0;
  return { dataSource, rowIndex };
};

describe('MatchCell under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should render match-row and prematch-nonelive', () => {
    const props = createProps('HomeNameTest', 'HomeNameTest', 'Cancel');
    const { container } = render(<MatchCell {...props} />);
    const element = container.querySelector('.match-row');
    expect(element).toBeInTheDocument();
    const element2 = container.querySelector('.prematch-nonelive');
    expect(element2).toBeInTheDocument();
  });

  it('matchStatus="LIVE(!)", should not render prematch-nonelive', () => {
    const props = createProps('HomeNameTest', 'HomeNameTest', 'LIVE(!)');
    const { container } = render(<MatchCell {...props} />);
    const element = container.querySelector('.prematch-nonelive');
    expect(element).toBeNull();
  });

  it('matchStatus="LIVE", should not render prematch-nonelive', () => {
    const props = createProps('HomeNameTest', 'HomeNameTest', 'LIVE');
    const { container } = render(<MatchCell {...props} />);
    const element = container.querySelector('.prematch-nonelive');
    expect(element).toBeNull();
  });
});
