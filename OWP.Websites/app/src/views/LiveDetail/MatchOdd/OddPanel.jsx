/* eslint-disable no-warning-comments */
import { isEqual, isNil } from 'lodash';
import ChevronDownIcon from 'mdi-react/ChevronDownIcon';
import ChevronRightIcon from 'mdi-react/ChevronRightIcon';
import PropTypes from 'prop-types';
import { Component } from 'react';
import { Button, CardHeader, Collapse } from 'reactstrap';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import { MajorBetTypes } from '../../Shared/Constants';
import OddTable from './OddTable';
import Service from './Service';

const Toastr = withReactContent(Swal);
const propTypes = {
  matchId: PropTypes.number.isRequired,
  policy: PropTypes.object.isRequired,
  onSave: PropTypes.func.isRequired
};

class OddPanel extends Component {
  constructor(props) {
    super(props);
    this.toggle = this.toggle.bind(this);
    this.state = {
      isCreating: false,
      isEditing: false,
      editingIndex: 0,
      isOpen: true
    };
  }

  componentDidMount() {
    (async () => {
      const { matchId } = this.props;
      let rows = [];
      if (!isNil(matchId)) {
        rows = await Service.query({
          OWPMatchId: matchId
        });
      }
      this.setState({ rows });
    })();
  }

  onEdit(index) {
    this.setState({
      isEditing: true,
      editingIndex: index
    });
  }

  async onDelete(index) {
    const result = await Toastr.fire({
      icon: 'question',
      text: 'Are you sure you want to permanently delete this recommend odds?',
      confirmButtonText: 'OK',
      cancelButtonText: 'Cancel',
      showCancelButton: true,
      showCloseButton: true
    });
    if (result.value) {
      const { matchId, onSave } = this.props;
      const { rows } = this.state;
      const { Id, BetType, HDP } = rows[index];
      const model = {
        Id,
        BetType,
        OWPMatchId: matchId,
        OldHandicap: HDP
      };

      await Service.delete(model);
      const leftRows = rows.slice(0, index);
      const rightRows = rows.slice(index + 1);
      const newRows = leftRows.concat(rightRows);
      this.setState({ rows: newRows });
      onSave();
    }
  }

  onSave = async newRow => {
    const { matchId, onSave } = this.props;
    const { rows, isCreating, editingIndex } = this.state;
    const row = rows[editingIndex];
    if (!isCreating && isEqual(row, newRow)) {
      this.setState({
        isCreating: false,
        isEditing: false
      });
      return;
    }

    const { BetType, HDP, HomeOdd, AwayOdd, DrawOdd, TeamChoice } = newRow;
    const sameRows = rows.filter(
      (item, itemIndex) =>
        editingIndex !== itemIndex &&
        item.BetType === BetType &&
        item.HDP === HDP &&
        item.TeamChoice === TeamChoice
    );
    if (sameRows.length > 0) {
      await Toastr.fire({
        icon: 'error',
        text: 'The recommend odds is already existed. Please add another one!',
        showCloseButton: true
      });
      return;
    }

    const model = {
      Id: row.Id,
      OWPMatchId: matchId,
      BetType,
      Handicap: HDP,
      TeamChoice,
      RecommendOdds: [
        { Choice: 1, Odds: HomeOdd },
        { Choice: 2, Odds: AwayOdd }
      ]
    };
    if (BetType === MajorBetTypes.OneXTwo) {
      model.RecommendOdds.push({ Choice: 3, Odds: DrawOdd });
    }

    rows[editingIndex] = newRow;
    if (isCreating) {
      const response = await Service.create(model);
      newRow.Id = response.Id;
      this.setState({
        isCreating: false,
        isEditing: false,
        rows
      });
      onSave();
    } else {
      await Service.update(model);
      newRow.Id = row.Id;
      this.setState({
        isCreating: false,
        isEditing: false,
        rows
      });
      onSave();
    }
  };

  onCancel() {
    const { isCreating, rows } = this.state;
    let newRows;
    if (isCreating) {
      newRows = rows.slice(1);
    } else {
      newRows = rows.slice();
    }

    this.setState({
      isCreating: false,
      isEditing: false,
      rows: newRows
    });
  }

  newRow() {
    const { rows } = this.state;
    const newRow = {
      BetType: null,
      TeamChoice: 1,
      HDP: null,
      HomeOdd: null,
      AwayOdd: null,
      DrawOdd: null
    };
    const newRows = [newRow].concat(rows);
    this.setState({
      isCreating: true,
      isEditing: true,
      editingIndex: 0,
      rows: newRows
    });
  }

  toggle() {
    this.setState({ isOpen: !this.state.isOpen });
  }

  render() {
    const { policy } = this.props;
    const { isCreating, isEditing, editingIndex, rows, isOpen } = this.state;
    return (
      <div className='widget-wrap odds'>
        <CardHeader className={isOpen ? 'card-expand' : 'card-collapse'}>
          <Button className='title' color='default' onClick={this.toggle}>
            {isOpen ? (
              <ChevronDownIcon size={16} />
            ) : (
              <ChevronRightIcon size={16} />
            )}{' '}
            Recommend Odds
          </Button>
        </CardHeader>
        <Collapse isOpen={isOpen}>
          {isNil(rows) ? (
            <div>Loading ...</div>
          ) : (
            <OddTable
              rows={rows}
              policy={policy}
              isCreating={isCreating}
              isEditing={isEditing}
              editingIndex={editingIndex}
              onEdit={event => this.onEdit(event)}
              onDelete={event => this.onDelete(event)}
              onSave={event => this.onSave(event)}
              onCancel={() => this.onCancel()}
            />
          )}
        </Collapse>
      </div>
    );
  }
}

OddPanel.propTypes = propTypes;
export default OddPanel;
