import { useState, useEffect } from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import HeaderCommon from './HeaderCommon';
import ToastrWrapper from '../../../components/ToastrWrapper';

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  useEffect: jest.fn(),
  createRef: jest.fn()
}));

const setState = jest.fn();
const mockedWindow = jest.fn();
const mockedToast = jest.fn();
const mockedEffect = jest.fn();

const setUp = initState => {
  useState.mockImplementation(() => [initState, setState]);
  window.open = mockedWindow;
  ToastrWrapper.showInfo = mockedToast;
  useEffect.mockImplementationOnce(mockedEffect);
};

const createProps = (title, lastUpdate) => {
  return { title, lastUpdate };
};

describe('HeaderCommon under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render correct Title', () => {
    setUp({
      isShow: true,
      canClick: true
    });
    const props = createProps(
      'NIGHTSHIFT',
      '(Last Updated at September 18, 2020 2:42 PM by sa)'
    );
    const { container } = render(<HeaderCommon {...props} />);
    const element = container.querySelector('.title');
    expect(element).toBeInTheDocument();
    expect(element.textContent).toEqual('NIGHTSHIFT');
  });
  it('should render correct Last Updated ', () => {
    setUp({
      isShow: true,
      canClick: true
    });

    const props = createProps(
      'NIGHTSHIFT',
      '(Last Updated at September 18, 2020 2:42 PM by sa)'
    );
    const { container } = render(<HeaderCommon {...props} />);
    const element = container.querySelector('.lastUpdate');
    expect(element).toBeInTheDocument();
    expect(element.textContent).toEqual(
      '(Last Updated at September 18, 2020 2:42 PM by sa)'
    );
  });
  it('should render btn Go to My Tickets ', async () => {
    setUp({
      isShow: true,
      canClick: true
    });
    const props = createProps(
      'NIGHTSHIFT',
      '(Last Updated at September 18, 2020 2:42 PM by sa)'
    );
    const { container } = render(<HeaderCommon {...props} />);
    const element = container.querySelector('.myTicket');
    expect(element).toBeInTheDocument();
    expect(element.textContent).toEqual('Go to My Tickets');
  });

  it('should show Toast.showInfo', async () => {
    setUp({
      isShow: true,
      canClick: false
    });
    const props = createProps(
      'NIGHTSHIFT',
      '(Last Updated at September 18, 2020 2:42 PM by sa)'
    );
    const { container } = render(<HeaderCommon {...props} />);
    const element = container.querySelector('.myTicket');
    fireEvent.click(element);
    expect(mockedToast).toBeCalledTimes(1);
  });
});
