import Notes from './Notes';
import Service from './Service';
import { render, screen, act } from '@testing-library/react';

jest.mock('./Service');

const promise = Promise.resolve();
describe('Notes', () => {
  const defaultProps = {
    matchId: 1,
    policy: {
      HasWrite: true
    }
  };

  beforeEach(() => {
    Service.query.mockResolvedValue();
  });

  it('should render Loading on init', async () => {
    const props = {
      ...defaultProps
    };
    const { container } = render(<Notes {...props} />);
    await act(async () => {
      await promise;
    })
    expect(container.querySelector('.widget-wrap')).toBeInTheDocument();
  });
});
