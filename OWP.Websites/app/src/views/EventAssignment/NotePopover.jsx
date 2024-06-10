import { createRef, Component } from 'react';
import PropTypes from 'prop-types';
import CloseIcon from 'mdi-react/CloseIcon';
import CommentIcon from 'mdi-react/CommentIcon';
import { EditIcon } from 'mdi-react';
import { pull } from 'lodash';
import NoteTable from '../LiveDetail/MatchNote/NoteTable';

const propTypes = {
  policy: PropTypes.object.isRequired,
  event: PropTypes.object.isRequired,
  onShowNote: PropTypes.func.isRequired
};

class NotePopover extends Component {
  static popovers = [];

  constructor(props) {
    super(props);
    this.state = {
      isOpen: false
    };
    this.iconRef = createRef();
    this.handleToggle = this.handleToggle.bind(this);
    this.handleSave = this.handleSave.bind(this);
  }

  shouldComponentUpdate(nextProps) {
    if (this.props.event.OWPMatchId !== nextProps.event.OWPMatchId) {
      this.setState({ isOpen: false });
      return false;
    }
    return true;
  }

  componentDidUpdate(_prevProps, prevState) {
    if (this.state.isOpen !== prevState.isOpen) {
      if (this.state.isOpen) {
        document.addEventListener('click', this.handleDocumentClick, true);
      } else {
        document.removeEventListener('click', this.handleDocumentClick, true);
      }
    }
  }

  componentDidMount() {
    NotePopover.popovers.push(this);
  }

  handleSave(notes) {
    const { event } = this.props;
    event.Notes = notes;
    this.forceUpdate();
  }

  render() {
    const { policy, event } = this.props;
    const { isOpen } = this.state;
    const hasNote = event.Notes.length > 0;
    return (
      <div className={'note-cell' + (hasNote ? ' has-note' : '')}>
        {hasNote ? (
          <span
            ref={this.iconRef}
            className='icon-comment'
            onClick={this.handleToggle}>
            <CommentIcon size={14} />
          </span>
        ) : (
          <span
            ref={this.iconRef}
            className='icon-edit'
            onClick={this.handleToggle}>
            <EditIcon size={14} />
          </span>
        )}
        <div className={'note-popover' + (isOpen ? ' is-show' : '')}>
          <span className='close-popup' onClick={this.handleToggle}>
            <CloseIcon size={14} />
          </span>
          <NoteTable
            matchId={event.OWPMatchId}
            policy={policy}
            rows={event.Notes}
            onSave={this.handleSave}
          />
        </div>
      </div>
    );
  }

  handleToggle() {
    NotePopover.popovers.forEach(popover => {
      if (popover === this) {
        this.setState(
          {
            isOpen: !this.state.isOpen
          },
          () => this.props.onShowNote(this.state.isOpen)
        );
      } else {
        if (popover.state.isOpen) {
          popover.setState({ isOpen: false });
        }
      }
    });
  }

  componentWillUnmount() {
    document.removeEventListener('click', this.handleDocumentClick, true);
    pull(NotePopover.popovers, this);
  }
}

NotePopover.propTypes = propTypes;
export default NotePopover;
