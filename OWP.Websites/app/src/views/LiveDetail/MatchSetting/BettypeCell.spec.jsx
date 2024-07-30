import BettypeCell from './BettypeCell';
import { fireEvent, render } from '@testing-library/react';

describe('BettypeCell', () => {
  const handleBettypeChange = jest.fn();
  it('should render <BettypeCell/>', () => {
    const props = {
      isEditing: true,
      handleBettypeChange: jest.fn(),
      minorBettypeId: null,
      BettypeId: 1
    };
    const { container } = render(<table>
      <tbody>
        <tr>
          <BettypeCell {...props} />
        </tr></tbody></table>);

    expect(container.firstChild).toMatchSnapshot();
  });

  it('shoud disable when readonly', () => {
    const props = {
      isEditing: false,
      handleBettypeChange: jest.fn(),
      minorBettypeId: null,
      BettypeId: 1,
      isDisabled: true,
      data: {
        MatchOffers: [{ value: 1, label: '10s Accept' }]
      }
    };

    const { container } = render(<table>
      <tbody>
        <tr>
          <BettypeCell {...props} />
        </tr></tbody></table>);

    const selectElement = container.getElementsByClassName('select__single-value select__single-value--is-disabled');
    expect(selectElement.length).toBe(1);
  });

  it('shoud enable when has write permission', () => {
    const props = {
      isEditing: true,
      handleBettypeChange: jest.fn(),
      minorBettypeId: null,
      BettypeId: 1,
      isDisabled: false
    };
    const { container } = render(<table>
      <tbody>
        <tr>
          <BettypeCell {...props} />
        </tr></tbody></table>);

    const selectElement = container.getElementsByClassName('select__single-value select__single-value--is-disabled');
    expect(selectElement.length).toBe(0);
  });

  it('handleBettypeChange func tobe call', () => {
    const props = {
      isEditing: true,
      handleBettypeChange,
      minorBettypeId: 8,
      BettypeId: 8,
      data: {
        MatchOffers: [{ value: 1, label: '10s Accept' }]
      }
    };
    const { container, getByText } = render(<table>
      <tbody>
        <tr>
          <BettypeCell {...props} />
        </tr>
      </tbody></table>);

    fireEvent.focus(container.querySelector('input'));
    fireEvent.keyDown(container.querySelector('input'), { key: 'ArrowDown', code: 40 });

    fireEvent.click(getByText('O/U'));

    expect(handleBettypeChange).toHaveBeenCalledWith(3);
  });

  it('should render options correct when match is main', () => {
    const props = {
      isEditing: true,
      handleBettypeChange: handleBettypeChange,
      minorBettypeId: null,
      BettypeId: 1
    };
    const { container } = render(<table>
      <tbody>
        <tr>
          <BettypeCell {...props} />
        </tr>
      </tbody></table>);

    fireEvent.focus(container.querySelector('input'));
    fireEvent.keyDown(container.querySelector('input'), { key: 'ArrowDown', code: 40 });

    expect(container.firstChild).toMatchSnapshot();
  });

  it('should render options correct when match is minnor TotalBooking/numberOfConner', () => {
    const props = {
      isEditing: true,
      handleBettypeChange: handleBettypeChange,
      minorBettypeId: -3,
      BettypeId: -5
    };
    const { container } = render(<table>
      <tbody>
        <tr>
          <BettypeCell {...props} />
        </tr>
      </tbody></table>);

    fireEvent.focus(container.querySelector('input'));
    fireEvent.keyDown(container.querySelector('input'), { key: 'ArrowDown', code: 40 });
    expect(container.firstChild).toMatchSnapshot();
    expect(container.firstChild).toMatchSnapshot();
  });
});
