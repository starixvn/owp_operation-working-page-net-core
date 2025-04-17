import React, { Component } from 'react';
import PropTypes from 'prop-types';
import SearchIcon from 'mdi-react/SearchIcon';
import AccountCircleIcon from 'mdi-react/AccountCircleIcon';

class SearchAccountChat extends Component {
  constructor(props) {
    super(props);
    this.state = {
      searchKeyword: '',
      filteredAccounts: [],
      isLoading: false
    };

    this.searchRef = React.createRef();
    this.handleSearch = this.handleSearch.bind(this);
    this.handleAccountClick = this.handleAccountClick.bind(this);
    this.handleClickOutside = this.handleClickOutside.bind(this);
  }

  componentDidMount() {
    document.addEventListener('mousedown', this.handleClickOutside);
  }

  componentWillUnmount() {
    document.removeEventListener('mousedown', this.handleClickOutside);
  }

  handleClickOutside(event) {
    if (
      this.searchRef.current &&
      !this.searchRef.current.contains(event.target)
    ) {
      // If click is outside search area
      const { handleSearch } = this.props;
      if (handleSearch) {
        handleSearch(false);
      }
      this.setState({
        searchKeyword: '',
        filteredAccounts: [],
        isLoading: false
      });
    }
  }

  handleSearch(event) {
    const value = event.target.value;
    this.setState({ searchKeyword: value, isLoading: true });

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

    this.setState({
      filteredAccounts: filtered,
      isLoading: false
    });
  }

  handleAccountClick(account) {
    if (this.props.onAccountSelect) {
      this.props.onAccountSelect(account);
    }
    this.setState({
      searchKeyword: '',
      filteredAccounts: []
    });
  }

  render() {
    const { isSearch } = this.props;
    const { searchKeyword, filteredAccounts, isLoading } = this.state;

    if (!isSearch) return null;

    return (
      <div ref={this.searchRef}>
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
            onChange={this.handleSearch}
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
                  onClick={() => this.handleAccountClick(account)}>
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
  }
}

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
