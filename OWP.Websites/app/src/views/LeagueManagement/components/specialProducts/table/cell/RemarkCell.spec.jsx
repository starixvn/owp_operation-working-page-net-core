import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureMockStore from 'redux-mock-store';

import RemarkCell from './RemarkCell';

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

describe('RemarkCell component under unit test', () => {
  it('render component correct', () => {
    const props = { productId: 11, value: '1' };
    const { container, getByText } = render(
      <Wrapper>
        <RemarkCell {...props} />
      </Wrapper>
    );
    expect(getByText('1')).toBeInTheDocument();
  });

  it('should render input when click div remark', () => {
    const props = { propKey: '11', value: '1', productId: 12 };
    const { container, getByText } = render(
      <Wrapper>
        <RemarkCell {...props} />
      </Wrapper>
    );

    const remarkElement = container.querySelector('.remark');

    fireEvent.click(remarkElement);

    expect(container.querySelector('.input-field')).toBeInTheDocument();
  });

  it('should render input when click div label', () => {
    const props = { propKey: '11', value: '1', productId: 12 };
    const { container, getByText } = render(
      <Wrapper>
        <RemarkCell {...props} />
      </Wrapper>
    );

    const labelElement = container.querySelector('label');

    fireEvent.click(labelElement);

    expect(container.querySelector('.input-field')).toBeInTheDocument();
  });

  it('should render input & handle blur when click div label', () => {
    const props = { propKey: '11', value: '1', productId: 283 };
    const { container, getByText } = render(
      <Wrapper>
        <RemarkCell {...props} />
      </Wrapper>
    );

    fireEvent.click(container.querySelector('.remark'));

    const input = container.querySelector('.input-field');
    fireEvent.change(input, { target: { value: 'abc' } });
    fireEvent.blur(input);

    expect(input).not.toBeInTheDocument();
    expect(store.dispatch).toBeCalled();
  });
});
