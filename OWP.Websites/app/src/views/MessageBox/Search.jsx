import { Component } from 'react';
import PropTypes from 'prop-types';
import MagnifyIcon from 'mdi-react/MagnifyIcon';
import CloseCircleIcon from 'mdi-react/CloseCircleIcon';
const propTypes = {
  submit: PropTypes.func
};

class Search extends Component {
  state = {
    searchText: '',
    showCancelButton: false
  };

  onKeyPressSearchText(evt) {
    if (evt.key === 'Enter') {
      const { searchText } = this.state;
      this.props.submit(searchText);
    }
  }

  onChangeSearchText(evt) {
    const searchText = evt.target.value;
    const showCancelButton = !!(searchText && searchText.length >= 1);
    this.setState({ searchText, showCancelButton });
  }

  onCancel(evt) {
    evt.preventDefault();
    const searchText = '';
    this.setState({ searchText, showCancelButton: false });
    this.props.submit(searchText);
  }

  render() {
    const { showCancelButton, searchText } = this.state;
    return (
      <div className='search-group'>
        <MagnifyIcon className='icon-search' size={17} />
        <input
          value={searchText}
          className='search-field'
          placeholder='Search'
          onKeyPress={evt => this.onKeyPressSearchText(evt)}
          onChange={evt => this.onChangeSearchText(evt)}
        />
        {showCancelButton && (
          <CloseCircleIcon
            className='icon-clear'
            size={15}
            onClick={evt => this.onCancel(evt)}
          />
        )}
      </div>
    );
  }
}
Search.propTypes = propTypes;
export default Search;
