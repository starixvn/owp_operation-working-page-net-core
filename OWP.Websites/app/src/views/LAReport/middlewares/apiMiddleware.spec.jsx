import { queryDataWithParamsMiddleware } from './apiMiddleware';

const create = () => {
  const store = {
    dispatch: jest.fn()
  };
  const next = jest.fn();
  const invoke = action => queryDataWithParamsMiddleware(store)(next)(action);
  return { store, next, invoke };
};

describe('apiMiddleware', () => {
  it('passes through non-function action', () => {
    const { next, invoke } = create();
    const action = { type: 'TEST' };
    invoke(action);
    expect(next).toHaveBeenCalledWith(action);
  });
});
