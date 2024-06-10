import {
  fireEvent,
  render,
  screen,
  waitFor,
  waitForElementToBeRemoved
} from '@testing-library/react';
import Service from './Service';
import AuthorizeService from '../Authorize/Service';
import ResourceManagementService from '../ResourceManagement/Service';
import OddsCompiler from './OddsCompiler';
import Utils from '../Shared/Utils';
import mockEvents from './Table.spec.json';
import userEvent from '@testing-library/user-event';

jest.mock('./Service');
jest.mock('../Authorize/Service');
jest.mock('../ResourceManagement/Service');
jest.mock('../Shared/Utils');

describe('EventAssignment', () => {
  beforeEach(() => {
    const policy = { HasRead: true, HasWrite: true };
    const events = mockEvents;
    AuthorizeService.authorize.mockResolvedValue(policy);
    Service.query.mockResolvedValue(events);
    Service.exportExcel.mockResolvedValue('url');
    waitFor(() => {
      ResourceManagementService.getByRefName.mockResolvedValue([
        [
          { RefName: 'MatchStatus', Id: 4, Value: 'Cancel' },
          { RefName: 'MatchStatus', Id: 3, Value: 'Completed' },
          { RefName: 'MatchStatus', Id: 2, Value: 'In-play' },
          { RefName: 'MatchStatus', Id: 1, Value: 'Live' },
          { RefName: 'MatchStatus', Id: 5, Value: 'Postponed' },
          { RefName: 'MatchStatus', Id: 6, Value: 'Suspended' }
        ]
      ]);
      Utils.isYesterdayOrAfterForAssignment.mockReturnValue(true);
      Utils.buildMatchStatusOption.mockReturnValue([
        { label: 'In-play', value: 2 },
        { label: 'Live', value: 1 }
      ]);
      Utils.buildSelectedMatchStatus.mockReturnValue([1, 2]);
    });
  });

  it('should match snapshot on init', () => {
    const { container } = render(<OddsCompiler />);
    waitFor(() => {
      expect(container).toMatchSnapshot();
    });
  });

  it('when click Submit btn ,should call Utils, Service', async () => {
    const { container } = render(<OddsCompiler />);
    const icon = container.querySelector(`[class="icon-loading"]`);

    await waitForElementToBeRemoved(icon);
    const submit = container.querySelector('.button-submit');
    await userEvent.click(submit);

    expect(Utils.isYesterdayOrAfterForAssignment).toBeCalled();
    expect(Service.query).toBeCalled();
  });

  it('when click Export btn, should call Service exportExcel', async () => {
    global.window = Object.create(window);
    const url = 'http://dummy.com';
    Object.defineProperty(window, 'location', {
      value: {
        href: url
      }
    });
    const { container } = render(<OddsCompiler />);
    const icon = container.querySelector(`[class="icon-loading"]`);

    await waitForElementToBeRemoved(icon);
    fireEvent.click(screen.getByText('Export'));

    expect(Service.exportExcel).toBeCalled();
  });

  it('should render Loading, when waiting query server', () => {
    const { container } = render(<OddsCompiler />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
  });
});
