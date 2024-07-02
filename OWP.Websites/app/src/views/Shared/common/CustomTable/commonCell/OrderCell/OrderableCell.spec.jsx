import OrderableCell from './OrderableCell';
import { render, fireEvent } from '@testing-library/react';

let container = null;

beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
});

afterEach(() => {
  document.body.removeChild(container);
  container = null;
});

describe('Event Checking OrderableCell under test', () => {
  it('OrderableCell should callback function onOrder DESC when click', () => {
    const mockCallBack = jest.fn();

    const { container } = render(
      <OrderableCell
        headerName='LeagueName'
        color='none'
        orderType='ORDER_DESC'
        onOrder={mockCallBack}
      />
    );

    const button = container.querySelector('.sort-icon');

    fireEvent.click(button, { bubbles: true });

    expect(mockCallBack.mock.calls.length).toEqual(1);
  });
  it('OrderableCell should callback function onOrder Asc when click', () => {
    const mockCallBack = jest.fn();
    const { container } = render(
      <OrderableCell
        headerName='LeagueName'
        color='none'
        orderType='ORDER_ASC'
        onOrder={mockCallBack}
      />
    );

    const button = container.querySelector('.sort-icon');

    fireEvent.click(button, { bubbles: true });

    expect(mockCallBack.mock.calls.length).toEqual(1);
  });
});
