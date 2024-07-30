import { Component } from 'react';
import { MultiSelect } from 'react-multi-select-component';
import PropTypes from 'prop-types';
import Utils from '../Utils';

const propTypes = {
  disableSearch: PropTypes.bool,
  options: PropTypes.array.isRequired,
  localStorageKey: PropTypes.string,
  onSelected: PropTypes.func.isRequired
};

const defaultProps = {
  disableSearch: true
};

class ToggleColumn extends Component {
  constructor(props) {
    super(props);
    this.handleSelectedChanged = this.handleSelectedChanged.bind(this);
    const { onSelected, localStorageKey, options } = this.props;
    const selectedColumns = Utils.convertJsonToObject(
      Utils.getDataFromLocalStore(localStorageKey)
    );
    const columnsValid = Array.isArray(selectedColumns)
      ? selectedColumns
      : options;
    this.state = {
      selected: columnsValid
    };
    onSelected(columnsValid);
  }

  handleSelectedChanged = selected => {
    const { localStorageKey, onSelected, options } = this.props;
    const selectedColumns = options.filter(option =>
      selected.some(item => item.value === option.value)
    );
    localStorage.setItem(localStorageKey, JSON.stringify(selectedColumns));
    this.setState(
      {
        selected: selectedColumns
      },
      () => onSelected(selectedColumns)
    );
  };

  render() {
    const { selected } = this.state;
    const { disableSearch, options } = this.props;
    return (
      <MultiSelect
        options={options}
        onChange={this.handleSelectedChanged}
        value={selected}
        disableSearch={disableSearch}
        ClearSelectedIcon={null}
      />
    );
  }
}

ToggleColumn.propTypes = propTypes;
ToggleColumn.defaultProps = defaultProps;

export default ToggleColumn;
