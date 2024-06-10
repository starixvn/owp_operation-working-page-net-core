import { Component } from 'react';
import EditableButtonBase from './EditableButtonBase';

class EditableButtonGroupForLiveDetail extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    const propsNew = {
      ...this.props,
      filterDate: null
    };
    return <EditableButtonBase {...propsNew} />;
  }
}

export default EditableButtonGroupForLiveDetail;
