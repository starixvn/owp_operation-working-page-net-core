/* eslint-disable no-warning-comments */
import getPermission from '../../Shared/permission/getPermission';
import {
  buildClassNameAssignmentCell,
  buildLAST_UPDATEDFormatter,
  buildLastUpdate
} from './preMatchHelper';

jest.mock('../../Shared/permission/getPermission');
const setUp = permission => {
  getPermission.mockReturnValue(permission);
};

const setUpPathname = pathname => {
  global.window = Object.create(window);
  const url = 'http://dummy.com/test/v1';

  Object.defineProperty(window, 'location', {
    value: {
      href: url,
      pathname: pathname
    }
  });
};

describe('preMatchHelper under test', () => {
  afterEach(() => {
    delete process.env.NODE_ENV;
  });

  it('when "highlight and Copy" buildClassNameAssignmentCell should return highlight copied', () => {
    const initialState = {
      isHighlight: true,
      isCopying: true
    };
    const result = buildClassNameAssignmentCell(
      initialState.isHighlight,
      initialState.isCopying
    );
    const expectState = 'highlight copied';
    expect(result).toEqual(expectState);
  });

  it('when "highlight" buildClassNameAssignmentCell should return highlight', () => {
    const initialState = {
      isHighlight: true,
      isCopying: false
    };
    const result = buildClassNameAssignmentCell(
      initialState.isHighlight,
      initialState.isCopying
    );
    const expectState = 'highlight';
    expect(result).toEqual(expectState);
  });

  it('buildLastUpdate should string', () => {
    const props = {
      LAST_UPDATEDFormatter: 'December 7, 2020 10:12 AM',
      updateBy: 'starix14'
    };
    const result = buildLastUpdate(props);
    const expectState =
      ' (Last Updated at December 7, 2020 10:12 AM GMT+8 by starix14)';
    expect(result).toEqual(expectState);
  });

  it('buildLAST_UPDATEDFormatter should return 2 param', () => {
    const props = {
      updateTime: '2020-12-07T10:12:56.343+07:00',
      updateBy: 'starix14'
    };
    const result = buildLAST_UPDATEDFormatter(props);
    const expectState = {
      LAST_UPDATEDFormatter: 'December 7, 2020 10:12 AM',
      updateBy: 'starix14'
    };
    expect(result).toEqual(expectState);
  });

  it('when updateTime =``, buildLAST_UPDATEDFormatter should return LAST_UPDATEDFormatter undefined ', () => {
    const props = {
      updateTime: '',
      updateBy: 'starix14'
    };
    const result = buildLAST_UPDATEDFormatter(props);
    const expectState = {
      LAST_UPDATEDFormatter: undefined,
      updateBy: 'starix14'
    };
    expect(result).toEqual(expectState);
  });
});
