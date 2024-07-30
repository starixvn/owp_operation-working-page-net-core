import { Component } from 'react';
import { Collapse, CardBody, Card, Button, CardHeader } from 'reactstrap';
import PropTypes from 'prop-types';
import ChevronDownIcon from 'mdi-react/ChevronDownIcon';
import ChevronRightIcon from 'mdi-react/ChevronRightIcon';
import EditableButtonGroup from './EditableButtonGroup';

const propTypes = {
  isOpen: PropTypes.bool,
  children: PropTypes.object.isRequired,
  policy: PropTypes.object.isRequired,
  title: PropTypes.string.isRequired,
  onEdit: PropTypes.func.isRequired,
  onSave: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  isEditing: PropTypes.bool.isRequired
};
const defaultProps = {
  isOpen: true
};

class EditableCollapsePanel extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isOpen: props.isOpen
    };
  }

  toggle = () => {
    this.setState({ isOpen: !this.state.isOpen });
  };

  render() {
    const {
      policy,
      title,
      onEdit,
      isEditing,
      onSave,
      onCancel,
      children
    } = this.props;
    const { isOpen } = this.state;
    return (
      <div className='info-wrap'>
        <CardHeader className={isOpen ? 'card-expand' : 'card-collapse'}>
          <Button className='title' color='default' onClick={this.toggle}>
            {isOpen ? (
              <ChevronDownIcon size={16} />
            ) : (
              <ChevronRightIcon size={16} />
            )}{' '}
            {title}
          </Button>
          {policy.HasWrite && (
            <EditableButtonGroup
              onEdit={onEdit}
              onCancel={onCancel}
              onSave={onSave}
              isEditing={isEditing}
            />
          )}
        </CardHeader>
        <Collapse isOpen={isOpen}>
          <Card>
            <CardBody>{children}</CardBody>
          </Card>
        </Collapse>
      </div>
    );
  }
}

EditableCollapsePanel.propTypes = propTypes;
EditableCollapsePanel.defaultProps = defaultProps;
export default EditableCollapsePanel;
