import { Component } from 'react';
import PropTypes from 'prop-types';
import { Button } from 'reactstrap';
import CloseIcon from 'mdi-react/CloseIcon';
import FilterIcon from 'mdi-react/FilterIcon';
import { pull } from 'lodash';

const propTypes = {
  onSubmit: PropTypes.func,
  onClear: PropTypes.func,
  keyword: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired,
  columnName: PropTypes.string.isRequired
};

class FilterHeader extends Component {
  static columns = [];
  constructor(props) {
    super(props);
    this.state = {
      isOpen: false,
      curentValue: '',
      value: this.props.keyword
    };

    this.onHandleClickOpen = this.onHandleClickOpen.bind(this);
    this.onHandleTextChange = this.onHandleTextChange.bind(this);
  }

  onHandleTextChange = event => {
    const { value } = event.target;
    this.setState({ value });
  };

  onHandleClearTextFilter = () => {
    this.props.onClear();
  };

  onHandleClickSubmit = () => {
    const { columnName } = this.props;
    const { value } = this.state;
    this.setState(
      {
        isOpen: false,
        curentValue: value
      },
      this.props.onSubmit(columnName, value)
    );
  };

  onKeyPress = event => {
    if (event.key === 'Enter') {
      this.onHandleClickSubmit();
    }
  };

  onHandleClickOpen = () => {
    FilterHeader.columns.forEach(column => {
      const isOpen = column === this ? !this.state.isOpen : false;
      column.setState({
        isOpen
      });
    });
  };

  componentDidMount() {
    FilterHeader.columns.push(this);
  }

  componentWillUnmount() {
    FilterHeader.columns = pull(FilterHeader.columns, this);
  }

  render() {
    const { columnName, keyword, title } = this.props;
    const { isOpen, value } = this.state;
    const newValue = value ? value.trim() : value;
    return (
      <div className='title-header'>
        <span>{title}</span>
        <div className='filter-table'>
          <span
            id={columnName}
            className={`icon-filter ${keyword ? 'is-filter' : ''}`}
            onClick={this.onHandleClickOpen}>
            <FilterIcon size={14} />
          </span>
          {isOpen && (
            <div className='filter-popup filter-popup is-show'>
              <span className='close-popup' onClick={this.onHandleClickOpen}>
                <CloseIcon size={14} />
              </span>
              <input
                className='input-field'
                placeholder='Search'
                value={value}
                onChange={this.onHandleTextChange}
                onKeyPress={this.onKeyPress}
              />
              <span
                className='clear-all'
                onClick={this.onHandleClearTextFilter}>
                Clear All Filters
              </span>
              <Button
                className='btn-search'
                disabled={!newValue}
                onClick={this.onHandleClickSubmit}>
                Search
              </Button>
            </div>
          )}
        </div>
      </div>
    );
  }
}

FilterHeader.propTypes = propTypes;
export default FilterHeader;
