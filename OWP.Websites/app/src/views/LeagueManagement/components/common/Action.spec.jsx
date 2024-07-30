import Action from './Action';
import { render, fireEvent } from '@testing-library/react';
import MaxbetConst from '../../constants/MaxbetConst';
import { useSelector, useDispatch } from 'react-redux';

const crudActionType = type => ({
  ADD: `${type}_ADD`,
  UPDATE: `${type}_UPDATE`,
  DELETE: `${type}_DELETE`
});
const actionType = crudActionType('TVList');

jest.mock('react-redux', () => {
  const actualReactRedux = jest.requireActual('react-redux');
  return {
    ...actualReactRedux,
    useDispatch: jest.fn()
  };
});
const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);

const modelDefault = {
  index: 1,
  modelsLength: 2,
  hasWrite: true,
  actionType,
  modelExpressionDefault: {
    tv: {
      expression: MaxbetConst.expressionOption[0],
      value: ''
    }
  }
};
describe('Action test', () => {
  it('Should render button Add when count tvList less Maxlength=20 and Has permission write', () => {
    const { container } = render(<Action {...modelDefault} />);
    const icon = container.querySelector('.icon-add');
    expect(icon).toBeInTheDocument();

    fireEvent.click(icon);
    expect(mockedDispatch).toBeCalled();
  });

  it('Should not render button Add when count tvList equal Maxlength=20', () => {
    const props = {
      ...modelDefault,
      index: 19,
      modelsLength: 20
    };
    const { container } = render(<Action {...props} />);
    expect(container.querySelector('.icon-add')).not.toBeInTheDocument();
  });

  it('Should not render button Add when Has permission read', () => {
    const props = {
      ...modelDefault,
      hasWrite: false
    };
    const { container } = render(<Action {...props} />);
    expect(container.querySelector('.icon-add')).not.toBeInTheDocument();
  });

  it('Should render button Delete when count tvList more than 1 and Has permission write', () => {
    const { container } = render(<Action {...modelDefault} />);
    const icon = container.querySelector('.icon-delete');
    expect(icon).toBeInTheDocument();

    fireEvent.click(icon);
    expect(mockedDispatch).toBeCalled();
  });

  it('Should not render button Delete when count tvList equal 1', () => {
    const props = {
      ...modelDefault,
      modelsLength: 1
    };
    const { container } = render(<Action {...props} />);
    expect(container.querySelector('.icon-delete')).not.toBeInTheDocument();
  });
  it('Should not render button Delete when Has permission read', () => {
    const props = {
      ...modelDefault,
      hasWrite: false
    };
    const { container } = render(<Action {...props} />);
    expect(container.querySelector('.icon-delete')).not.toBeInTheDocument();
  });
});
