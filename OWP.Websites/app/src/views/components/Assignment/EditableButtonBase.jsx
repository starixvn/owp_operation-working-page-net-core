import { Component } from 'react';
import EditableTextSuggestion from './EditableTextSuggestion';
import EditableTextMention from './EditableTextMention';
import PlusCircleIcon from 'mdi-react/PlusCircleIcon';
import PropTypes from 'prop-types';
import { pullAt } from 'lodash';
import { v4 as uuidv4 } from 'uuid';
import { BetTypeSuggestion } from '../../EventAssignment/constants/Column';
import QuestionMarkCircleIcon from 'mdi-react/QuestionMarkCircleIcon';

const propTypes = {
  main: PropTypes.object.isRequired,
  policy: PropTypes.object.isRequired,
  others: PropTypes.array,
  matchId: PropTypes.number.isRequired,
  isAllowAdd: PropTypes.bool,
  onSaved: PropTypes.func,
  onDeleted: PropTypes.func,
  betType: PropTypes.number,
  index: PropTypes.string,
  columnName: PropTypes.string,
  rowIndex: PropTypes.number,
  filterDate: PropTypes.string,
  otherTime: PropTypes.string
};

class EditableButtonBase extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isEditableMainTraderName: false,
      isEditableOtherTraderName: false,
      isVisibleButtonPlus: true,
      suggestionMainTrader: this.props.main,
      listEditableTextMentions: [],
      matchId: this.props.matchId,
      main: this.props.main,
      policy: this.props.policy,
      isAllowAdd: this.props.isAllowAdd,
      betType: this.props.betType,
      index: this.props.index
    };
    this.onClickButtonPlus = this.onClickButtonPlus.bind(this);
    this.mapEditableTextMention = this.mapEditableTextMention.bind(this);

    this.onEventChangeMainTrader = this.onEventChangeMainTrader.bind(this);
    this.onDeletedMainTrader = this.onDeletedMainTrader.bind(this);

    this.onEventChangeOtherTrader = this.onEventChangeOtherTrader.bind(this);
    this.onDeletedOtherTrader = this.onDeletedOtherTrader.bind(this);
  }

  componentDidMount() {
    const { others, policy } = this.props;
    let listTextMentions = [];
    listTextMentions = others.map((item, index) => {
      return {
        assignmentItem: item,
        dataIndex: index,
        isEditing: false
      };
    });

    this.setState({
      listEditableTextMentions: listTextMentions,
      isVisibleButtonPlus: policy.HasWrite
    });
  }

  mapEditableTextMention(item, index) {
    const { main, policy } = this.state;
    const key = `EditableTextMention_${index}_${item.assignmentItem.AssignmentId}`;

    const { matchId } = this.props;
    return (
      <EditableTextMention
        key={key}
        dataIndex={index}
        policy={policy}
        matchId={matchId}
        mainTrader={main}
        assignmentItem={item.assignmentItem}
        isEditing={item.isEditing}
        onEventChange={this.onEventChangeOtherTrader}
        onDeleted={this.onDeletedOtherTrader}
      />
    );
  }

  onEventChangeMainTrader(isVisibleButtonPlus, newValue, hasChange) {
    if (hasChange) {
      const { columnName, rowIndex } = this.props;
      this.props.onSaved(newValue, columnName, rowIndex);
      this.setState({
        suggestionMainTrader: newValue
      });
    }
    this.setState({
      isVisibleButtonPlus,
      isEditableMainTraderName: !isVisibleButtonPlus
    });
  }

  onDeletedMainTrader(modelItem) {
    this.setState({
      isVisibleButtonPlus: true,
      isEditableMainTraderName: false,
      suggestionMainTrader: {
        AssignmentId: -1,
        AssignmentNote: null,
        AssignmentType: modelItem.AssignmentType,
        Bettype: modelItem.Bettype,
        OWPMatchId: modelItem.OWPMatchId,
        MinorBettypeId: modelItem.MinorBettypeId,
        UserId: null,
        Username: null
      },
      listEditableTextMentions: []
    });
    const { columnName, rowIndex } = this.props;
    this.props.onDeleted(modelItem, columnName, rowIndex);
  }

  onEventChangeOtherTrader(assignmentItem, dataIndex, hasChange) {
    const { listEditableTextMentions } = this.state;
    if (listEditableTextMentions) {
      const indexs = [];
      for (let i = 0; i < listEditableTextMentions.length; i++) {
        if (!listEditableTextMentions[i].assignmentItem.AssignmentNote) {
          indexs.push(i);
        }
      }

      pullAt(listEditableTextMentions, indexs);
    }

    if (hasChange) {
      const { columnName, rowIndex } = this.props;
      if (
        assignmentItem &&
        assignmentItem.AssignmentId > 0 &&
        assignmentItem.AssignmentNote
      ) {
        listEditableTextMentions[dataIndex] = {
          assignmentItem,
          dataIndex,
          isEditing: false
        };
        this.props.onSaved(assignmentItem, columnName, rowIndex);
      } else {
        listEditableTextMentions.splice(dataIndex, 1);
      }
    }

    this.setState({
      isVisibleButtonPlus: true,
      listEditableTextMentions
    });
  }

  onDeletedOtherTrader(assignmentItem, dataIndex) {
    const { listEditableTextMentions } = this.state;
    const { columnName, rowIndex } = this.props;
    const indexs = [dataIndex];
    for (let i = 0; i < listEditableTextMentions.length; i++) {
      if (!listEditableTextMentions[i].assignmentItem.AssignmentNote) {
        indexs.push(i);
      }
    }

    pullAt(listEditableTextMentions, indexs);

    this.props.onDeleted(assignmentItem, columnName, rowIndex);
    this.setState({
      isVisibleButtonPlus: true,
      listEditableTextMentions
    });
  }

  onClickButtonPlus() {
    const { listEditableTextMentions, suggestionMainTrader, matchId } =
      this.state;
    if (!suggestionMainTrader.UserId) {
      this.setState({
        isEditableMainTraderName: true,
        isVisibleButtonPlus: false
      });
    } else {
      listEditableTextMentions.push({
        isEditing: true,
        dataIndex: listEditableTextMentions.length + 1,
        assignmentItem: {
          OWPMatchId: suggestionMainTrader.OWPMatchId || matchId,
          Bettype: suggestionMainTrader.Bettype,
          MinorBettypeId: suggestionMainTrader.MinorBettypeId
        }
      });
      this.setState({
        listEditableTextMentions,
        isVisibleButtonPlus: false
      });
    }
  }

  handleLinkClick = (event, url) => {
    event.preventDefault();
    window.open(url, '_blank');
  };

  render() {
    const {
      isEditableMainTraderName,
      suggestionMainTrader,
      isVisibleButtonPlus,
      listEditableTextMentions,
      policy,
      isAllowAdd,
      betType
    } = this.state;
    const { matchId, index, filterDate, otherTime } = this.props;
    const hasMentions = listEditableTextMentions.length > 0;
    const displayListEditableMention = listEditableTextMentions.map(
      this.mapEditableTextMention
    );
    const classNameCss = 'assignment ' + index;

    return (
      <div className={classNameCss}>
        <EditableTextSuggestion
          key={uuidv4()}
          betType={betType}
          policy={policy}
          matchId={matchId}
          assignmentItem={suggestionMainTrader}
          isEditableTextSuggest={isEditableMainTraderName}
          onEventChange={this.onEventChangeMainTrader}
          onDeleted={this.onDeletedMainTrader}
        />
        {hasMentions && (
          <div className='list-edit-mentions'>{displayListEditableMention}</div>
        )}
        {isAllowAdd && (
          <div className='button-plus'>
            <button
              className='btn-add'
              onClick={this.onClickButtonPlus}
              disabled={!isVisibleButtonPlus}>
              <PlusCircleIcon size={15} />
            </button>
          </div>
        )}
        {isAllowAdd &&
          filterDate &&
          BetTypeSuggestion.some(x => x === this.props.columnName) && (
            <div className='button-suggestion'>
              <QuestionMarkCircleIcon
                size={14}
                className='btn-ko-time-suggestion'
                onClick={event =>
                  this.handleLinkClick(
                    event,
                    `AssigneeSuggestionList?owpMatchId=${matchId}&betTypeId=${betType}&filterDate=${filterDate}`
                  )
                }
              />
              {otherTime && (
                <QuestionMarkCircleIcon
                  className='btn-other-time-suggestion'
                  size={14}
                  onClick={event =>
                    this.handleLinkClick(
                      event,
                      `AssigneeSuggestionList?owpMatchId=${matchId}&betTypeId=${betType}&filterDate=${filterDate}&otherTime=${otherTime}`
                    )
                  }
                />
              )}
            </div>
          )}
      </div>
    );
  }
}
EditableButtonBase.propTypes = propTypes;
EditableButtonBase.defaultProps = {
  policy: { HasRead: true, HasWrite: false },
  isAllowAdd: true,
  main: {},
  others: [],
  matchId: null,
  data: {},
  rowIndex: null,
  index: 'EditableButtonGroup_Key',
  betType: null,
  onSaved: f => f,
  onDeleted: f => f
};

export default EditableButtonBase;
