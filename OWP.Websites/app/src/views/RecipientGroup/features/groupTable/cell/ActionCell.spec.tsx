import { fireEvent, render, screen } from '@testing-library/react';
import { useState } from 'react';
import GroupPopup from '../../groupPopup/GroupPopup';
import { GroupDataSource } from '../../Type';
import ActionCell from './ActionCell';

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));

jest.mock('../../groupPopup/GroupPopup', () => {
  return jest.fn(() => 'hi');
});

const setState = jest.fn();
const setUp = (isOpenPopup: boolean = false) => {
  (useState as jest.Mock).mockImplementation(init => [isOpenPopup, setState]);
};

const data = {
  groupId: 1,
  groupName: 'LST'
} as GroupDataSource;

describe('test ActionCell', () => {
  beforeEach(() => {
    setUp();
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('isOpen=false, should render ActionCell and button, not render GroupPopup', () => {
    const { container } = render(<ActionCell {...data} />);
    expect(container.querySelector('.icon-edit')).toBeInTheDocument();
    expect(container.querySelector('.action-cell')).toBeInTheDocument();
  });

  it('click edit icon, should call setState', () => {
    const { container } = render(<ActionCell {...data} />);
    const element = container.querySelector('.icon-edit');
    fireEvent.click(element!);
    expect(setState).toHaveBeenCalledTimes(1);
  });

  it('isOpen=true, should render GroupPopup', () => {
    setUp(true);
    render(<ActionCell {...data} />);

    expect(screen.queryByText('hi')).toBeInTheDocument();
    expect(GroupPopup).toBeCalled();
  });
});
