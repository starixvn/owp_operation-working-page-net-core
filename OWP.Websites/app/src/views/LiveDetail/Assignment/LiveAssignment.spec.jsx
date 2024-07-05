import LiveAssignment from './LiveAssignment';
import AssignmentService from './Service';
import MatchSettingService from '../MatchSetting/Service';
import LiveDetailService from './LiveDetailService';
import { act, fireEvent, render } from '@testing-library/react';

jest.mock('./Service');
jest.mock('../MatchSetting/Service');
jest.mock('./LiveDetailService');

const promise = Promise.resolve();

describe('LiveAssignment', () => {
  const dataSource = [];

  const defaultProps = {
    matchId: 1,
    policy: { HasRead: true, HasWrite: true }
  };

  const matchSettings = [
    {
      Bettype: 1,
      DisplayName: 'HDP',
      IsFromMaxbetSetting: true,
      Maxbet: null,
      MinorBettypeId: 0,
      OWPMatchId: 279044,
      Spread: null
    }
  ];

  const assignments = [
    {
      AssignmentId: 211849,
      AssignmentNote: null,
      AssignmentType: 1,
      Bettype: 1,
      DisplayName: 'HDP',
      MinorBettypeId: 0,
      OWPMatchId: 279044,
      UserId: 8,
      Username: 'superman'
    }
  ];

  beforeEach(() => {
    MatchSettingService.query.mockReturnValue(matchSettings);
    AssignmentService.query.mockReturnValue(assignments);
    LiveDetailService.getAll.mockReturnValue(dataSource);
  });

  it('should hide TableAssignment when click button Toggle', async () => {
    const { getByTestId, container } = render(<LiveAssignment {...defaultProps} />);
    const button = getByTestId('toggle-testId');

    fireEvent.click(button);

    await act(async () => {
      await promise;
    })

    expect(container.getElementsByClassName('card-collapse').length).toBe(1)
  });
});
