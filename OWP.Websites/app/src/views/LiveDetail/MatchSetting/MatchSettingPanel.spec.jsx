import MatchSettingPanel from './MatchSettingPanel';
import MatchSettingService from './Service';
import { act, fireEvent, render, screen } from '@testing-library/react';

const promise = Promise.resolve();

jest.mock('./Service');
jest.mock('../../components/ToastrWrapper');
jest.mock('../../components/Spinner');

MatchSettingService.getMatchsettingByMatchId.mockReturnValue([
  {
    BettypeId: 1,
    Maxbet: null,
    Spread: null,
  },
  {
    BettypeId: 2,
    Maxbet: 3,
    Spread: 3,
  }
]);

describe('<MatchSettingPanel/>', () => {
  const onSave = jest.fn();
  const defaultProps = {
    policy: {
      HasWrite: true
    },
    matchId: 306332,
    minorBettypeId: 1,
    onSave,
    data: {
      MatchOffers: [{ value: 1, label: '10s Accept' }]
    }
  };
  it('render <MatchSettingPanel/> component success', async () => {
    const props = {
      ...defaultProps,
      data: {
        MatchOffers: [{ value: 1, label: '10s Accept' }]
      }
    };
    MatchSettingService.getMatchsettingByMatchId.mockReturnValue([]);
    const { container } = render(<MatchSettingPanel {...props} />);
    await act(async () => {
      await promise;
    })
    expect(container.firstChild).toMatchSnapshot();
  });

  it('render <MatchSettingPanel/> component when matchsetting not empty', async () => {
    const props = {
      ...defaultProps
    };
    MatchSettingService.getMatchsettingByMatchId.mockReturnValue([
      {
        BettypeId: 1,
        Maxbet: null,
        Spread: null,
      },
      {
        BettypeId: 2,
        Maxbet: 3000,
        Spread: 0.003,
      }
    ]);
    render(<MatchSettingPanel {...props} />);

    await act(async () => {
      await promise;
    });


    expect(screen.getByDisplayValue('3')).toBeInTheDocument();
    expect(screen.getByDisplayValue('0.3')).toBeInTheDocument();
  });

  it('should show button add when user has permission write', async () => {
    const props = {
      ...defaultProps
    };
    const { container } = render(<MatchSettingPanel {...props} />);
    const addOddButton = container.getElementsByClassName('btn-add-odd');
    await act(async () => {
      await promise;
    });
    expect(addOddButton.length).toEqual(1);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('should not show button add when user only read', async () => {
    const props = {
      ...defaultProps,
      policy: {
        HasWrite: false
      }
    };
    const { container } = render(<MatchSettingPanel {...props} />);
    const addOddButton = container.getElementsByClassName('btn-add-odd');
    await act(async () => {
      await promise;
    });
    expect(addOddButton.length).toEqual(0);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('collapse pannel success', async () => {
    const props = {
      ...defaultProps,
      policy: {
        HasWrite: true
      }
    };
    const { container } = render(<MatchSettingPanel {...props} />);
    const collapseButton = container.querySelector('.title');
    fireEvent.click(collapseButton);
    await act(async () => {
      await promise;
    });

    expect(container.querySelector('.card-collapse')).toBeInTheDocument();
    expect(container.firstChild).toMatchSnapshot();
  });

  it('update state when click edit button', async () => {
    const props = {
      ...defaultProps,
      editingIndex: 0
    };
    MatchSettingService.getMatchsettingByMatchId.mockReturnValue([
      {
        BettypeId: 1,
        Maxbet: null,
        Spread: null,
      },
      {
        BettypeId: 2,
        Maxbet: 3,
        Spread: 3,
      }
    ]);
    const { container } = render(<MatchSettingPanel {...props} />);

    await act(async () => {
      await promise;
    });
    const editButton = container.querySelectorAll('.icon-edit');

    fireEvent.click(editButton[0]);

    expect(container.querySelectorAll('.input-invalid').length).toBe(2);
    expect(container.firstChild).toMatchSnapshot();
  });


  it('update state when click cancel button', async () => {
    const props = {
      ...defaultProps,
      editingIndex: 0
    };
    MatchSettingService.getMatchsettingByMatchId.mockReturnValue([
      {
        BettypeId: 2,
        Maxbet: 3,
        Spread: 3,
      }
    ]);
    const { container } = render(<MatchSettingPanel {...props} />);

    await act(async () => {
      await promise;
    });
    const editButton = container.querySelector('.icon-edit');

    fireEvent.click(editButton);

    expect(container.querySelector('.icon-cancel')).toBeInTheDocument();

    const cancelButton = container.querySelector('.icon-cancel');

    fireEvent.click(cancelButton);
    expect(container.querySelector('.icon-edit')).toBeInTheDocument();
  });
});
