import { useState, createRef } from 'react';
import { act, fireEvent, render, waitFor } from '@testing-library/react';
import ActionCell from './ActionCell';
import { useDispatch, useSelector } from 'react-redux';
import axios from 'axios';
import Spinner from '../../../../components/Spinner';
import { noop } from 'lodash';
import ToastrWrapper from '../../../../components/ToastrWrapper';

jest.mock('axios');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));
jest.mock('../../../../components/Spinner');
Spinner.show.mockImplementation(() => noop);
Spinner.hide.mockImplementation(() => noop);
jest.mock('../../../app/store');
jest.mock('../../../../components/ToastrWrapper');

const promise = Promise.resolve();
const mockDispatch = jest.fn();
const setState = jest.fn();
const ref = { current: { value: 'Hello' } };
const mockToast = jest.fn();
//jest.setTimeout(20000);

const setup = (isContainsAssignment = true, toastrWrapperValue = true) => {
  const state = {
    copy: {
      isCopying: false,
      copiedValue: {
        HDP: '',
        OU: ''
      },
      highlightedCell: {
        PreMatchLeagueId: [1],
        rowId: 1,
        isTakeOver: false
      }
    }
  };
  axios.post.mockResolvedValue({
    data: [{ IsContainsAssignment: isContainsAssignment }, 2, 3]
  });
  useState.mockImplementation(init => [init, setState]);
  useDispatch.mockReturnValue(mockDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
  createRef.mockReturnValue(ref);
  const object = { value: toastrWrapperValue };
  ToastrWrapper.confirmYesNo.mockImplementation(() => object);
  ToastrWrapper.showError.mockImplementation(() => mockToast());
};
const createProps = (LeagueName, isShow, PreMatchLeagueId, InfoSiteMatchId) => {
  const dataSource = [
    {
      PreMatchLeagueId,
      LeagueName,
      isShow,
      InfoSiteMatchId
    }
  ];
  const rowIndex = 0;
  return { dataSource, rowIndex, columnId: 'Remark' };
};

describe('ActionCell under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
    axios.post.mockReset();
  });

  it('should render icon-trash', () => {
    setup();
    const props = createProps('LeagueNameTest', true, null, null);
    const { container } = render(<ActionCell {...props} />);
    const iconDelete = container.querySelector('.icon-trash');
    expect(iconDelete).toBeInTheDocument();
  });

  it('when row is match, should not render CopyButton', () => {
    setup();
    const props = createProps('LeagueNameTest', true, 1, 1);
    const { container } = render(<ActionCell {...props} />);
    const iconDelete = container.querySelector('.icon-trash');
    expect(iconDelete).toBeNull();
  });

  it('click TrashIcon, isContainsAssignment is true, should not call dispatch', async () => {
    setup();
    const props = createProps('LeagueNameTest', true, null, null);
    const { container } = render(<ActionCell {...props} />);
    const iconDelete = container.querySelector('.icon-trash');
    fireEvent.click(iconDelete);
    await waitFor(() => {
      expect(mockDispatch).toHaveBeenCalledTimes(0);
      expect(mockToast).toHaveBeenCalledTimes(1);
    });
  });

  it('click TrashIcon, isContainsAssignment is false,confirmResult.value = true should call dispatch 2 times', async () => {
    setup(false);
    const props = createProps('LeagueNameTest', true, 1, null);
    const { container } = render(<ActionCell {...props} />);
    const iconDelete = container.querySelector('.icon-trash');
    fireEvent.click(iconDelete);
    await waitFor(() => expect(mockDispatch).toHaveBeenCalledTimes(2));
  });

  it('click TrashIcon, isContainsAssignment is false,confirmResult.value = false, should call dispatch 0 times', async () => {
    setup(false, false);
    const props = createProps('LeagueNameTest', true, 1, null);
    const { container } = render(<ActionCell {...props} />);
    const iconDelete = container.querySelector('.icon-trash');
    fireEvent.click(iconDelete);
    await waitFor(() => expect(mockDispatch).toHaveBeenCalledTimes(0));
  });
});
