import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import TVList from './TVList';
import '@testing-library/jest-dom';

const middleWares = [];
const mockStore = configureStore(middleWares);

describe('test Header component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  const props = {
    onClose: jest.fn()
  };

  it('should render Loading component', () => {
    const initialState = {
      tvListReducer: {
        tvListPending: true
      },
      policyReducer: {
        policyData: {
          tvListPolicy: {
            HasRead: true,
            HasWrite: true
          }
        }
      }
    };
    const store = mockStore(initialState);
    const Wrapper = ({ children }) => (
      <Provider store={store}>{children}</Provider>
    );

    const { getByText, container } = render(
      <Wrapper>
        <TVList {...props} />
      </Wrapper>
    );
    expect(container.querySelector('.loading-process')).toBeInTheDocument();
  });

  it('should render Dialog ', () => {
    const initialState = {
      tvListReducer: {
        tvListPending: false,
        tvListExpression: [
          {
            key: 'ab',
            value: 'abc',
            expression: {
              label: 'Contains',
              value: 1
            }
          }
        ]
      },
      policyReducer: {
        policyData: {
          tvListPolicy: {
            HasRead: true
          }
        }
      }
    };
    const store = mockStore(initialState);
    const Wrapper = ({ children }) => (
      <Provider store={store}>{children}</Provider>
    );

    const { getByText } = render(
      <Wrapper>
        <TVList {...props} />
      </Wrapper>
    );

    expect(getByText('TV list')).toBeInTheDocument();
  });

  it('should render Dialog & handle save', () => {
    const initialState = {
      tvListReducer: {
        tvListPending: false,
        tvListExpression: [
          {
            key: 'ab',
            value: 'abc',
            expression: {
              label: 'Contains',
              value: 1
            }
          }
        ]
      },
      policyReducer: {
        policyData: {
          tvListPolicy: {
            HasRead: true,
            HasWrite: true
          }
        }
      }
    };
    const store = mockStore(initialState);
    const Wrapper = ({ children }) => (
      <Provider store={store}>{children}</Provider>
    );

    jest.spyOn(store, 'dispatch');

    const { getByText } = render(
      <Wrapper>
        <TVList {...props} />
      </Wrapper>
    );

    const save = getByText('Save');
    fireEvent.click(save);

    expect(store.dispatch).toBeCalled();
  });
});
