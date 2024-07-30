import { Provider } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';
import configureMockStore from 'redux-mock-store';

import SpreadCell from './SpreadCell';

jest.mock('react-redux', () => {
  const actualReactRedux = jest.requireActual('react-redux');
  return {
    ...actualReactRedux,
    useSelector: jest.fn().mockImplementation(() => {
      return {
        specialProductData: [
          {
            Maxbet_FTHDP: 1,
            ProductId: 283,
            Remark: 'Vietnam’s N'
          }
        ]
      };
    })
  };
});

const middleWares = [];
const mockStore = configureMockStore(middleWares);
const initialState = {
  policyReducer: {
    policyData: {
      generalPolicy: {
        HasWrite: true
      }
    }
  }
};
const store = mockStore(initialState);

jest.spyOn(store, 'dispatch');

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('LastUpdatedCell component under unit test', () => {
  const props = { propKey: 'Remark', value: 1, productId: 283 };

  it('render component correct', () => {
    const { container } = render(
      <Wrapper>
        <SpreadCell {...props} />
      </Wrapper>
    );
    expect(container.querySelector('.input-value')).toBeInTheDocument();
  });

  it('should render input & handle blur when click div label', () => {
    const props = { propKey: '11', value: 1, productId: 283 };
    const { container, getByText } = render(
      <Wrapper>
        <SpreadCell {...props} />
      </Wrapper>
    );

    const input = container.querySelector('.input-field');
    fireEvent.change(input, { target: { value: 100 } });
    fireEvent.blur(input);

    expect(store.dispatch).toBeCalled();
  });
});
