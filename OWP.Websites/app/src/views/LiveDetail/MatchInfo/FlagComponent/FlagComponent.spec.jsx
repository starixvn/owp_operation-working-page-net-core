import FlagComponent from './FlagComponent';
import { render, screen } from '@testing-library/react';

const createProps = isExpand => {
  return {
    teamName: {
      homeName: 'Arsenal (V)',
      AwayName: 'AC Milan (V)'
    },
    leagueName: 'PremierLeague',
    isExpand: isExpand
  };
};

describe('test FlagComponent', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('render FlagMatch', () => {
    const props = createProps(true);
    const { container } = render(<FlagComponent {...props} />);
    expect(container.firstChild).toMatchSnapshot();
  });
});
