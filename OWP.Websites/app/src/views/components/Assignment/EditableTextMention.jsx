import { createRef, Component } from 'react';
import { MentionsInput, Mention } from 'react-mentions';
import PropTypes from 'prop-types';
import defaultMentionStyle from './defaultMentionStyle';
import UserInfoContextMenu from './UserInfoContextMenu';
import UserService from '../../Shared/User/UserService';
import AssignmentService from './Service';
import UserItem from './UserItem';
import { noop } from 'lodash';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import AssignmentContext from './AssignmentContext';
import ToastrWrapper from '../ToastrWrapper';
const Toastr = withReactContent(Swal);
const showWarning = message => {
  Toastr.fire({
    icon: 'warning',
    html: message,
    allowOutsideClick: false,
    allowEnterKey: false
  });
};
const queryUsers = query => UserService.getMentionsUsers(query);

const getTraderNameFromInput = (inputTextParam, userId) => {
  let user = {};
  let isFound = false;
  if (!inputTextParam) {
    return { found: isFound, user, note1: '', note2: '' };
  }

  const inputText = inputTextParam + ' ';
  const indexOfPattern = inputText.indexOf('#');

  if (indexOfPattern <= -1) {
    return {
      found: isFound,
      note1: inputText
    };
  }

  let textBeforePattern = inputText.substring(0, indexOfPattern);
  const textAfterPattern = inputText.substring(
    textBeforePattern.length,
    inputText.length
  );
  const usernameText = textAfterPattern.substring(
    0,
    textAfterPattern.indexOf(' ')
  );
  let textAfterUsername = textAfterPattern.substring(
    usernameText.length,
    textAfterPattern.length
  );
  if (userId) {
    user = UserService.getInfoByUserId(userId);
  } else {
    const getAccountName = usernameText.replace('#', '');
    user = UserService.getInfoByUserName(getAccountName);
  }

  isFound = user && user.UserId && user.UserId > 0;
  if (textBeforePattern !== '' && textBeforePattern.endsWith('')) {
    textBeforePattern = textBeforePattern + ' ';
  }
  if (textAfterUsername !== '' && textAfterUsername.startsWith('')) {
    textAfterUsername = ' ' + textAfterUsername.trim();
  }

  return {
    found: isFound,
    user,
    userName: usernameText,
    note1: textBeforePattern.trim() + ' ',
    note2: ' ' + textAfterUsername
  };
};

const propTypes = {
  OWPMatchId: PropTypes.number.isRequired,
  dataIndex: PropTypes.number.isRequired,
  assignmentItem: PropTypes.object.isRequired,
  policy: PropTypes.object.isRequired,
  isEditing: PropTypes.bool,
  onEventChange: PropTypes.func.isRequired,
  onDeleted: PropTypes.func
};

class EditableTextMention extends Component {
  constructor(props) {
    super(props);

    this.state = {
      plainTextValue: '',
      mentionValue: '',
      isNeedMention: true,
      isEditableTextMention: this.props.isEditing,
      UserId: this.props.assignmentItem.UserId,
      selectedUserName: this.props.assignmentItem.Username,
      hasChange: false
    };

    this.input = createRef();
    this.handleChange = this.handleChange.bind(this);
    this.renderSuggestion = this.renderSuggestion.bind(this);
    this.onAdd = this.onAdd.bind(this);
    this.textValueMentionKeyDown = this.textValueMentionKeyDown.bind(this);
    this.onClickEditable = this.onClickEditable.bind(this);
    this.handleOnBlur = this.handleOnBlur.bind(this);
    this.handleGetUserByUserName = this.handleGetUserByUserName.bind(this);
    this.handleGetAssignmentNote = this.handleGetAssignmentNote.bind(this);
    this.crudAssignment = this.crudAssignment.bind(this);
  }

  async handlerMessageToastr(data, onSuccess, onFailure) {
    if (!data) {
      return;
    }
    const isSuccess = data.Code === 0;
    if (isSuccess) {
      onSuccess();
    } else {
      if (data.Message) {
        ToastrWrapper.showError(data.Message);
      }

      if (onFailure) {
        onFailure();
      }
    }
  }

  handleChange(_, newValue, newPlainTextValue, mentions) {
    let selectedUserName = '';
    const maxLengthInputDefault = 60;
    let maxLengthInput = maxLengthInputDefault;
    let maxLengthName = 0;
    let isNeedMention = true;
    let subbedNewValue = newValue;
    let subbedNewPlainTextValue = newPlainTextValue;
    if (mentions && mentions.length) {
      selectedUserName = mentions[0].id;
      maxLengthName = selectedUserName.length;
      maxLengthInput = maxLengthInput + maxLengthName;
    }

    if (newPlainTextValue.split('#').length > 2) {
      isNeedMention = false;
    }
    if (newValue.length > maxLengthInput) {
      subbedNewValue = newValue.substring(0, maxLengthInput);
      subbedNewPlainTextValue = newPlainTextValue.substring(0, maxLengthInput);
      showWarning(
        `The input maximum allowed ${maxLengthInputDefault} characters.`
      );
    }
    this.setState(() => {
      return {
        plainTextValue: subbedNewPlainTextValue,
        mentionValue: subbedNewValue,
        isNeedMention,
        selectedUserName,
        hasChange: true
      };
    });
  }

  renderSuggestion(entry, _, highlightedDisplay, index) {
    return (
      <UserItem
        user={entry}
        highlightedDisplay={highlightedDisplay}
        index={index}
      />
    );
  }

  onAdd() {
    this.setState({
      isNeedMention: false
    });
  }

  createAssignment(modelItem) {
    const { assignmentItem, dataIndex } = this.props;
    let assignmentId = -1;

    (async () => {
      const response = await AssignmentService.create(modelItem);
      assignmentId = response.AssignmentId;
      this.handlerMessageToastr(
        response,
        () => {
          this.context.onUpdateAssignments();
          this.setState({
            assignmentItem: modelItem,
            UserId: modelItem.UserId
          });
        },
        () => {
          const newAssignment = {
            assignmentItem,
            UserId: assignmentItem.UserId
          };
          this.setState(newAssignment);
        }
      );
      modelItem.AssignmentId = assignmentId;
      this.props.onEventChange(modelItem, dataIndex, true);
    })();
  }

  updateAssignment(modelItem) {
    const { assignmentItem, dataIndex } = this.props;
    AssignmentService.update(modelItem).then(response => {
      this.handlerMessageToastr(
        response,
        () => {
          this.setState({
            assignmentItem: modelItem,
            UserId: modelItem.UserId
          });
          this.context.onUpdateAssignments();
        },
        () => {
          this.setState({
            assignmentItem,
            UserId: assignmentItem.UserId
          });
        }
      );
    });
    this.setState({
      isEditableTextMention: false
    });
    this.props.onEventChange(modelItem, dataIndex, true);
  }

  deleteAssignment(modelItem) {
    const { dataIndex } = this.props;
    AssignmentService.delete({
      AssignmentId: modelItem.AssignmentId,
      OWPMatchId: modelItem.OWPMatchId,
      AssignmentType: modelItem.AssignmentType,
      Bettype: modelItem.Bettype
    }).then(response => {
      this.handlerMessageToastr(
        response,
        () => {
          this.context.onUpdateAssignments();
        },
        () => {
          this.setState({
            value: ''
          });
        }
      );
    });
    this.props.onDeleted(modelItem, dataIndex);
  }

  cancelChangeOtherAssignment() {
    this.setState({
      isEditableTextMention: false
    });
    this.props.onEventChange(null, null, false);
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
        ToastrWrapper.showError(
          this.renderValidationResult(response.FailedResults)
        );
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
            this.cancelChangeOtherAssignment();
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

  handleGetUserByUserName() {
    const { selectedUserName } = this.state;
    if (selectedUserName) {
      const getUser = UserService.getInfoByUserName(selectedUserName);
      if (getUser) {
        return getUser.UserId;
      }
    }
    return null;
  }

  handleGetAssignmentNote(inputUserId) {
    const { plainTextValue } = this.state;
    let isValidUser = true;
    let assignmentNote = plainTextValue ? plainTextValue.trim() : '';
    let userId = inputUserId;
    if (assignmentNote) {
      const detectNote = getTraderNameFromInput(assignmentNote, userId);

      if (detectNote.found && detectNote.user && detectNote.user.UserId) {
        userId = detectNote.user.UserId;
        assignmentNote = `${detectNote.note1}#<user>${detectNote.note2}`;
      } else if (assignmentNote.indexOf('#') > -1) {
        ToastrWrapper.showError('User not found');
        isValidUser = false;
      }
    }
    return {
      isValidUser,
      assignmentNote,
      userId
    };
  }

  crudAssignment(modelItem) {
    if (modelItem.AssignmentId > 0) {
      if (modelItem.AssignmentNote) {
        this.validateAssignment(modelItem, () =>
          this.updateAssignment(modelItem)
        );
      } else {
        modelItem.UserId = null;
        this.validateAssignment(modelItem, () =>
          this.deleteAssignment(modelItem)
        );
      }
    } else if (modelItem.AssignmentNote) {
      this.validateAssignment(modelItem, () =>
        this.createAssignment(modelItem)
      );
    } else {
      this.cancelChangeOtherAssignment();
    }
  }

  createUpdateOrDeleteAssignment() {
    const { selectedUserName, hasChange } = this.state;
    const { assignmentItem, OWPMatchId } = this.props;
    if (!hasChange) {
      this.cancelChangeOtherAssignment();
    } else {
      const id = this.handleGetUserByUserName();
      const { isValidUser, assignmentNote, userId } =
        this.handleGetAssignmentNote(id);
      if (isValidUser) {
        const getMatchId = !assignmentItem.OWPMatchId
          ? OWPMatchId
          : assignmentItem.OWPMatchId;
        const modelItem = {
          AssignmentId: assignmentItem.AssignmentId,
          AssignmentType: 2,
          Bettype: assignmentItem.Bettype,
          MinorBettypeId: assignmentItem.MinorBettypeId,
          OWPMatchId: getMatchId,
          UserId: userId,
          Username: selectedUserName,
          AssignmentNote: assignmentNote.trim()
        };
        this.crudAssignment(modelItem);
      }
    }
  }

  handleOnBlur(_, clickedSuggestion) {
    if (!clickedSuggestion) {
      this.createUpdateOrDeleteAssignment();
    }
  }

  textValueMentionKeyDown(event) {
    event.persist();
    if (event.key === 'Escape') {
      this.setState({
        isEditableTextMention: false,
        mentionValue: '',
        plainTextValue: '',
        isNeedMention: false
      });
      this.props.onEventChange(true, event);
    } else if (event.key === 'Enter') {
      event.target.blur();
    } else {
      this.setState({
        isEditableTextMention: true
      });
    }
  }

  onClickEditable() {
    const { policy, assignmentItem } = this.props;
    let isNeedMention = false;
    if (policy.HasWrite) {
      let assignmentNote = assignmentItem.AssignmentNote;
      if (assignmentItem.UserId) {
        const userInfo = UserService.getInfoByUserId(assignmentItem.UserId);
        if (userInfo) {
          const nickName = userInfo.UserName;
          assignmentNote = assignmentNote.replace('<user>', nickName);
        }
      } else {
        assignmentNote = assignmentNote.replace('#<user>', '');
      }
      if (assignmentNote.indexOf('#') === -1) {
        isNeedMention = true;
      }

      this.setState({
        isEditableTextMention: true,
        mentionValue: `${assignmentNote}`,
        plainTextValue: `${assignmentNote}`,
        isNeedMention
      });
    }
  }

  render() {
    const { isEditableTextMention, mentionValue, UserId, isNeedMention } =
      this.state;
    const { policy, assignmentItem } = this.props;
    let assignmentNote = assignmentItem.AssignmentNote;
    if (!assignmentItem.UserId && assignmentNote) {
      assignmentNote = assignmentNote.replace('#<user>', '');
    }
    const getTraderNameInput = getTraderNameFromInput(assignmentNote, UserId);

    return (
      <div style={{ display: 'inline-block' }}>
        {isEditableTextMention && (
          <div className='editableText' key={assignmentItem.AssignmentId}>
            <MentionsInput
              className='mentions-user'
              value={mentionValue}
              onChange={this.handleChange}
              onBlur={this.handleOnBlur}
              singleLine
              allowSpaceInQuery
              onKeyDown={this.textValueMentionKeyDown}
              placeholder=''
              markup='@[<<__display__>>](<<__id__>>)'
              displayTransform={text => `#${text}`}
              ref={this.input}>
              <Mention
                onAdd={this.onAdd}
                renderSuggestion={this.renderSuggestion}
                trigger='#'
                data={isNeedMention ? queryUsers : []}
                appendSpaceOnAdd
                style={defaultMentionStyle}
              />
            </MentionsInput>
          </div>
        )}
        {!isEditableTextMention && getTraderNameInput && policy && (
          <div>
            {(getTraderNameInput.note1 || getTraderNameInput.note2) && (
              <div className='editableText' onClick={this.onClickEditable}>
                <span className='spanClassName'>
                  {getTraderNameInput.note1}
                  <UserInfoContextMenu
                    policy={policy}
                    user={getTraderNameInput.user}
                    mainMenuText={getTraderNameInput.userName}
                  />
                  {getTraderNameInput.note2}
                </span>
              </div>
            )}
          </div>
        )}
      </div>
    );
  }
}
EditableTextMention.propTypes = propTypes;
EditableTextMention.contextType = AssignmentContext;
EditableTextMention.defaultProps = {
  dataIndex: 0,
  assignmentItem: {},
  policy: { HasRead: false, HasWrite: false },
  isEditing: false,
  onEventChange: noop,
  onDeleted: () => noop,
  OWPMatchId: 0
};

export default EditableTextMention;
