import { createRef, Component } from 'react';
import PlusIcon from 'mdi-react/PlusIcon';
import PropTypes from 'prop-types';
import { noop } from 'lodash';
import OddTable from '../../LiveDetail/MatchOdd/OddTable';
import { MajorBetTypes } from '../../Shared/Constants';
import service from '../../LiveDetail/MatchOdd/Service';

const propTypes = {
  policy: PropTypes.object.isRequired
};

const defaultState = {
  rows: [],
  isCreating: false,
  isEditing: false,
  isMultipleEdit: true,
  editingIndex: 0
};

class FormRecommendOdds extends Component {
  constructor(props) {
    super(props);
    this.state = defaultState;
    this.tableRef = createRef();
    this.newRow = this.newRow.bind(this);
    this.onDelete = this.onDelete.bind(this);
    this.onRowChange = this.onRowChange.bind(this);
    this.defaultRows = [];
  }

  reset() {
    this.setState(
      {
        ...defaultState
      },
      () => {
        this.defaultRows = [];
      }
    );
  }

  validateAndGetData() {
    const rows = this.tableRef.current.rows;
    for (let i = 0; i < rows.length; i++) {
      const row = rows[i];
      if (!service.validate(row)) {
        return {
          code: 1,
          message: 'Invalid recommend odds'
        };
      }

      const sameRows = rows.filter(
        (item, itemIndex) => i !== itemIndex && item.BetType === row.BetType
      );
      if (sameRows.length > 0) {
        return {
          code: 1,
          message: 'Duplicated recommend odds'
        };
      }
    }

    const data = rows.map(row => {
      const model = {
        BetType: row.BetType,
        Handicap: row.HDP,
        TeamChoice: row.TeamChoice,
        RecommendOdds: [
          { Choice: 1, Odds: row.HomeOdd },
          { Choice: 2, Odds: row.AwayOdd }
        ]
      };
      if (row.BetType === MajorBetTypes.OneXTwo) {
        model.RecommendOdds.push({ Choice: 3, Odds: row.DrawOdd });
      }

      return model;
    });

    return {
      code: 0,
      data: {
        MatchOdds: data
      }
    };
  }

  newRow() {
    const newRow = {
      BetType: MajorBetTypes.Handicap,
      TeamChoice: 1,
      HDP: null,
      HomeOdd: null,
      AwayOdd: null,
      DrawOdd: null
    };
    const newRows = [newRow].concat(this.defaultRows);
    this.defaultRows = newRows;
    this.setState({
      isCreating: true,
      isEditing: true,
      editingIndex: 0,
      rows: this.defaultRows
    });
  }

  onRowChange(row, index) {
    this.defaultRows[index] = row;
  }

  onDelete(index) {
    const { rows } = this.state;
    const leftRows = rows.slice(0, index);
    const rightRows = rows.slice(index + 1);
    const newRows = leftRows.concat(rightRows);
    this.defaultRows = newRows;
    this.setState({ rows: this.defaultRows });
  }

  render() {
    const { policy } = this.props;
    const { isCreating, isEditing, editingIndex, isMultipleEdit, rows } =
      this.state;

    return (
      <div className='card-item recommend-odds'>
        <div className='header'>
          Recommend Odds
          {policy.HasWrite && rows && rows.length < 3 && (
            <button className='btn-add' onClick={this.newRow}>
              <PlusIcon size={15} />
              <span>Add</span>
            </button>
          )}
        </div>
        <div className='content form-vertical tbl-recommendodds'>
          <OddTable
            rows={rows}
            policy={policy}
            isCreating={isCreating}
            isEditing={isEditing}
            isMultipleEdit={isMultipleEdit}
            editingIndex={editingIndex}
            onEdit={noop}
            onRowChange={this.onRowChange}
            onDelete={this.onDelete}
            onSave={noop}
            onCancel={noop}
            ref={this.tableRef}
          />
        </div>
      </div>
    );
  }
}

FormRecommendOdds.propTypes = propTypes;
export default FormRecommendOdds;
