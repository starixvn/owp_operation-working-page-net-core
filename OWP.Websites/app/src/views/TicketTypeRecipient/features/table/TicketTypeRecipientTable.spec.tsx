import { useSelector, useDispatch } from 'react-redux';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { TicketTypeRecipientDataSource } from '../TypeNative';
import TicketTypeRecipientTable from './TicketTypeRecipientTable';
import usePermission from '../../../Shared/customHook/usePermission';
import ActionCell from './../cell/ActionCell';
import { render, screen } from '@testing-library/react';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

jest.mock('../../../Shared/customHook/usePermission');
jest.mock('../../../components/Loading', () => {
  return jest.fn(() => 'Loading');
});
const mockedDispatch = jest.fn();
(useDispatch as jest.Mock).mockReturnValue(mockedDispatch);
(usePermission as jest.Mock).mockReturnValue({
  loading: false
});

describe('TicketTypeRecipientTable under test', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    jest.resetModules();
  });

  it('should show <Loading/>', () => {
    (useSelector as jest.Mock).mockImplementation(selectorFn =>
      selectorFn({
        ticketTypeRecipient: {
          loading: REQUEST_STATUS.pending,
          entities: [] as TicketTypeRecipientDataSource[]
        }
      })
    );
    render(<TicketTypeRecipientTable />);
    expect(screen.getByText('Loading')).toBeInTheDocument();
  });

  it('should dispatch once when render', () => {
    (useSelector as jest.Mock).mockImplementation(selectorFn =>
      selectorFn({
        ticketTypeRecipient: {
          loading: REQUEST_STATUS.pending,
          entities: [] as TicketTypeRecipientDataSource[]
        }
      })
    );
    render(<TicketTypeRecipientTable />);
    expect(useDispatch).toBeCalledTimes(1);
  });

  it('should render table successfully when data is available', () => {
    (useSelector as jest.Mock).mockImplementation(selectorFn =>
      selectorFn({
        ticketTypeRecipient: {
          loading: REQUEST_STATUS.idle,
          entities: [
            {
              ticketType: {
                typeId: 1,
                typeName: 'type1'
              },
              subtypes: [
                {
                  subtypeId: 1,
                  typeId: 1,
                  subtypeName: 'Liam'
                },
                {
                  subtypeId: 2,
                  typeId: 2,
                  subtypeName: 'Liam2'
                }
              ],
              recipients: [
                {
                  ticketTypeRecipientId: 1,
                  recipientId: 1,
                  recipientType: 1,
                  recipientName: 'sa',
                  recipientNickname: 'sa_nickname',
                  subtypeName: 'Liam'
                }
              ]
            }
          ]
        }
      })
    );

    render(<TicketTypeRecipientTable />);
    expect(screen.findByText('sa_nickname')).toBeInTheDocument;
    expect(screen.findByText('abc subtype')).toBeInTheDocument;
    expect(screen.findByText('Liam')).toBeInTheDocument;
  });
});
