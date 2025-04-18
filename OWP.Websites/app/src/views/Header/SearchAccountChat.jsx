import React, { useState, useRef, useEffect } from 'react';
import PropTypes from 'prop-types';
import SearchIcon from 'mdi-react/SearchIcon';
import AccountCircleIcon from 'mdi-react/AccountCircleIcon';
import UserService from '../Shared/User/UserService';

const SearchAccountChat = ({ isSearch, onAccountSelect, handleSearch }) => {
  const [searchKeyword, setSearchKeyword] = useState('');
  const [filteredAccounts, setFilteredAccounts] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const searchRef = useRef(null);

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (searchRef.current && !searchRef.current.contains(event.target)) {
        handleSearch?.(false);
        setSearchKeyword('');
        setFilteredAccounts([]);
        setIsLoading(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [handleSearch]);

  const handleSearchInput = async (event) => {
    const value = event.target.value;
    setSearchKeyword(value);
    setIsLoading(true);

    try {
      const users = await UserService.getUserFromCached();
      const filtered = value
        ? users.filter(user => 
            (user.UserName?.toLowerCase()?.includes(value.toLowerCase()) ||
             user.NickName?.toLowerCase()?.includes(value.toLowerCase()))
          )
        : [];

      setFilteredAccounts(filtered);
    } catch (error) {
      console.error('Error fetching users:', error);
      setFilteredAccounts([]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleAccountClick = (account) => {
    onAccountSelect?.(account);
    setSearchKeyword('');
    setFilteredAccounts([]);
  };

  if (!isSearch) return null;

  return (
    <div ref={searchRef}>
      <div className='search-account-chat-container'>
        <div className='search-icon'>
          <SearchIcon size={26} />
        </div>
        <input
          type='text'
          className='form-control'
          placeholder='Search by account name or nickname'
          spellCheck='false'
          value={searchKeyword}
          onChange={handleSearchInput}
          autoComplete='off'
        />
      </div>

      {searchKeyword && (
        <div className='area-account search-dropdown'>
          {isLoading ? (
            <div className='search-item loading'>
              <span>Searching...</span>
            </div>
          ) : filteredAccounts.length > 0 ? (
            filteredAccounts.map(account => (
              <div
                className='search-item'
                key={account.UserId}
                onClick={() => handleAccountClick(account)}>
                <span className='user-icon'>
                  <AccountCircleIcon size={24} />
                </span>
                <div className='user-info'>
                  <div className='user-name'>{account.UserName}</div>
                  {account.NickName && (
                    <div className='user-nickname'>{account.NickName}</div>
                  )}
                </div>
              </div>
            ))
          ) : (
            <div className='search-item no-results'>
              <span>No results found</span>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

SearchAccountChat.propTypes = {
  isSearch: PropTypes.bool.isRequired,
  onAccountSelect: PropTypes.func,
  handleSearch: PropTypes.func
};

SearchAccountChat.defaultProps = {
  onAccountSelect: () => {},
  handleSearch: () => {}
};

export default SearchAccountChat;
