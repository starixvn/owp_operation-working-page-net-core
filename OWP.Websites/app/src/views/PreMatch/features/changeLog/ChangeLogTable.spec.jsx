import { fireEvent, render } from '@testing-library/react';
import ChangeLogTable from './ChangeLogTable';

describe('ChangeLogTable under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render `There is no data`, when not have logs', () => {
    const { container } = render(<ChangeLogTable logs={[]} />);
    const element = container.querySelector('.no-data');
    expect(element).toBeInTheDocument();
    expect(element.textContent).toEqual('There is no data');
  });
  it('should render 1 table`, when have logs', () => {
    const logs = [
      {
        Action: 'Update Other Assignment',
        LastModified: '2020-10-12T02:34:44.803+07:00',
        LastModifiedInString: '10/12/2020 02:34 PM',
        LeagueName: 'CLUB FRIENDLY',
        ModifiedBy: 'sa',
        NewValue: 'HDP: #starix14 bajd',
        OldValue: 'HDP: #sa'
      }
    ];
    const { container } = render(<ChangeLogTable logs={logs} />);
    const element = container.querySelector('.table-content');
    expect(element).toBeInTheDocument();
  });
});
