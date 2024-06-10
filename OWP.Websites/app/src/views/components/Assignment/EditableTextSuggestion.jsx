import { Component } from 'react';
import PropTypes from 'prop-types';
import Autosuggest from 'react-autosuggest';
import UserService from '../../Shared/User/UserService';
import UserInfoContextMenu from './UserInfoContextMenu';
import AssignmentService from './Service';
import UserItem from './UserItem';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import AssignmentContext from './AssignmentContext';
import ToastrWrapper from '../ToastrWrapper';
const Toastr = withReactContent(Swal);
const getSuggestionValue = suggestion => suggestion.UserName;

const renderSuggestion = suggestion => {
  if (UserService.getUseSuggesionFromCache()) {
    return <UserItem user={suggestion} />;
  } else {
    const cssName = suggestion.IsAssigned ? 'red' : '';
    return (
      <div style={{ color: cssName }}>
        {suggestion.UserName}{' '}
        {suggestion.UserName && `(${suggestion.NickName})`}
      </div>
    );
  }
};

const propTypes = {
  policy: PropTypes.object.isRequired,
  assignmentItem: PropTypes.object.isRequired,
  matchId: PropTypes.number.isRequired,
  onEventChange: PropTypes.func,
  onDeleted: PropTypes.func,
  isEditableTextSuggest: PropTypes.bool,
  betType: PropTypes.number
};

class EditableTextSuggestion extends Component {
  constructor(props) {
    super(props);
    this.state = {
      value: '',
      UserId: '',
      matchId: this.props.matchId,
      suggestions: [],
      suggestionSelected: {},
      hasChange: false
    };
    this.isBlurKeyDownInvoked = false;
    this.onClickEditable = this.onClickEditable.bind(this);
    this.onChange = this.onChange.bind(this);
    this.onBlur = this.onBlur.bind(this);
    this.onKeyPress = this.onKeyPress.bind(this);
    this.onSuggestionSelected = this.onSuggestionSelected.bind(this);
    this.updateAssignment = this.updateAssignment.bind(this);
    this.insertAssignment = this.insertAssignment.bind(this);
  }

  componentDidMount() {
    const { assignmentItem } = this.props;
    let userName = '';
    let userId = '';
    if (assignmentItem) {
      userName = assignmentItem.Username;
      userId = assignmentItem.UserId;
    }

    this.setState({
      value: userName,
      UserId: userId
    });
  }

  handlerMessageToastr(data, onSuccess, onFailure) {
    if (!data) {
      return;
    }
    const isSuccess = data.Code === 0;
    if (isSuccess) {
      onSuccess();
    } else {
      data.Message && ToastrWrapper.showError(data.Message);
      if (onFailure) {
        onFailure();
      }
    }
  }

  addNewAssignment(modelItem) {
    AssignmentService.create(modelItem).then(response => {
      this.handlerMessageToastr(response, () => {
        this.context.onUpdateAssignments();
        modelItem.AssignmentId = response.AssignmentId;
        this.setState({
          value: modelItem.Username,
          UserId: modelItem.UserId,
          hasChange: false
        });
        this.props.onEventChange(true, modelItem, true);
      });
    });
  }

  editAssignment(modelItem) {
    AssignmentService.update(modelItem).then(response => {
      this.handlerMessageToastr(response, () => {
        this.context.onUpdateAssignments();
        this.setState({
          value: modelItem.Username,
          UserId: modelItem.UserId,
          hasChange: false
        });
        this.props.onEventChange(true, modelItem, true);
      });
    });
  }

  deleteAssignment(modelItem) {
    AssignmentService.delete({
      AssignmentId: modelItem.AssignmentId,
      OWPMatchId: modelItem.OWPMatchId,
      AssignmentType: modelItem.AssignmentType,
      Bettype: modelItem.Bettype
    }).then(response => {
      this.handlerMessageToastr(response, () => {
        this.context.onUpdateAssignments();
        this.props.onDeleted(modelItem);
        modelItem.AssignmentId = -1;
        this.setState({
          value: '',
          suggestionSelected: {}
        });
      });
    });
  }

  cancelChangeAssignment() {
    this.props.onEventChange(true, null, false);
    this.setState({
      hasChange: false,
      value: this.props.assignmentItem.Username
    });
  }

  validateAssignment(modelItem, onYes) {
    AssignmentService.validate({
      OWPMatchId: modelItem.OWPMatchId,
      Bettype: modelItem.Bettype,
      AssignmentType: modelItem.AssignmentType,
      Username: modelItem.UserName,
      UserId: modelItem.UserId
    }).then(response => {
      if (response.IsFailed) {
        Toastr.fire({
          icon: 'error',
          html: this.renderValidationResult(response.FailedResults)
        });

        return;
      }

      if (response.IsHasWarning) {
        Toastr.fire({
          icon: 'warning',
          allowOutsideClick: false,
          showCancelButton: true,
          reverseButtons: true,
          html: this.renderValidationResult(response.WarningResults)
        }).then(result => {
          if (result.value) {
            onYes();
          } else if (result.dismiss === Swal.DismissReason.cancel) {
            this.cancelChangeAssignment();
          }
        });
      } else {
        onYes();
      }
    });
  }

  renderValidationResult(results) {
    return (
      <div>
        <strong className='title'>
          Are you sure you want to change assignment?
        </strong>
        <ul className='list-warning'>
          {results.map((x, i) => (
            <li key={i}>{x.Message}</li>
          ))}
        </ul>
      </div>
    );
  }

  updateAssignment = (modelItem, oldAssignmentItem) => {
    const findUser = UserService.getInfoByUserName(modelItem.Username);
    if (findUser && findUser.UserId) {
      modelItem.UserId = findUser.UserId;
      modelItem.Username = findUser.UserName;
      if (modelItem.UserId !== oldAssignmentItem.UserId) {
        this.validateAssignment(modelItem, () =>
          this.editAssignment(modelItem)
        );
      } else {
        this.cancelChangeAssignment();
      }
    } else {
      this.handlerMessageToastr(
        { Code: 1, Message: 'User not found' },
        f => f,
        () => {
          this.setState({
            value: oldAssignmentItem.Username || modelItem.Username
          });
        }
      );
    }
  };

  insertAssignment = modelItem => {
    const findUser = UserService.getInfoByUserName(modelItem.Username);
    if (findUser && findUser.UserId) {
      modelItem.UserId = findUser.UserId;
      modelItem.Username = findUser.UserName;
      if (this.props.betType && !modelItem.Bettype) {
        modelItem.Bettype = this.props.betType;
      }

      this.validateAssignment(modelItem, () =>
        this.addNewAssignment(modelItem)
      );
    } else {
      this.handlerMessageToastr(
        { Code: 1, Message: 'User not found' },
        f => f,
        () => {
          this.setState({
            value: ''
          });
        }
      );
    }
  };

  insertUpdateDeleteAssignment(modelItem) {
    if (modelItem.AssignmentId && modelItem.AssignmentId > 0) {
      const oldAssignmentItem = this.props.assignmentItem;
      if (modelItem.Username) {
        this.updateAssignment(modelItem, oldAssignmentItem);
      } else {
        modelItem.UserId = null;
        this.validateAssignment(modelItem, () =>
          this.deleteAssignment(modelItem)
        );
      }
    } else {
      if (modelItem.Username) {
        this.insertAssignment(modelItem);
      } else {
        this.cancelChangeAssignment();
      }
    }
  }

  onChange = (_, { newValue }) => {
    this.setState({
      value: newValue,
      hasChange: true
    });
  };

  onSave() {
    const { suggestionSelected, value, hasChange } = this.state;
    const { matchId } = this.props;

    if (hasChange) {
      const oldAssignmentItem = this.props.assignmentItem;
      if (oldAssignmentItem.Username === value) {
        this.cancelChangeAssignment();
        return;
      }

      let getMatchId = oldAssignmentItem.OWPMatchId;
      if (!getMatchId) {
        getMatchId = matchId;
      }

      const modelItem = {
        AssignmentId: oldAssignmentItem.AssignmentId,
        AssignmentType: 1,
        Bettype: oldAssignmentItem.Bettype,
        MinorBettypeId: oldAssignmentItem.MinorBettypeId,
        OWPMatchId: getMatchId,
        UserId: suggestionSelected.UserId,
        Username: value.trim()
      };

      this.insertUpdateDeleteAssignment(modelItem);
    } else {
      this.cancelChangeAssignment();
    }
  }

  onKeyPress = event => {
    if (event.key === 'Enter') {
      this.isBlurKeyDownInvoked = true;
      this.onSave();
    }
  };

  onBlur = () => {
    if (this.isBlurKeyDownInvoked) {
      this.isBlurKeyDownInvoked = false;
      return;
    }
    this.onSave();
  };

  onSuggestionsFetchRequested = ({ value }) => {
    const minLengthToStartSuggest = 2;
    const { matchId } = this.props;
    if (value && value.trim() && value.length >= minLengthToStartSuggest) {
      if (UserService.getUseSuggesionFromCache()) {
        const response = UserService.getSuggestion(value);
        if (response) {
          this.setState({
            suggestions: response
          });
        }
      } else {
        UserService.getSuggestionUsers({
          Keyword: value,
          OWPMatchId: matchId
        }).then(response => {
          if (response) {
            this.setState({
              suggestions: response
            });
          }
        });
      }
    }
  };

  onSuggestionsClearRequested = () => {
    this.setState({
      suggestions: []
    });
  };

  onSuggestionSelected(_, { suggestion }) {
    this.setState({
      suggestionSelected: suggestion
    });
  }

  onClickEditable() {
    const { policy } = this.props;
    if (policy.HasWrite) {
      this.props.onEventChange(false, null);
      this.setState({ hasChange: true });
    }
  }

  render() {
    const { value, suggestions, UserId } = this.state;
    const { isEditableTextSuggest } = this.props;
    const { policy } = this.props;
    const inputProps = {
      placeholder: 'Type a username',
      value: !value ? '' : value,
      onChange: this.onChange,
      onBlur: this.onBlur,
      onKeyPress: this.onKeyPress,
      autoFocus: true
    };
    const user = UserService.getInfoByUserId(UserId);
    if (user) {
      user.NickName = value;
    }
    return (
      <div className='auto-suggest'>
        {isEditableTextSuggest && (
          <Autosuggest
            highlightFirstSuggestion
            suggestions={suggestions}
            onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
            onSuggestionsClearRequested={this.onSuggestionsClearRequested}
            onSuggestionSelected={this.onSuggestionSelected}
            getSuggestionValue={getSuggestionValue}
            renderSuggestion={renderSuggestion}
            inputProps={inputProps}
            focusInputOnSuggestionClick
          />
        )}
        {!isEditableTextSuggest && user && value && (
          <UserInfoContextMenu
            policy={policy}
            user={user}
            mainMenuText={value}
            onClickEditable={this.onClickEditable}
          />
        )}
      </div>
    );
  }
}

EditableTextSuggestion.propTypes = propTypes;
EditableTextSuggestion.contextType = AssignmentContext;
EditableTextSuggestion.defaultProps = {
  policy: { HasRead: false, HasWrite: false },
  matchId: 0,
  onEventChange: f => f,
  onDeleted: f => f,
  isEditableTextSuggest: false
};
export default EditableTextSuggestion;
