import {
  CContainer,
  CDropdownHeader,
  CHeader,
  CHeaderNav,
  CNavItem
} from '@coreui/react';
import PropTypes from 'prop-types';
import { useState } from 'react';
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
import KeyChangeIcon from 'mdi-react/KeyChangeIcon';
import Notification from './Notification';
import Search from './Search';
import SearchAccountChat from './SearchAccountChat';
import {
  IconChat,
  IconSearchAccount
} from '../Shared/common/icons/common-icons';
import useRocketChat from '../Shared/customHook/useRocketChat';

const propTypes = {
  title: PropTypes.string.isRequired,
  username: PropTypes.string.isRequired,
  logoutLink: PropTypes.string.isRequired,
  notificationSettingLink: PropTypes.string.isRequired,
  accountSettingLink: PropTypes.string.isRequired,
  quickSearchPolicy: PropTypes.object.isRequired,
  rocketChatPolicy: PropTypes.object.isRequired,
  changePasswordPath: PropTypes.string.isRequired
};

const Header = ({
  title,
  username,
  logoutLink,
  notificationSettingLink,
  changePasswordPath,
  accountSettingLink,
  quickSearchPolicy,
  rocketChatPolicy
}) => {
  const [isVisibleSearchArea, setIsVisibleSearchArea] = useState(false);
  const [isSearchAccountChat, setIsSearchAccountChat] = useState(false);
  const { authToken } = useRocketChat();
  const handleVisibleSearchArea = isVisible => {
    if (isVisible) {
      document.body.classList.add('is-open-search');
      toggleSideBar(false);
    } else {
      document.body.classList.remove('is-open-search');
    }

    setIsVisibleSearchArea(isVisible);
  };

  const handleSearchAccountChat = isSearch => {
    setIsSearchAccountChat(isSearch);
  };

  const handleAccountSelect = account => {
    if (account && account.UserName) {
      const chatUrl = `${import.meta.env.VITE_CHAT_SEVER_URL}/direct/${
        account.UserName
      }${authToken ? `?resumeToken=${authToken}` : ''}`;
      window.open(chatUrl, '_blank');
    }
    setIsSearchAccountChat(false);
  };

  const toggleSideBar = (open = true) => {
    const body = document.body;
    if (body.className === '' && open) {
      document.body.classList.add('sidebar-lg-show');
    } else {
      document.body.classList.remove('sidebar-lg-show');
    }
  };

  return (
    <div>
      <CHeader className='cd-main-header animate-search fixed header-wrapper'>
        <CContainer fluid>
          <CHeaderNav className='d-md-flex me-auto'>
            <CNavItem>
              <button
                type='button'
                className='navbar-toggler'
                onClick={() => toggleSideBar()}>
                <span className='navbar-toggler-icon'></span>
              </button>
            </CNavItem>
            <CNavItem>
              <div className='page-title'>
                <span className='active'>{title}</span>
              </div>
            </CNavItem>
          </CHeaderNav>

          <SearchAccountChat
            isSearch={isSearchAccountChat}
            onAccountSelect={handleAccountSelect}
            handleSearch={handleSearchAccountChat}
          />

          <CHeaderNav>
            {quickSearchPolicy.HasRead && (
              <CNavItem>
                <NavLink
                  data-testid='search'
                  href='#'
                  className='cd-search-trigger cd-text-replace'
                  onClick={() => handleVisibleSearchArea(true)}
                />
              </CNavItem>
            )}

            {rocketChatPolicy.HasRead && (
              <>
                <CNavItem>
                  <NavLink
                    href='#'
                    className='rocket-chat menu-search-account'
                    onClick={() => handleSearchAccountChat(true)}>
                    <IconSearchAccount className='icon-search-account' />
                  </NavLink>
                </CNavItem>
                <CNavItem>
                  <NavLink href='/OWPChat' className='rocket-chat'>
                    <IconChat className='icon-chat' />
                  </NavLink>
                </CNavItem>
              </>
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
                      <span className='account-setting'>
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
                    <DropdownItem href={changePasswordPath}>
                      <span className='change-password'>
                        <KeyChangeIcon size={14} />
                      </span>{' '}
                      Change Password
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
          isVisible={isVisibleSearchArea}
          handleVisibleSearchArea={handleVisibleSearchArea}
        />
      )}
      <div
        className={
          'cd-cover-layer ' + (isVisibleSearchArea ? 'search-form-visible' : '')
        }
        onClick={() => handleVisibleSearchArea(false)}
      />
    </div>
  );
};

Header.propTypes = propTypes;
export default Header;
