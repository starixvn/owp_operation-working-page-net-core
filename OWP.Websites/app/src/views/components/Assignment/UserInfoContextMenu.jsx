import { Component } from 'react';
import PropTypes from 'prop-types';
import { ContextMenu, MenuItem, ContextMenuTrigger } from 'react-contextmenu';
import { noop } from 'lodash';
import { HighlightUserContext } from '../../Shared/Context';
import Utils from '../../Shared/Utils';

const propTypes = {
  mainMenuText: PropTypes.string,
  user: PropTypes.object.isRequired,
  policy: PropTypes.object.isRequired,
  onClickEditable: PropTypes.func
};

class UserInfoContextMenu extends Component {
  constructor(props) {
    super(props);
    this.onClickEditable = this.onClickEditable.bind(this);
  }

  onClickEditable() {
    this.props.onClickEditable(true);
  }

  render() {
    const { user, mainMenuText, policy } = this.props;
    let highlighClass = '';
    if (user && user.UserId) {
      highlighClass = policy.UserId === user.UserId ? 'highligh' : '';
    }
    return (
      <HighlightUserContext.Consumer>
        {({ userName }) => {
          let highlightSelectedEmployee = '';
          highlightSelectedEmployee =
            Utils.toLowerCaseForFilter(user.UserName) ===
              Utils.toLowerCaseForFilter(userName)
              ? ' highlighted'
              : '';
          return (
            <span
              data-testid="contextMenu-testId"
              className='contextMenu'
              key={mainMenuText}
              onClick={this.onClickEditable}>
              {mainMenuText && user && user.UserId ? (
                <ContextMenuTrigger id={user.UserName}>
                  <a
                    className={
                      `aClassName${highlighClass}${highlightSelectedEmployee}`
                    }
                    title={user.UserName}>
                    {user.UserName}
                  </a>
                </ContextMenuTrigger>
              ) : (
                <ContextMenuTrigger id={mainMenuText}>
                  <a className='aClassName' title={mainMenuText}>
                    {mainMenuText}
                  </a>
                </ContextMenuTrigger>
              )}
              {mainMenuText && user && user.UserId && (
                <ContextMenu id={user.UserName}>
                  <MenuItem
                    data={{ item: user.Skype }}
                    onClick={this.handleClick}>
                    <i className='mdi mdi-skype' />{' '}
                    <span className='skype-color'> {user.Skype}</span>
                  </MenuItem>
                  <MenuItem
                    data={{ item: user.Extension }}
                    onClick={this.handleClick}>
                    <i className='mdi mdi-deskphone' />
                    {user.Extension}
                  </MenuItem>
                  <MenuItem
                    data={{ item: user.Phone }}
                    onClick={this.handleClick}>
                    <i className='mdi mdi-cellphone' /> {user.Phone}
                  </MenuItem>
                </ContextMenu>
              )}
            </span>
          );
        }}
      </HighlightUserContext.Consumer>
    );
  }
}

UserInfoContextMenu.propTypes = propTypes;
UserInfoContextMenu.defaultProps = {
  mainMenuText: '',
  user: {},
  policy: { HasRead: false, HasWrite: false },
  onClickEditable: noop
};
export default UserInfoContextMenu;
