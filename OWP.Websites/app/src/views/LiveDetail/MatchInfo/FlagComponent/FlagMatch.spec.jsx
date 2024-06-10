import FlagMatch from './FlagMatch';
import { render, screen } from '@testing-library/react';

const createProps = (flagInfo, awayFlag, homeFlag) => {
  return {
    teamName: {
      homeName: 'Arsenal (V)',
      awayName: 'AC Milan (V)'
    },
    awayFlag: awayFlag,
    homeFlag: homeFlag,
    flagInfo: flagInfo
  };
};

describe('test FlagExpand', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('awayFlag available and homeFlag is null, should render FlagDetail', () => {
    const props = createProps('FIFA', 'aaaa', 'bbbbbb');
    const { container } = render(<FlagMatch {...props} />);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('flagInfo = PES, should render FlagDetail', () => {
    const props = createProps('PES', 'aaaa', 'bbbb');
    const { container } = render(<FlagMatch {...props} />);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('flagInfo = PES 21, should render FlagDetail', () => {
    const props = createProps('PES 21', 'aaaa', 'bbbb');
    const { container } = render(<FlagMatch {...props} />);
    expect(container.firstChild).toMatchSnapshot();
  });
});
