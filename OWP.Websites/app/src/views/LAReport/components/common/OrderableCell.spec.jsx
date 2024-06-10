import { fireEvent, render } from '@testing-library/react';
import OrderableCell from './OrderableCell';

let container = null;

beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
});

afterEach(() => {
  document.body.removeChild(container);
  container = null;
});

describe('LA OrderableCell under test', () => {
  it('OrderableCell should  callback function onOrder ORDER_DESC when click', () => {
    const mockCallBack = jest.fn();

    const { container } = render(
      <OrderableCell
        columnName='LA'
        color='none'
        orderType='ORDER_DESC'
        onOrder={mockCallBack}
      />
    );

    const button = container.querySelector('.sort-icon');
    fireEvent.click(button);

    expect(mockCallBack.mock.calls.length).toEqual(1);
  });
  it('OrderableCell should  callback function onOrder ORDER_ASC when click', () => {
    const mockCallBack = jest.fn();

    const { container } = render(
      <OrderableCell
        columnName='LA'
        color='none'
        orderType='ORDER_ASC'
        onOrder={mockCallBack}
      />
    );

    const button = container.querySelector('.sort-icon');
    fireEvent.click(button);

    expect(mockCallBack.mock.calls.length).toEqual(1);
  });
});
