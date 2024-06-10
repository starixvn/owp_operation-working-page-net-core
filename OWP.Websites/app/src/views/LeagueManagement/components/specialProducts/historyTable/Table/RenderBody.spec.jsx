import { Table } from 'reactstrap';
import { render } from '@testing-library/react';
import RenderBody from './RenderBody';

console.warn = jest.fn();
describe('test EditCell component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });
  const props = {
    dataRows: [
      {
        Product: 'MB of FT.HDP',
        Action: 'add',
        OldValue: null,
        NewValue: '100K',
        ModifyDate: '6/17/2020',
        ModifyBy: 'sa'
      }
    ]
  };
  it('render correct loading component', () => {
    const { getByText } = render(
      <Table>
        <RenderBody {...props} />
      </Table>
    );
    expect(getByText('MB of FT.HDP')).toBeInTheDocument();
  });
});
