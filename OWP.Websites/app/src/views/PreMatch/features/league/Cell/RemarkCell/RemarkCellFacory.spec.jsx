import { useDispatch, useSelector } from 'react-redux';
import { render } from '@testing-library/react';
import RemarkCellFactory from './RemarkCellFactory';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('../../../../app/store');

const mockDispatch = jest.fn();
const setup = () => {
  const state = {
    policy: {
      permission: {
        HasWrite: true
      }
    },
    dayShift: {
      isExpandAll: true
    }
  };
  useDispatch.mockReturnValue(mockDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
};

const createProps = (InfoSiteMatchId, isEditing) => {
  const dataSource = [
    {
      InfoSiteMatchId,
      LeagueName: 'LeagueNameTest',
      PreMatchLeagueId: 1,
      isExpand: true,
      MatchCount: 1
    }
  ];
  const rowIndex = 0;
  return { dataSource, rowIndex, isEditing, columnId: 'Remark' };
};

describe('RemarkCellFacory under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('when edit mode, should render EditableRemark', () => {
    setup();
    const props = createProps(null, true);
    const { container } = render(<RemarkCellFactory {...props} />);
    const element = container.querySelector('.input-field');
    expect(element).toBeInTheDocument();
  });

  it('when view mode, should render ReadOnlyRemark', () => {
    setup();
    const props = createProps(null, false);
    const { container } = render(<RemarkCellFactory {...props} />);
    const element = container.querySelector('#Remark0');
    expect(element).toBeInTheDocument();
  });

});
