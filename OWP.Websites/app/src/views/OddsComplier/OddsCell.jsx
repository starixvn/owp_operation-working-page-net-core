import { PureComponent } from 'react';
import PropTypes from 'prop-types';
import NumericTextbox from 'react-numeric-textbox';
import { isNil } from 'lodash';
import { Input } from 'reactstrap';

const propTypes = {
  policy: PropTypes.object.isRequired,
  value: PropTypes.number,
  isValidate: PropTypes.bool,
  onChange: PropTypes.func,
  validateValue: PropTypes.func,
  isAutoField: PropTypes.bool,
  isDisabled: PropTypes.bool
};

class OddsCell extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      value: this.props.value,
      isInvalid: false
    };
    this.onBlur = this.onBlur.bind(this);
    this.onChange = this.onChange.bind(this);
  }

  componentDidUpdate(prevProps) {
    if (
      prevProps.isValidate !== this.props.isValidate ||
      prevProps.value !== this.props.value
    ) {
      this.setState({
        isInvalid: this.props.isValidate && isNil(this.props.value)
      });
    }
  }

  onBlur() {
    const { onChange, validateValue } = this.props;
    const newValue = this.state.value;

    if (!validateValue || validateValue(newValue)) {
      this.setState({ isInvalid: false });
      onChange && onChange(newValue);
    } else {
      this.setState({ isInvalid: true });
    }
  }

  onChange(value) {
    this.setState({ value });
  }

  render() {
    const { policy, value, isAutoField, isDisabled } = this.props;
    const classNameCss = `input-field ${value < 0 ? ' input-negative' : ''}${
      this.state.isInvalid ? ' input-invalid' : ''
    }`;
    const isDisable = isDisabled || isAutoField;

    return (
      <div className='cell-input'>
        {isAutoField && this.state.isInvalid && (
          <span className='error'>error</span>
        )}
        {policy.HasWrite && !isDisable ? (
          <NumericTextbox
            className={classNameCss}
            min={-1000}
            max={1000}
            decimals={2}
            autoCorrect
            value={value}
            format=''
            onChange={this.onChange}
            onBlur={this.onBlur}
            onEnter={this.onBlur}
          />
        ) : (
          <Input
            className={classNameCss}
            value={value !== null ? value : ''}
            disabled
          />
        )}
      </div>
    );
  }
}

OddsCell.propTypes = propTypes;
export default OddsCell;
