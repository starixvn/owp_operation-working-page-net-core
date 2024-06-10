import UserInfoContextMenu from './UserInfoContextMenu';
import { HighlightUserContext } from '../../Shared/Context';
import { render, fireEvent, screen } from '@testing-library/react';
import '@testing-library/jest-dom';

describe('UserInfoContextMenu Component Tests', () => {
  it('should render <UserInfoContextMenu />', () => {
    const props = {
      mainMenuText: 'sa',
      user: {
        UserId: 8,
        UserName: 'sa',
        NickName: 'superman',
        Skype: 'skype',
        Phone: '123456789',
        Extension: '123'
      },
      policy: {
        UserId: 8,
        HasRead: false,
        HasWrite: false
      }
    };

    const utils = render(<UserInfoContextMenu {...props} />);

    expect(utils.container.getElementsByClassName('contextMenu')).toHaveLength(1);

  });

  it('UserInfoContextMenu with onClickEditable', () => {
    const props = {
      mainMenuText: 'sa',
      user: {
        UserId: 8,
        UserName: 'sa',
        NickName: 'superman',
        Skype: 'skype',
        Phone: '123456789',
        Extension: '123'
      },
      onClickEditable: jest.fn()
    };
    const utils = render(<UserInfoContextMenu {...props} />);
    const element = utils.getByTestId('contextMenu-testId');

    // Act
    fireEvent.click(element);

    // Assert
    expect(props.onClickEditable).toBeCalled();
  });

  it('UserInfoContextMenu should highlight searched employee', () => {
    const props = {
      mainMenuText: 'sa',
      user: {
        UserId: 8,
        UserName: 'sa',
        NickName: 'superman',
        Skype: 'skype',
        Phone: '123456789',
        Extension: '123'
      }
    };
    const utils = render(
      <HighlightUserContext.Provider value={{ userName: 'sa' }}>
        <UserInfoContextMenu {...props} />
      </HighlightUserContext.Provider>
    );

    expect(utils.container.getElementsByClassName('highlighted')).toHaveLength(1);
  });
});
