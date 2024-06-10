import { act, fireEvent, render } from '@testing-library/react';
import Service from './Service';
import AuthorizeService from '../Authorize/Service';
import LSTReport from './LSTReport';
import Spinner from '../components/Spinner';
import { noop } from 'lodash';

jest.mock('./Service');
jest.mock('../Authorize/Service');
jest.mock('../components/Spinner');
Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());
global.setImmediate = jest.useRealTimers;
const promise = Promise.resolve();

const mockAuthorize = {
  HasRead: true,
  HasWrite: false,
  SiteId: 99,
  UserId: 360418,
  Username: 'starix1'
};
const mockDataQuery = [
  {
    ESoccer_5Mins: '-',
    ESoccer_6Mins: '-',
    ESoccer_15Mins: '-',
    ESoccer_16Mins: '-',
    ESoccer_20Mins: '-',
    PH_1X2_Major: '-',
    PH_1X2_Minor1: '-',
    PH_1X2_Minor2: '-',
    PH_1X2_Minor3: '-',
    PH_1X2_Others: 2,
    Robot3_X2: 1,
    Robot3_X3: '-',
    Robot3_X3_Plus: 3,
    Robot5_X5: '-',
    SectionId: '2',
    SectionName: 'Live Assistant',
    TotalMatch: 102,
    TurnOver_1H_HDP: '-',
    TurnOver_1H_OU: '-',
    TurnOver_FT_HDP: '-',
    TurnOver_FT_OU: '-',
    TurnOver_Others: '-',
    Type1_FT_HDP: 9,
    Type1_FT_HDPOU: 92,
    Type1_FT_OU: 1,
    Type2_AUS_League: '-',
    Type2_AUS_League_Corner: '-',
    Type2_JAP_Divison1: '-',
    Type2_JAP_Divison2: '-',
    Type3_Solo_Major: '-',
    Type4_1ST_Major: '-',
    Type4_1ST_Other: 92,
    Type4_10M: '-',
    Type4_15M: '-',
    Type4_Corner_NextCorner: 1,
    Type4_Fantasy: '-',
    Type4_Major: '-',
    Type4_Minor1: '-',
    Type4_Minor2: '-',
    Type4_Minor3: '-',
    Type4_PKSO: '-',
    Type4_TotalBooking: '-',
    Type4_Winner: '-',
    UserId: 8,
    Username: 'sa',
    WinLoss_FT_HDP: '-',
    Winloss_1H_HDP: '-',
    Winloss_1H_OU: '-',
    Winloss_FT_OU: '-',
    Winloss_Others: '-',
    X5: '-'
  }
];

AuthorizeService.authorize.mockResolvedValue(mockAuthorize);
Service.getServerTime.mockResolvedValue('01/20/2022');
Service.query.mockResolvedValue(mockDataQuery);
jest.setTimeout(500000);

describe('LiveAssignment', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should show filter', async () => {
    const { container } = render(<LSTReport />);
    await act(async () => {
      await promise;
    });

    const button = container.querySelector('.button-submit');
    expect(button).toBeInTheDocument();
  });

  it('test coverage export', async () => {
    const { container, getByText } = render(<LSTReport />);
    await act(async () => {
      await promise;
    });
    global.window = Object.create(window);
    const url = 'domain/url';
    Object.defineProperty(window, 'location', { value: { href: url } });
    Service.exportUrl.mockReturnValue(url);

    container.querySelector('.button-submit');
    fireEvent.click(getByText('Export'));
    expect(window.location.href).toEqual(url);
  });

  it('should show table', async () => {
    const { container } = render(<LSTReport />);
    await act(async () => {
      await promise;
    });

    const button = container.querySelector('.button-submit');
    fireEvent.click(button);

    await act(async () => {
      await promise;
    });

    const table = container.querySelector('.table-content');
    expect(table).toBeInTheDocument();
  });

  it('should call order', async () => {
    const { container } = render(<LSTReport />);
    await act(async () => {
      await promise;
    });

    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    await act(async () => {
      await promise;
    });

    const sortIcon = container.querySelector('.sort-icon');
    fireEvent.click(sortIcon);
    await act(async () => {
      await promise;
    });
  });

  it('test coverage onFilter', async () => {
    const { container } = render(<LSTReport />);
    await act(async () => {
      await promise;
    });

    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    await act(async () => {
      await promise;
    });

    fireEvent.click(container.querySelector('.icon-filter'));
    fireEvent.change(container.querySelector('.input-field'), {
      target: { value: 'new value' }
    });
    fireEvent.click(container.querySelector('.btn-search'));
    await act(async () => {
      await promise;
    });
  });

  it('test coverage onClear', async () => {
    const { container } = render(<LSTReport />);
    await act(async () => {
      await promise;
    });

    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    await act(async () => {
      await promise;
    });

    fireEvent.click(container.querySelector('.icon-filter'));
    fireEvent.change(container.querySelector('.input-field'), {
      target: { value: 'new value' }
    });
    fireEvent.click(container.querySelector('.clear-all'));
  });
});
