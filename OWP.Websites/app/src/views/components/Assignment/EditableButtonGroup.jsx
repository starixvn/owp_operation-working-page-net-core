import { Component } from 'react';
import { connect } from 'react-redux';
import EditableButtonBase from './EditableButtonBase';
import PropTypes from 'prop-types';

const propTypes = {
  filterDate: PropTypes.string
};

class EditableButtonGroup extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    const propsNew = {
      ...this.props,
      filterDate: this.props.filterDate
    };
    return <EditableButtonBase {...propsNew} />;
  }
}

EditableButtonGroup.propTypes = propTypes;

const mapStateToProps = state => ({
  filterDate: state.tableReducer.filterDate
});

export default connect(mapStateToProps)(EditableButtonGroup);
