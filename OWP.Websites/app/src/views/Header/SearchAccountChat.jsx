import React, { useState, useRef, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import SearchIcon from 'mdi-react/SearchIcon';
import AccountCircleIcon from 'mdi-react/AccountCircleIcon';
import UserService from '../Shared/User/UserService';
import { debounce } from 'lodash';

const SearchAccountChat = ({ isSearch, onAccountSelect, handleSearch }) => {
  const [searchKeyword, setSearchKeyword] = useState('');
  const [filteredAccounts, setFilteredAccounts] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const searchRef = useRef(null);
  const inputRef = useRef(null);

  useEffect(() => {
    const handleClickOutside = event => {
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

  useEffect(() => {
    if (isSearch && inputRef.current) {
      inputRef.current.focus();
    }
  }, [isSearch]);

  const debouncedSearch = useCallback(
    debounce(async value => {
      setIsLoading(true);
      try {
        const users = await UserService.getAll();
        const mainAccounts = users.filter(user => !user.IsSecondAccount);
        const filtered = value
          ? mainAccounts.filter(
              user =>
                user.UserName?.toLowerCase()?.includes(value.toLowerCase()) ||
                user.NickName?.toLowerCase()?.includes(value.toLowerCase())
            )
          : [];
        setFilteredAccounts(filtered);
      } catch (error) {
        console.info('Error filtering users:', error);
        setFilteredAccounts([]);
      } finally {
        setIsLoading(false);
      }
    }, 300),
    []
  );

  const handleSearchInput = event => {
    const value = event.target.value.trim();
    setSearchKeyword(value);
    debouncedSearch(value);
  };

  const handleAccountClick = account => {
    onAccountSelect?.(account);
    setSearchKeyword('');
    setFilteredAccounts([]);
  };

  const renderSearchResults = () => {
    if (isLoading) {
      return (
        <div className='search-item loading'>
          <span>Searching...</span>
        </div>
      );
    }
    
    if (filteredAccounts.length > 0) {
      return filteredAccounts.map(account => (
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
      ));
    }
    
    return (
      <div className='search-item no-results'>
        <span>No results found</span>
      </div>
    );
  };

  if (!isSearch) {
    return null;
  }

  return (
    <div className='area-search' ref={searchRef}>
      <div className='search-account-chat-container'>
        <div className='search-icon'>
          <SearchIcon size={26} />
        </div>
        <input
          ref={inputRef}
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
        <>
          <div className='pseudo-classes' />
          <div className='area-account search-dropdown'>
            {renderSearchResults()}
          </div>
        </>
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
