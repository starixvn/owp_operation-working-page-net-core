import { useState } from 'react';
import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import ModalCommon from './ModalCommon';
import { Modal } from 'reactstrap';

import { setImmediate } from 'timers';
const flushPromises = () => {
  return new Promise(resolve => setImmediate(resolve));
};

const createProps = () => {
  const className = 'deleted-league';
  const title = 'View Deleted Leagues';
  const size = 'xl';
  return { className, title, size };
};
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  useEffect: jest.fn(),
  createRef: jest.fn()
}));
const setState = jest.fn();

const setup = () => {
  useState.mockImplementation(init => [init, setState]);
};
describe('ModalCommon under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render Modal', () => {
    setup();
    const props = createProps();
    const { container } = render(<ModalCommon {...props} />);
    const element = container.querySelector('.deleted-league');
    expect(element).toBeInTheDocument();
  });

  it('when click label,should execute setModalIsOpen , ', () => {
    setup(true);
    const props = createProps('LeagueNameTest', 1, true, 1);
    const { container } = render(<ModalCommon {...props} />);
    const button = container.querySelector('label');
    fireEvent.click(button);
    expect(setState).toHaveBeenCalledTimes(1);
  });
});
