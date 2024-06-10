import FlagWrapper from './FlagWrapper';
import { checkLeague } from './FlagLogic';
import { useDispatch } from 'react-redux';
import { render } from '@testing-library/react';

jest.mock('./FlagLogic.jsx');
const mockedDispatch = jest.fn();
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

useDispatch.mockReturnValue(mockedDispatch);
const onChangeGeneralArea = jest.fn();
const createProps = isExpand => {
  return {
    teamName: {
      homeName: 'Arsenal (V)',
      awayName: 'AC Milan (V)'
    },
    leagueName: 'PremierLeague',
    isExpand: isExpand,
    onChangeGeneralArea: onChangeGeneralArea
  };
};

describe('test FlagWrapper', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('isExpand is true, render FlagComponent', () => {
    const props = createProps(true);
    const responseData = {
      isHaveFlag: true
    };
    checkLeague.mockReturnValue(responseData);
    const { container } = render(<FlagWrapper {...props} />);
    const flagBlock = container.getElementsByClassName('flag-block');
    const iconToggle = container.getElementsByClassName('icon-toggle');

    expect(flagBlock.length).toEqual(1);
    expect(iconToggle.length).toEqual(1);
  });

  it('isExpand is true and isHaveFlag is true, render ChevronDoubleUpIcon', () => {
    const props = createProps(true);
    const responseData = {
      isHaveFlag: true
    };
    checkLeague.mockReturnValue(responseData);
    const { container } = render(<FlagWrapper {...props} />);
    const iconToggle = container.getElementsByClassName('icon-expand');
    expect(iconToggle.length).toEqual(1);
  });

  it('isExpand is true and isHaveFlag is true, render ChevronDoubleDownIcon', () => {
    const props = createProps(false);
    const responseData = {
      isHaveFlag: true
    };
    checkLeague.mockReturnValue(responseData);
    const { container } = render(<FlagWrapper {...props} />);
    const iconToggle = container.getElementsByClassName('icon-collapse');
    expect(iconToggle.length).toEqual(1);
  });

  it('isHaveFlag is false return null', () => {
    const props = createProps(false);
    const responseData = {
      isHaveFlag: false
    };
    checkLeague.mockReturnValue(responseData);
    const { container } = render(<FlagWrapper {...props} />);
    expect(container.getElementsByClassName('icon-collapse').length).toEqual(0);
    expect(container.getElementsByClassName('icon-expand').length).toEqual(0);
  });
});
