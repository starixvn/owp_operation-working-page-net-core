import { fireEvent, render, screen } from '@testing-library/react';
import TeamInfo from './TeamInfo';
import { useDispatch, useSelector } from 'react-redux';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
(useDispatch as jest.Mock).mockReturnValue(mockDispatch);

const setUp = (loading, entity) => {
  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn({
      teamInfoReducer: {
        loading,
        entity
      }
    })
  );
};

describe('test EditCell component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });
  const props = {
    owpLeagueId: 10,
    onClose: jest.fn(),
    leagueName: 'ABC'
  };
  it('render  Loading component correct', () => {
    setUp('pending', []);
    const { container } = render(<TeamInfo {...props} />);
    expect(container.querySelector('.loading-process')).toBeInTheDocument();
  });

  it('render correct Dialog component', () => {
    setUp('success', []);
    const { container } = render(<TeamInfo {...props} />);

    const button = screen.getByText('Close');
    expect(button).toBeInTheDocument();

    fireEvent.click(button);

    expect(props.onClose).toBeCalled();
  });

  it('render correct Dialog component with body', () => {
    setUp('success', [{ OWPTeamName: 'ABC' }]);
    const { container } = render(<TeamInfo {...props} />);

    expect(screen.getByText('ABC')).toBeInTheDocument();
  });
});
