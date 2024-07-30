import TradingInfoRow from './TradingInfoRow';
import { fireEvent, render } from '@testing-library/react';


const bettypesOpition = [
  { value: 1, label: 'bettype 1' },
  { value: 2, label: 'bettype 2' },
  { value: 3, label: 'bettype 3' },
  { value: 4, label: 'bettype 4' },
  { value: 5, label: 'bettype 5' }
];

const matchSetting = {
  bettype: { value: 1, label: 'bettype 1' },
  maxbet: 1,
  spread: 2
};

describe('<TradingInfoRow/>', () => {
  let props;

  beforeAll(() => {
    props = {
      matchSetting: matchSetting,
      bettypesOpition: bettypesOpition,
      handleMatchSettingChange: jest.fn(),
      handleDeleteMatchSetting: jest.fn()
    };
  }, 3000);

  it('update state when change bettype', () => {
    const { container, getByText } = render(
      <table>
        <tbody>
          <TradingInfoRow {...props} />
        </tbody>
      </table>
    );
    fireEvent.focus(container.querySelector('input'));
    fireEvent.keyDown(container.querySelector('input'), {
      key: 'ArrowDown',
      code: 40
    });
    fireEvent.click(getByText('bettype 2'));
    expect(getByText('bettype 2')).toBeInTheDocument();
    expect(props.handleMatchSettingChange).toBeCalled();
  });

  it('update state when change maxbet', () => {
    const { container } = render(
      <table>
        <tbody>
          <TradingInfoRow {...props} />
        </tbody>
      </table>
    );
    fireEvent.change(container.querySelectorAll('.input-field')[0], {
      target: {
        value: 123
      }
    });
    expect(container.querySelectorAll('.input-field')[0].value).toEqual('123');
    expect(props.handleMatchSettingChange).toBeCalled();
  });

  it('update state when change spread', () => {
    const { container } = render(
      <table>
        <tbody>
          <TradingInfoRow {...props} />
        </tbody>
      </table>
    );
    fireEvent.change(container.querySelectorAll('.input-field')[1], {
      target: {
        value: 0.5
      }
    });
    expect(container.querySelectorAll('.input-field')[1].value).toEqual('0.5');
    expect(props.handleMatchSettingChange).toBeCalled();
  });

  it('handleDeleteMatchSetting to be call', () => {
    const { container } = render(
      <table>
        <tbody>
          <TradingInfoRow {...props} />
        </tbody>
      </table>
    );
    fireEvent.click(container.querySelector('.delete-btn'));
    expect(props.handleDeleteMatchSetting).toBeCalled();
  });
});
