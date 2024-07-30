import { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';
import RowsExpression from './RowsExpression';
import MaxbetConst from '../../constants/MaxbetConst';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);

describe('RowsExpression under test', () => {
  const props = {
    hasWrite: true,
    actionType: {},
    models: [
      {
        item: {
          expression: {}
        },
        index: 1,
        modelsLength: 1,
        hasWrite: true,
        actionType: {},
        key: '1'
      }
    ],
    titleName: '',
    modelsLength: 1,
    modelExpressionDefault: {}
  };
  it('render correct Expression component', () => {
    const { container } = render(<RowsExpression {...props} />);
    expect(container.querySelector('.list-content')).toBeInTheDocument();
  });
});
