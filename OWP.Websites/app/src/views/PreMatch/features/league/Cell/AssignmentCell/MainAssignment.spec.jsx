import { useSelector, useDispatch } from 'react-redux';
import { fireEvent, render, waitFor } from '@testing-library/react';
import MainAssignment from './MainAssignment';
import axios from 'axios';
import Spinner from '../../../../../components/Spinner';
import { noop } from 'lodash';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import { validateAssignment } from './AssignmentHelper';

jest.mock('axios');
jest.mock('./AssignmentHelper.jsx');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);

const setUp = props => {
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};

jest.mock('../../../../../components/Spinner');
Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());

jest.mock('../../../../../components/ToastrWrapper');
ToastrWrapper.showInfo.mockImplementation(() => noop());

describe('MainAssignment under test', () => {
  afterEach(() => {
    axios.get.mockReset();
    axios.post.mockReset();
    jest.clearAllMocks();
  });

  it('should call dispatch when blur on input, with validateResult = true', async () => {
    setUp({
      copy: {
        highlightedCell: {
          PreMatchLeagueId: 4,
          isTakeOver: true,
          isCopying: true
        },
        copiedValue: {
          OU: 'sa',
          HDP: 'sa'
        },
        isCopying: false
      },
      editor: {
        isEdit: true
      }
    });
    const props = {
      columnId: 'league',
      rowIndex: 0,
      dataSource: [
        {
          PreMatchLeagueId: 4,
          InfoSiteMatchId: undefined,
          league: 'abc'
        }
      ]
    };

    const responseData = {
      validateResult: true,
      valueAfterValidate: 'esther',
      valueBeforeValidate: 'esther'
    };
    validateAssignment.mockReturnValue(responseData);
    const { container } = render(<MainAssignment {...props} />);
    const input = container.querySelector('input');
    fireEvent.blur(input);
    await waitFor(() => expect(mockedDispatch).toBeCalledTimes(1));
  });

  it('should not call dispatch when blur on input, with validateResult = false', async () => {
    setUp({
      copy: {
        highlightedCell: {
          PreMatchLeagueId: 4,
          isTakeOver: true,
          isCopying: true
        },
        copiedValue: {
          OU: 'sa',
          HDP: 'sa'
        },
        isCopying: false
      },
      editor: {
        isEdit: true
      }
    });
    const props = {
      columnId: 'league',
      rowIndex: 0,
      dataSource: [
        {
          PreMatchLeagueId: 4,
          InfoSiteMatchId: undefined,
          league: 'abc'
        }
      ]
    };

    const responseData = {
      validateResult: false,
      valueAfterValidate: '',
      valueBeforeValidate: 'test'
    };
    validateAssignment.mockReturnValue(responseData);
    const { container } = render(<MainAssignment isShowIconPlus {...props} />);
    const input = container.querySelector('input');
    fireEvent.blur(input);
    await waitFor(() => expect(mockedDispatch).not.toHaveBeenCalled());
  });

  it('should render cell can edit', () => {
    setUp({
      copy: {
        highlightedCell: {
          PreMatchLeagueId: 4,
          isTakeOver: true,
          isCopying: true,
          copiedValue: {
            OU: 'sa',
            HDP: 'sa'
          }
        }
      }
    });
    const props = {
      columnId: 'league',
      rowIndex: 0,
      dataSource: [
        {
          PreMatchLeagueId: 4,
          InfoSiteMatchId: undefined
        }
      ],
      handleShowHideIconPlus: jest.fn()
    };

    const { container } = render(<MainAssignment {...props} />);
    expect(container.querySelector('.input-field')).toBeInTheDocument();
  });

  it('should call dispatch when blur on input and handleShowHideIconPlus = false, with condition input empty', () => {
    setUp({
      copy: {
        highlightedCell: {
          PreMatchLeagueId: 4,
          isTakeOver: true,
          isCopying: true
        },
        copiedValue: {
          OU: 'sa',
          HDP: 'sa'
        },
        isCopying: false
      },
      editor: {
        isEdit: true
      }
    });
    const props = {
      columnId: 'league',
      rowIndex: 0,
      dataSource: [
        {
          PreMatchLeagueId: 4,
          InfoSiteMatchId: undefined
        }
      ],
      handleShowHideIconPlus: jest.fn()
    };

    const { container } = render(
      <MainAssignment handleShowHideIconPlus isShowIconPlus {...props} />
    );
    const input = container.querySelector('input');
    fireEvent.blur(input);
    expect(mockedDispatch).toHaveBeenCalledTimes(2);
  });
});
