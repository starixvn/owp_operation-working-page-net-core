import { fireEvent, render, waitFor } from '@testing-library/react';
import UserService from '../Shared/User/UserService';
import Table from './Table';
import events from './Table.spec.json';
import Service from './Service';
import userEvent from '@testing-library/user-event';
import Toastr from '../components/ToastrWrapper';

jest.mock('../components/ToastrWrapper');
jest.mock('../Shared/User/UserService');
jest.mock('./Service', () => {
  return {
    query: jest.fn(() => {}),
    insertOdds: jest.fn(() => {}),
    deleteOdds: jest.fn(() => {}),
    updateOdds: jest.fn(() => {}),
    updateMarket: jest.fn(() => {}),
    exportExcel: jest.fn(() => {})
  };
});

Toastr.confirm = jest.fn(() => {
  return {
    value: 'Ok'
  };
});
const props = {
  policy: { HasRead: true, HasWrite: true },
  matchSettingData: [
    {
      Bettype: 1,
      Spread: 0.2
    },
    {
      Bettype: 2,
      Spread: 0.25
    }
  ],
  events: events,
  pageSize: 200,
  pageIndex: 0,
  columnFilters: [],
  updateDataSource: jest.fn(),
  columnFiltersChange: jest.fn(),
  onOddsChange: jest.fn()
};

describe('Table component', () => {
  beforeEach(() => {
    const user = {
      Skype: '2312',
      Extension: '12',
      Phone: '0999',
      UserId: 8,
      UserName: 'sa',
      NickName: 'superman'
    };
    waitFor(() => {
      UserService.getInfoByUserId.mockResolvedValue(user);
    });
  });
  afterEach(() => {
    props.updateDataSource.mockReset();
    props.columnFiltersChange.mockReset();
    props.onOddsChange.mockReset();

    Service.query.mockReset();
    Service.insertOdds.mockReset();
    Service.deleteOdds.mockReset();
    Service.updateOdds.mockReset();
    Service.updateMarket.mockReset();
    Service.exportExcel.mockReset();
  });

  it('should render 1 table, render MarketCell', async () => {
    const { container } = render(<Table {...props} />);
    const marketInput = container.querySelector('.text-market');
    await userEvent.click(marketInput);
    await userEvent.keyboard('Liam Test');
    fireEvent.blur(marketInput);
    expect(Service.updateMarket).toBeCalledTimes(1);
    expect(marketInput).toBeInTheDocument();
    const tableBlock = container.querySelector('.table-block');
    expect(tableBlock).toBeInTheDocument();
  });

  it('should render 1 table,call deleteOdds', async () => {
    const { container } = render(<Table {...props} />);

    const oddsInput = container
      .querySelector('.cell-draw')
      .querySelector('.cell-input')
      .querySelector('input');

    await userEvent.type(oddsInput, '3');
    fireEvent.blur(oddsInput);
    expect(Service.deleteOdds).toBeCalledTimes(1);
  });

  it('should render 1 table, render OddsCell', async () => {
    const { container } = render(<Table {...props} />);
    const oddsInput = container
      .querySelector('.cell-handicap-home')
      .querySelector('.cell-input')
      .querySelector('input');

    await userEvent.click(oddsInput);
    await userEvent.keyboard('1');
    fireEvent.blur(oddsInput);

    expect(oddsInput).toBeInTheDocument();
    expect(props.onOddsChange).toBeCalled();
  });

  it('should render 1 table,onChange OddsRows ', async () => {
    Service.updateOdds.mockReset();
    const { container } = render(<Table {...props} />);
    const oddsInput2 = container
      .querySelector('.cell-handicap-home')
      .querySelector('.cell-input')
      .querySelector('input');
    const oddsInput3 = container
      .querySelector('.cell-handicap')
      .querySelector('.cell-input')
      .querySelector('input');

    const oddsInput4 = container
      .querySelector('.cell-over-under')
      .querySelector('.cell-input')
      .querySelector('input');
    const oddsInput6 = container
      .querySelector('.cell-under')
      .querySelector('.cell-input')
      .querySelector('input');

    const oddsInput7 = container
      .querySelector('.cell-one')
      .querySelector('.cell-input')
      .querySelector('input');
    const oddsInput8 = container
      .querySelector('.cell-two')
      .querySelector('.cell-input')
      .querySelector('input');

    fireEvent.change(oddsInput4, { target: { value: '0.25' } });
    fireEvent.blur(oddsInput4);
    fireEvent.change(oddsInput6, { target: { value: '1' } });
    fireEvent.blur(oddsInput6);

    fireEvent.change(oddsInput7, { target: { value: '1' } });
    fireEvent.blur(oddsInput7);
    fireEvent.change(oddsInput8, { target: { value: '1' } });
    fireEvent.blur(oddsInput8);

    fireEvent.change(oddsInput2, { target: { value: '1' } });
    fireEvent.blur(oddsInput2);
    fireEvent.change(oddsInput3, { target: { value: '-0.25' } });
    fireEvent.blur(oddsInput3);

    expect(Service.updateOdds).toBeCalledTimes(2);
  });

  it('should render 1 table,OddsRows on delete', async () => {
    const { container } = render(<Table {...props} />);
    const oddsInput = container.querySelector('.btn-delete');
    await userEvent.click(oddsInput);

    expect(oddsInput).toBeInTheDocument();
    expect(Service.deleteOdds).toBeCalledTimes(1);
  });
});
