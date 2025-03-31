import {
  CContainer,
  CDropdownHeader,
  CHeader,
  CHeaderNav,
  CNavItem
} from '@coreui/react';
import PropTypes from 'prop-types';
import { Component } from 'react';
import {
  DropdownItem,
  DropdownMenu,
  DropdownToggle,
  NavLink,
  UncontrolledDropdown
} from 'reactstrap';
import AccountIcon from 'mdi-react/AccountIcon';
import LogoutIcon from 'mdi-react/LogoutIcon';
import SettingsIcon from 'mdi-react/SettingsIcon';
import Notification from './Notification';
import Search from './Search';
import { IconChat } from '../Shared/common/icons/common-icons';

const propTypes = {
  title: PropTypes.string.isRequired,
  username: PropTypes.string.isRequired,
  logoutLink: PropTypes.string.isRequired,
  notificationSettingLink: PropTypes.string.isRequired,
  accountSettingLink: PropTypes.string.isRequired,
  quickSearchPolicy: PropTypes.object.isRequired,
  rocketChatPolicy: PropTypes.object.isRequired
};

class Header extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isVisibleSearchArea: false
    };

    this.handleVisibleSearchArea = this.handleVisibleSearchArea.bind(this);
    this.toggleSideBar = this.toggleSideBar.bind(this);
  }

  handleVisibleSearchArea(isVisible) {
    if (isVisible) {
      document.body.classList.add('is-open-search');
      this.toggleSideBar(false);
    } else {
      document.body.classList.remove('is-open-search');
    }

    this.setState({
      ...this.state,
      isVisibleSearchArea: isVisible
    });
  }

  toggleSideBar(open = true) {
    const body = document.body;
    if (body.className === '' && open) {
      document.body.classList.add('sidebar-lg-show');
    } else {
      document.body.classList.remove('sidebar-lg-show');
    }
  }

  render() {
    const {
      title,
      username,
      logoutLink,
      notificationSettingLink,
      accountSettingLink,
      quickSearchPolicy,
      rocketChatPolicy
    } = this.props;
    return (
      <div>
        <CHeader className='cd-main-header animate-search fixed header-wrapper'>
          <CContainer fluid>
            <CHeaderNav className='d-md-flex me-auto'>
              <CNavItem>
                <button
                  type='button'
                  className='navbar-toggler'
                  onClick={() => this.toggleSideBar()}>
                  <span className='navbar-toggler-icon'></span>
                </button>
              </CNavItem>
              <CNavItem>
                <div className='page-title'>
                  <span className='active'>{title}</span>
                </div>
              </CNavItem>
            </CHeaderNav>
            <CHeaderNav>
              {quickSearchPolicy.HasRead && (
                <CNavItem>
                  <NavLink
                    data-testid='search'
                    href='#'
                    className='cd-search-trigger cd-text-replace'
                    onClick={() => this.handleVisibleSearchArea(true)}
                  />
                </CNavItem>
              )}

              {rocketChatPolicy.HasRead && (
                <CNavItem>
                  <NavLink href='RocketChat' target='_blank'>
                    <IconChat className={'mdi-icon icon-chat'} />
                  </NavLink>
                </CNavItem>
              )}

              <CNavItem>
                <NavLink
                  href='Feedback'
                  className='cd-feedback'
                  target='_blank'
                />
              </CNavItem>
              <CNavItem>
                <Notification />
              </CNavItem>
              <CNavItem>
                <CDropdownHeader direction='down' className='user-profile'>
                  <UncontrolledDropdown>
                    <DropdownToggle nav>
                      <span className='user-name'>{username}</span>
                      <span className='icon-account'>
                        <AccountIcon size={16} />
                      </span>
                    </DropdownToggle>
                    <DropdownMenu right>
                      <DropdownItem href={notificationSettingLink}>
                        <span className='icon-setting'>
                          <SettingsIcon size={14} />
                        </span>{' '}
                        Notification Setting
                      </DropdownItem>
                      <DropdownItem href={accountSettingLink}>
                        <span className='icon-logout'>
                          <AccountIcon size={14} />
                        </span>{' '}
                        Account Setting
                      </DropdownItem>
                      <DropdownItem href={logoutLink}>
                        <span className='icon-logout'>
                          <LogoutIcon size={14} />
                        </span>{' '}
                        Logout
                      </DropdownItem>
                    </DropdownMenu>
                  </UncontrolledDropdown>
                </CDropdownHeader>
              </CNavItem>
            </CHeaderNav>
          </CContainer>
        </CHeader>
        {quickSearchPolicy.HasRead && (
          <Search
            isVisible={this.state.isVisibleSearchArea}
            handleVisibleSearchArea={this.handleVisibleSearchArea}
          />
        )}
        <div
          className={
            'cd-cover-layer ' +
            (this.state.isVisibleSearchArea ? 'search-form-visible' : '')
          }
          onClick={() => this.handleVisibleSearchArea(false)}
        />
      </div>
    );
  }
}

Header.propTypes = propTypes;
export default Header;
