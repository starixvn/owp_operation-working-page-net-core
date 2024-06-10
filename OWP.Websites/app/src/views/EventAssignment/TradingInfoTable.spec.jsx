import TradingInfoTable from './TradingInfoTable';
import { fireEvent, render } from '@testing-library/react';

const bettypes = [
  { BettypeId: 1, ShortName: 'HDP' },
  { BettypeId: 2, ShortName: 'O/U' },
  { BettypeId: 3, ShortName: 'bettype 3' },
  { BettypeId: 4, ShortName: 'bettype 4' }
];
describe('<TradingInfoTable/>', () => {
  const props = {
    bettypes: bettypes
  };

  it('init matchsetting correct', () => {
    const { container } = render(<TradingInfoTable {...props} />);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('init bettypesOpition correct', () => {
    const { container, getByText, getAllByText } = render(<TradingInfoTable {...props} />);
    fireEvent.focus(container.querySelector('input'));
    fireEvent.keyDown(container.querySelector('input'), { key: 'ArrowDown', code: 40 });
    expect(getByText('bettype 3')).toBeInTheDocument();
    expect(getByText('bettype 4')).toBeInTheDocument();
    expect(getAllByText('HDP').length).toEqual(2);
    expect(getAllByText('O/U').length).toEqual(2);

  });

  it('add new matchSetting success', () => {
    const { container } = render(<TradingInfoTable {...props} />);
    fireEvent.click(container.querySelector('.btn-add'));
    expect(container.firstChild).toMatchSnapshot();
  });
});
