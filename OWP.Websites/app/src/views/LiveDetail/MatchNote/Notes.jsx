import { PureComponent } from 'react';
import PropTypes from 'prop-types';
import { Button, CardHeader, Collapse } from 'reactstrap';
import ChevronDownIcon from 'mdi-react/ChevronDownIcon';
import ChevronRightIcon from 'mdi-react/ChevronRightIcon';
import { isNil } from 'lodash';
import NoteTable from './NoteTable';
import NoteContext from './NoteContext';
import Service from './Service';

const propTypes = {
  matchId: PropTypes.number.isRequired,
  policy: PropTypes.object.isRequired
};

class Notes extends PureComponent {
  constructor(props) {
    super(props);
    this.toggle = this.toggle.bind(this);
    this.onSave = this.onSave.bind(this);
    this.state = {
      isEditing: false,
      isOpen: true,
      rows: []
    };
  }

  componentDidMount() {
    const { matchId } = this.props;
    Service.query({
      OWPMatchId: matchId
    }).then(rows => {
      this.setState({ rows });
    });
  }

  toggle() {
    this.setState({ isOpen: !this.state.isOpen });
  }

  onSave(rows) {
    const { onUpdateNotes } = this.context;
    this.setState({ rows });
    onUpdateNotes();
  }

  render() {
    const { policy, matchId } = this.props;
    const { rows, isOpen } = this.state;
    return (
      <div className='widget-wrap notes'>
        <CardHeader className={isOpen ? 'card-expand' : 'card-collapse'}>
          <Button className='title' color='default' onClick={this.toggle}>
            {isOpen ? (
              <ChevronDownIcon size={16} />
            ) : (
              <ChevronRightIcon size={16} />
            )}{' '}
            Notes
          </Button>
        </CardHeader>
        <Collapse isOpen={isOpen}>
          {isNil(rows) ? (
            <div>Loading ...</div>
          ) : (
            <NoteTable
              policy={policy}
              matchId={matchId}
              rows={rows}
              onSave={this.onSave}
            />
          )}
        </Collapse>
      </div>
    );
  }
}

Notes.propTypes = propTypes;
Notes.contextType = NoteContext;
export default Notes;
