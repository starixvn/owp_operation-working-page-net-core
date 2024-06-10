import { createRef, PureComponent } from 'react';
import { Button } from 'reactstrap';
import CloseIcon from 'mdi-react/CloseIcon';
import FilterIcon from 'mdi-react/FilterIcon';
import PropTypes from 'prop-types';

const propTypes = {
  onSubmit: PropTypes.func.isRequired,
  onClear: PropTypes.func.isRequired,
  name: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired,
  keyword: PropTypes.string,
  clearFilterTitle: PropTypes.string
};

class FilterHeaderNative extends PureComponent {
  constructor(props) {
    super(props);
    this.currentKeyword = props.keyword;
    this.state = {
      isOpen: false,
      value: ''
    };
    this.ref = createRef();
    this.handleClickOutside = this.handleClickOutside.bind(this);
    this.onHandleClickOpen = this.onHandleClickOpen.bind(this);
  }

  static getDerivedStateFromProps = (nextProps, state) => {
    const { isOpen, value } = state;
    let newValue = value;
    if (this.currentKeyword !== nextProps.keyword) {
      this.currentKeyword = nextProps.keyword;
      newValue = this.currentKeyword;
    }
    return {
      isOpen,
      value: newValue
    };
  };

  handleClickOutside(event) {
    if (
      this.ref.current &&
      !this.ref.current.contains(event.target) &&
      this.state.isOpen
    ) {
      this.setState(prevState => {
        return {
          ...prevState,
          isOpen: false
        };
      });
    }
  }

  onHandleTextChange = event => {
    const value = event.target.value;
    this.setState({ value });
  };

  onHandleClearTextFilter = () => {
    const { name } = this.props;
    this.setState({
      isOpen: false,
      value: ''
    });
    this.props.onClear(name);
  };

  onHandleClickSubmit = () => {
    const { name } = this.props;
    const { value } = this.state;
    this.setState({
      isOpen: false
    });
    this.props.onSubmit(name, value);
  };

  onKeyPress = event => {
    if (event.key === 'Enter') {
      this.onHandleClickSubmit();
    }
  };

  onHandleClickOpen = () => {
    this.setState(prevState => {
      return {
        ...prevState,
        isOpen: !prevState.isOpen
      };
    });
  };

  componentDidMount() {
    document.addEventListener('click', this.handleClickOutside, true);
  }

  componentWillUnmount() {
    document.removeEventListener('click', this.handleClickOutside, true);
  }

  render() {
    const { name, title, keyword, clearFilterTitle } = this.props;
    const { value, isOpen } = this.state;
    return (
      <div className='title-header' ref={this.ref}>
        <span>{title}</span>
        <div className='filter-table'>
          <span
            id={name}
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
                autoFocus
                onChange={this.onHandleTextChange}
                onKeyPress={this.onKeyPress}
              />
              <span
                className='clear-all'
                onClick={this.onHandleClearTextFilter}>
                {clearFilterTitle}
              </span>
              <Button
                className='btn-search'
                disabled={!value}
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

FilterHeaderNative.propTypes = propTypes;
FilterHeaderNative.defaultProps = {
  clearFilterTitle: ' Clear All Filters'
};
export default FilterHeaderNative;
