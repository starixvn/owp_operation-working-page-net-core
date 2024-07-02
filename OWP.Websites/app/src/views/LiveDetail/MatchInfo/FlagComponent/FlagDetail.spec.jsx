import FlagDetail from './FlagDetail';
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

describe('test FlagDetail', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('awayFlag available and homeFlag is null, should render img', () => {
    const props = createProps('FIFA', 'aaaa');
    const { container } = render(<FlagDetail {...props} />);
    const element = container.querySelectorAll("img");
    expect(element.length).toEqual(2);
  });

  it('flagInfo = PES, should render img', () => {
    const props = createProps('PES', 'aaaa', 'bbbb');
    const { container } = render(<FlagDetail {...props} />);
    const element = container.querySelectorAll("img");
    expect(element.length).toEqual(2);
  });

  it('always render team-flag', () => {
    const props = createProps('PES', 'aaaa', 'bbbb');
    const { container } = render(<FlagDetail {...props} />);
    const element = container.getElementsByClassName("team-flag");
    expect(element.length).toEqual(1);
  });
});
