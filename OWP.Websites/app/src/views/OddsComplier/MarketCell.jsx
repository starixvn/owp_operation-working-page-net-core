import { createRef, Component } from 'react';
import PropTypes from 'prop-types';
import Service from './Service';

const propTypes = {
  policy: PropTypes.object.isRequired,
  event: PropTypes.object.isRequired,
  onChange: PropTypes.func.isRequired
};

class MarketCell extends Component {
  constructor(props) {
    super(props);
    this.state = {
      value: props.event.Market || ''
    };
    this.textRef = createRef();
  }

  componentDidMount() {
    const { policy } = this.props;
    if (policy.HasWrite && this.textRef.current != null) {
      this.textRef.current.addEventListener('blur', this);
    }
  }

  async handleEvent(e) {
    const { event } = this.props;
    const value = e.target.textContent;
    const currentValue = event.Market || '';
    const newValue = value || '';
    if (newValue === currentValue) {
      return;
    }

    const postModel = {
      OWPMatchId: event.OWPMatchId,
      Market: newValue
    };
    await Service.updateMarket(postModel);
    this.setState({ value }, this.props.onChange(value));
  }

  shouldComponentUpdate(_, nextState) {
    return this.state.value !== nextState.value;
  }

  render() {
    const { policy } = this.props;
    const { value } = this.state;
    return policy.HasWrite ? (
      <div className='cell-market'>
        <div
          className='text-market'
          contentEditable
          ref={this.textRef}
          suppressContentEditableWarning
          onBlur={null}>
          {value}
        </div>
      </div>
    ) : (
      <span>{value}</span>
    );
  }
}

MarketCell.propTypes = propTypes;
export default MarketCell;
