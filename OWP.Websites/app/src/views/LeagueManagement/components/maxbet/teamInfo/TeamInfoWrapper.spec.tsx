import { render, fireEvent } from '@testing-library/react';
import TeamInfoWrapper from './TeamInfoWrapper';

let mockSetState = jest.fn();
let wrapper;
jest.mock('react', () => ({
  ...(jest.requireActual('react') as any),
  useState: initial => [initial, mockSetState]
}));

describe('TeamInfoWrapper under test', () => {
  beforeEach(() => {
    wrapper = render(
      <TeamInfoWrapper owpLeagueId={123} leagueName='abc' onClose={() => {}} />
    );
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should  render correct', () => {
    expect(wrapper.container.querySelector('.btn-show')).toBeInTheDocument();
  });

  it('should calls setVisibleTeamInfo with value true ', () => {
    const button = wrapper.container.querySelector('.btn-show');
    fireEvent.click(button);
    expect(mockSetState).toHaveBeenCalled();
  });
});
