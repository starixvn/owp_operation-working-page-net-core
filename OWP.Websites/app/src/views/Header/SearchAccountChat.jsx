import React, { useState, useRef, useEffect } from 'react';
import PropTypes from 'prop-types';
import SearchIcon from 'mdi-react/SearchIcon';
import AccountCircleIcon from 'mdi-react/AccountCircleIcon';

const SearchAccountChat = ({ isSearch, onAccountSelect, handleSearch }) => {
  const [searchKeyword, setSearchKeyword] = useState('');
  const [filteredAccounts, setFilteredAccounts] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const searchRef = useRef(null);

  useEffect(() => {
    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const handleClickOutside = (event) => {
    if (
      searchRef.current &&
      !searchRef.current.contains(event.target)
    ) {
      // If click is outside search area
      if (handleSearch) {
        handleSearch(false);
      }
      setSearchKeyword('');
      setFilteredAccounts([]);
      setIsLoading(false);
    }
  };

  const handleSearchInput = (event) => {
    const value = event.target.value;
    setSearchKeyword(value);
    setIsLoading(true);

    // Mock data - replace with actual API call
    const mockAccounts = [
      { id: 1, name: 'Lilac Nguyen' },
      { id: 2, name: 'Starix156' },
      { id: 5, name: 'Starix123456780' },
      {
        id: 4,
        name: 'this_is_a_very_long_account_name_that_might_break_layouts'
      }
    ];

    const filtered = value
      ? mockAccounts.filter(acc =>
          acc.name.toLowerCase().includes(value.toLowerCase())
        )
      : [];

    setFilteredAccounts(filtered);
    setIsLoading(false);
  };

  const handleAccountClick = (account) => {
    if (onAccountSelect) {
      onAccountSelect(account);
    }
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
                key={account.id}
                onClick={() => handleAccountClick(account)}>
                <span className='user-icon'>
                  <AccountCircleIcon size={24} />
                </span>
                <div className='user-info'>
                  <div className='user-name'>{account.name}</div>
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
