import { createRef, Component } from 'react';
import ReactDOM from 'react-dom';
import { find, isEqual } from 'lodash';
import { Table } from 'fixed-data-table-2';
import {
  renderMatchNoColumn,
  renderKOTimeColumn,
  renderTVColumn,
  renderProviderColumn,
  renderMaxbetSpreadColumn,
  renderRemarkColumn
} from './Column/columnBase';

import { renderNoteColumn } from './Column/noteColumn';
import { renderTicketColumn } from './Column/ticketColumn';
import { renderEventColumn } from './Column/eventColumn';
import { renderLAColumn } from './Column/laColumn';
import { renderBettypeColumn } from './Column/bettypeColumn';
import { AppContext } from '../Shared/Context';
import { MatchStatuses } from '../Shared/Constants';
import PropTypes from 'prop-types';
import memoizeOne from 'memoize-one';
import isDeepEqual from 'lodash.isequal';
import Loading from '../components/Loading';
import { filterByColumn } from './FilterHelper.native';
import { onSavedAssigment, onDeletedAssigment } from './AssignmentHelper';
import { TableInfo, ColumnTV, ColumnProvider } from './constants/Column';
import NotePopupTableCss from '../Shared/NotePopupTableCss';
import { connect } from 'react-redux';
import { withResizeDetector } from 'react-resize-detector';

const identity = x => x;
const deepMemoized = memoizeOne(identity, isDeepEqual);

const propTypes = {
  dataEvent: PropTypes.array,
  dataSource: PropTypes.array.isRequired,
  matchIsScrollTo: PropTypes.number.isRequired,
  isAllowAddEvent: PropTypes.bool.isRequired,
  bettypeColumns: PropTypes.array.isRequired,
  policy: PropTypes.object.isRequired,
  notePolicy: PropTypes.object.isRequired,
  addSpecialEvent: PropTypes.func.isRequired,
  openDetailInformation: PropTypes.func.isRequired,
  columnFilters: PropTypes.array.isRequired,
  columnFiltersChange: PropTypes.func.isRequired,
  scrollLeft: PropTypes.number
};

class EventAssignmentTable extends Component {
  constructor(props) {
    super(props);
    this.tableRef = createRef();
    this.state = {
      data: [],
      ticketMatch: [],
      loading: true,
      scrollLeft: this.props.scrollLeft,
      scrollTop: 0,
      indexMatchNew: -1,
      closePopup: false
    };
    const { columnFilters } = this.props;
    this.filterModel = {
      columnFilters,
      onClickFilter: (name, keyword) => this.handleClickFilter(name, keyword),
      onClearTextFilter: () => this.handleClearTextFilter()
    };
    this.actionOnAssignment = {
      onSavedAssigment: (assigment, columnName, rowIndex) =>
        this.handleSaveAssignment(assigment, columnName, rowIndex),
      onDeletedAssigment: (assigment, columnName, rowIndex) =>
        this.handleDeleteAssignment(assigment, columnName, rowIndex)
    };
    this.rowClassNameGetter = this.rowClassNameGetter.bind(this);
    this.renderBody = this.renderBody.bind(this);
    this.handleClickFilter = this.handleClickFilter.bind(this);
    this.handleClearTextFilter = this.handleClearTextFilter.bind(this);
    this.handleHorizontalScroll = this.handleHorizontalScroll.bind(this);
    this.handleVerticalScroll = this.handleVerticalScroll.bind(this);
    this.handleSaveAssignment = this.handleSaveAssignment.bind(this);
    this.handleDeleteAssignment = this.handleDeleteAssignment.bind(this);
    this.handleShowNote = this.handleShowNote.bind(this);
    this.rollBackClassNameOfAllSelector =
      this.rollBackClassNameOfAllSelector.bind(this);
    this.updateDataSources = this.updateDataSources.bind(this);
  }

  rollBackClassNameOfAllSelector = () => {
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react/no-find-dom-node
    const tableElement = ReactDOM.findDOMNode(this.tableRef.current);
    if (tableElement === null) {
      return;
    }
    const rowElements = tableElement.querySelectorAll(
      '.fixedDataTableRowLayout_rowWrapper'
    );
    for (const element of rowElements) {
      element.className = 'fixedDataTableRowLayout_rowWrapper';
    }
  };

  updateDataSources = data => {
    this.setState({
      data
    });
  };

  handleShowNote = show => {
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react/no-find-dom-node
    const tableElement = ReactDOM.findDOMNode(this.tableRef.current);
    NotePopupTableCss(tableElement, show);
  };

  handleDeleteAssignment = (assigment, bettypeName, rowIndex) => {
    const data = onDeletedAssigment(
      this.state.data,
      assigment,
      bettypeName,
      rowIndex
    );
    this.setState({ data });
  };

  handleSaveAssignment = (assigment, bettypeName, rowIndex) => {
    const data = onSavedAssigment(
      this.state.data,
      assigment,
      bettypeName,
      rowIndex
    );
    this.setState({ data });
  };

  filter = deepMemoized((events, columnFilters) => {
    let filterEvents = events;
    columnFilters.forEach(item => {
      filterEvents = filterByColumn(filterEvents, item);
    });

    return filterEvents;
  });

  handleClickFilter = (name, keyword) => {
    const { columnFilters, columnFiltersChange } = this.props;
    const item = find(columnFilters, column => column.name === name);
    if (item) {
      item.keyword = keyword;
    } else {
      columnFilters.push({ name, keyword });
    }
    columnFiltersChange(columnFilters, this.state.scrollLeft);
  };

  handleClearTextFilter = () => {
    this.props.columnFiltersChange([], this.state.scrollLeft);
  };

  renderBody = () => {
    const { detailInformationPolicy } = this.context;
    const { data, closePopup } = this.state;
    const {
      isAllowAddEvent,
      bettypeColumns,
      policy,
      notePolicy,
      addSpecialEvent,
      openDetailInformation
    } = this.props;
    const columns = [];
    const eventColRest = {
      dataSource: data,
      isAllowAddEvent,
      policy,
      addSpecialEvent,
      openDetailInformation,
      detailInformationPolicy,
      closePopup
    };
    columns.push(renderMatchNoColumn(data, this.filterModel, closePopup));
    columns.push(renderKOTimeColumn(data, this.filterModel, closePopup));
    columns.push(renderEventColumn(this.filterModel, eventColRest));
    columns.push(
      renderTicketColumn(data, this.handleShowNote, this.updateDataSources)
    );
    bettypeColumns.some(b => isEqual(b.value, ColumnTV.value)) &&
      columns.push(renderTVColumn(data, this.filterModel, closePopup));
    bettypeColumns.some(b => isEqual(b.value, ColumnProvider.value)) &&
      columns.push(renderProviderColumn(data, this.filterModel, closePopup));
    columns.push(renderMaxbetSpreadColumn(data, this.filterModel, closePopup));
    columns.push(renderRemarkColumn(data, this.filterModel, closePopup));
    columns.push(
      renderNoteColumn(
        data,
        notePolicy,
        this.filterModel,
        this.handleShowNote,
        closePopup
      )
    );
    columns.push(
      renderBettypeColumn(
        bettypeColumns,
        data,
        policy,
        this.filterModel,
        this.actionOnAssignment,
        closePopup
      )
    );
    columns.push(
      renderLAColumn(
        data,
        policy,
        this.filterModel,
        this.actionOnAssignment,
        closePopup
      )
    );
    return columns;
  };

  rowClassNameGetter = rowIndex => {
    const classNames = [];
    const { data, indexMatchNew } = this.state;
    data[rowIndex] &&
      isEqual(data[rowIndex].MatchStatus, MatchStatuses.Cancel) &&
      classNames.push('match-cancelled');
    isEqual(indexMatchNew, rowIndex) && classNames.push('is-focus');
    // eslint-disable-next-line no-warning-comments
    /* Number 7, 4 in here to caculate space to show note */
    data.length > 7 &&
      rowIndex > data.length - 4 &&
      classNames.push('is-last-row');

    return classNames.join(' ');
  };

  handleVerticalScroll = scrollTop => {
    this.setState(
      {
        scrollTop,
        closePopup: true
      },
      () => this.rollBackClassNameOfAllSelector()
    );
  };

  handleHorizontalScroll = scrollLeft => {
    this.setState(
      {
        scrollLeft,
        closePopup: true
      },
      () => this.rollBackClassNameOfAllSelector()
    );
  };

  render() {
    const {
      loading,
      data,
      scrollLeft,
      closeAllPopup,
      scrollTop,
      indexMatchNew
    } = this.state;
    const { rowHeight, maxHeight, headerHeight } = TableInfo;
    if (loading) {
      return <Loading />;
    }
    const widthTable =
      window.innerWidth > 680 ? window.innerWidth - 110 : window.innerWidth;

    return (
      <div className='table'>
        <Table
          ref={this.tableRef}
          rowHeight={rowHeight}
          rowsCount={data.length}
          width={widthTable}
          height={window.innerHeight - 170}
          maxHeight={maxHeight}
          headerHeight={headerHeight}
          scrollToRow={indexMatchNew}
          scrollLeft={scrollLeft}
          scrollTop={scrollTop}
          closeAllPopup={closeAllPopup}
          onHorizontalScroll={this.handleHorizontalScroll}
          onVerticalScroll={this.handleVerticalScroll}
          rowClassNameGetter={this.rowClassNameGetter}
          rowClass>
          {this.renderBody()}
        </Table>
        <div className='total'>Total records:{data.length}</div>
      </div>
    );
  }

  componentDidMount() {
    const { dataSource, columnFilters } = this.props;
    const data = this.filter(dataSource, columnFilters);
    const indexMatchNew = data.findIndex(d =>
      isEqual(d.OWPMatchId, this.props.matchIsScrollTo)
    );
    this.setState({
      loading: false,
      indexMatchNew,
      data
    });
  }

  componentDidUpdate(prevProps) {
    if (this.props.dataEvent !== prevProps.dataEvent) {
      this.setState({
        data: this.props.dataEvent
      });
    }
  }
}

EventAssignmentTable.contextType = AppContext;
EventAssignmentTable.propTypes = propTypes;
const mapStateToProps = state => ({
  dataEvent: state.tableReducer.dataEvent
});
export default connect(
  mapStateToProps,
  null
)(withResizeDetector(EventAssignmentTable));
