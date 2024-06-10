import { render } from '@testing-library/react';
import DeletedLeagueTable from './DeletedLeagueTable';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';

jest.mock('../../app/store');
describe('DeletedLeagueTable under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render `There is no data`, when not have deleted league', () => {
    const { container } = render(<DeletedLeagueTable leagueDeleted={[]} />);
    const element = container.querySelector('.no-data');
    expect(element).toBeInTheDocument();
    expect(element.textContent).toEqual('There is no data');
  });

  it('should render 1 table`, when have deleted league', () => {
    const store = configureStore()({ output: 10 });
    const { container } = render(
      <Provider store={store}>
        <DeletedLeagueTable
          leagueDeleted={[
            {
              PreMatchLeagueId: 9219,
              LeagueName: 'AUSTRALIA NEW SOUTH WALES PREMIER LEAGUE',
              DeletedBy: 'sa',
              DeletedTime: '2020-10-15T14:52:18.537+07:00'
            }
          ]}
        />
      </Provider>
    );
    const element = container.querySelector('.table-content');
    expect(element).toBeInTheDocument();
  });
});
