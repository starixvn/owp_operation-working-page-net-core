import { Component } from 'react';
import PropTypes from 'prop-types';
import { Input } from 'reactstrap';
import Service from './Service';

const propTypes = {
  value: PropTypes.string,
  refName: PropTypes.string.isRequired,
  isEditing: PropTypes.bool,
  onChange: PropTypes.func.isRequired,
  onKeyPress: PropTypes.func
};

class EditableCell extends Component {
  onChange(value) {
    const { onChange } = this.props;
    onChange(value);
  }

  onKeyPressHandle(value) {
    const { onKeyPress } = this.props;
    onKeyPress(value);
  }

  render() {
    const { value, isEditing, refName } = this.props;

    return (
      <>
        {isEditing && (
          <td>
            <div className='content'>
              <Input
                type='text'
                className='input-field'
                value={value}
                maxLength={Service.getMaxLength(refName)}
                format=''
                onChange={event => this.onChange(event)}
                onKeyPress={evt => this.onKeyPressHandle(evt)}
              />
            </div>
          </td>
        )}
        {!isEditing && <td className=''>{value}</td>}
      </>
    );
  }
}

EditableCell.propTypes = propTypes;
export default EditableCell;
