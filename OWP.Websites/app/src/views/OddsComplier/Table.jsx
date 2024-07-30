import { Fragment, Component } from 'react';
import { Button, UncontrolledPopover, PopoverBody } from 'reactstrap';
import ReactTable from 'react-table';
import PropTypes from 'prop-types';
import {
  findIndex,
  includes,
  filter,
  find,
  isEqual,
  map,
  cloneDeep
} from 'lodash';
import withFixedColumns from 'react-table-hoc-fixed-columns';
import FilterHeader from '../Shared/FilterHeader/FilterHeaderNative';
import OddsRow from './OddsRow';
import Utils from '../Shared/Utils';
import '../components/Assignment/assignment.scss';
import { MatchStatuses, DefaultPageSize, PageSizes } from '../Shared/Constants';
import memoizeOne from 'memoize-one';
import isDeepEqual from 'lodash.isequal';
import { v4 as uuidv4 } from 'uuid';
import { BettypeId } from './constants/Column';
import MarketCell from './MarketCell';
import TimeOfMatch from '../Shared/LiveGameShared/TimeOfMatch';

const identity = x => x;
const deepMemoized = memoizeOne(identity, isDeepEqual);

const { convertThousandToK, convertSpreadToPercent, toLowerCaseForFilter } =
  Utils;

const ReactTableFixedColumns = withFixedColumns(ReactTable);

const propTypes = {
  policy: PropTypes.object.isRequired,
  pageSize: PropTypes.number.isRequired,
  pageIndex: PropTypes.number.isRequired,
  events: PropTypes.array.isRequired,
  columnFilters: PropTypes.array.isRequired,
  columnFiltersChange: PropTypes.func.isRequired,
  onOddsChange: PropTypes.func.isRequired,
  onPageSizeChange: PropTypes.func,
  updateDataSource: PropTypes.func.isRequired
};

class Table extends Component {
  constructor(props) {
    super(props);
    this.state = {
      key: uuidv4(),
      page: this.props.pageIndex,
      pageSize: props.pageSize,
      events: props.events
    };

    this.renderEventColumn = this.renderEventColumn.bind(this);
    this.renderWebLink = this.renderWebLink.bind(this);
    this.renderMaxbetSpreadColumn = this.renderMaxbetSpreadColumn.bind(this);
    this.renderMarketColumn = this.renderMarketColumn.bind(this);
    this.renderMatchNumber = this.renderMatchNumber.bind(this);
    this.renderRemarkColumn = this.renderRemarkColumn.bind(this);
    this.renderProviderColumn = this.renderProviderColumn.bind(this);
    this.renderOddsRow = this.renderOddsRow.bind(this);
    this.onHandleClearTextFilter = this.onHandleClearTextFilter.bind(this);
    this.onHandleClickFilter = this.onHandleClickFilter.bind(this);
    this.renderKickOffTimeColumn = this.renderKickOffTimeColumn.bind(this);
    this.onMarketChange = this.onMarketChange.bind(this);
    this.handlePageChange = this.handlePageChange.bind(this);
    this.updatePageSize = this.updatePageSize.bind(this);
    this.onOddsChange = this.onOddsChange.bind(this);
    this.events = cloneDeep(props.events);
  }

  shouldComponentUpdate(_, nextState) {
    return !isEqual(this.state.key, nextState.key);
  }

  renderMatchNumber(cellInfo) {
    const event = cellInfo.original;
    return (
      <div className='match-no'>
        <span className='match-number'>{event.MatchNo}</span>
        {event.MatchStatus !== MatchStatuses.Live && (
          <span
            title={event.MatchStatus}
            className={`icon-status icon-${event.MatchStatus.toLowerCase()}`}
          />
        )}
      </div>
    );
  }

  renderEventColumn = cellInfo => {
    const event = cellInfo.original;
    return (
      <Fragment key={`Event_${this.state.key}`}>
        <p className='league-name'>{event.League}</p>
        <p className='match-name'>{event.MatchName}</p>
      </Fragment>
    );
  };

  renderWebLink(cellInfo) {
    const event = cellInfo.original;
    const websites = [
      event.Website1,
      event.Website2,
      event.Website3,
      event.Website4
    ];
    return (
      <div className='websites'>
        {map(websites, (website, index) => {
          const key = `website_${index}_${this.state.key}`;
          return website && (
            <Fragment key={key}>
              <a
                id={key}
                className='website'
                href={website}
                target='_blank'
                rel='noopener noreferrer'>
                <span data-tip={website} data-for={key}>
                  [{index + 1}]
                </span>
              </a>
            </Fragment>
          );
        })}
      </div>
    );
  }

  renderMaxbetSpreadColumn(cellInfo) {
    const event = cellInfo.original;
    const handicapSetting = event.MatchSettings.find(m =>
      isEqual(m.Bettype, BettypeId.handicapSetting)
    );
    const overUnderSetting = event.MatchSettings.find(m =>
      isEqual(m.Bettype, BettypeId.overUnderSetting)
    );
    const isDisplayMBCSettings = this.isDisplayMBCSettings(
      handicapSetting,
      overUnderSetting
    );

    const maxBetSpreadMBCCell = this.renderMaxBetSpreadMBC(
      handicapSetting,
      isDisplayMBCSettings,
      overUnderSetting
    );
    return <div className='maxbet-cell'>{maxBetSpreadMBCCell}</div>;
  }

  isDisplayMBCSettings(handicapSetting, overUnderSetting) {
    return (
      (handicapSetting && handicapSetting.SpreadToDisplay) ||
      (overUnderSetting && overUnderSetting.SpreadToDisplay)
    );
  }

  renderMaxBetSpreadMBC(
    handicapSetting,
    isDisplayMBCSettings,
    overUnderSetting
  ) {
    const isHDPSpreadExisting =
      handicapSetting && handicapSetting.SpreadToDisplay;

    return (
      <div>
        <div title='Maxbet/Spread of MBC'>
          {handicapSetting && handicapSetting.MaxBetToDisplay}
          {isDisplayMBCSettings && (
            <span>({handicapSetting && handicapSetting.SpreadToDisplay}</span>
          )}
          {overUnderSetting && overUnderSetting.SpreadToDisplay && (
            <>
              {isHDPSpreadExisting && <span>- </span>}
              {overUnderSetting.SpreadToDisplay}
            </>
          )}
          {isDisplayMBCSettings && <span>)</span>}
        </div>
      </div>
    );
  }

  onMarketChange(event, index, value) {
    const { events } = this.state;
    event.Market = value;
    events[index] = event;
    this.setState({ events }, this.props.updateDataSource(index, event));
  }

  renderMarketColumn(cellInfo) {
    const { policy } = this.props;
    const event = cellInfo.original;
    return (
      <MarketCell
        policy={policy}
        event={event}
        onChange={value => this.onMarketChange(event, cellInfo.index, value)}
      />
    );
  }

  renderRemarkColumn(cellInfo) {
    const event = cellInfo.original;
    const arrayCombine = [];

    if (event.Remark) {
      arrayCombine.push(<p>{event.Remark}</p>);
    }

    if (event.Round) {
      arrayCombine.push(
        <p>
          <b>Round:</b>
          {event.Round}
        </p>
      );
    }

    if (event.Division) {
      arrayCombine.push(
        <p>
          <b>Division:</b>
          {event.Division}
        </p>
      );
    }

    if (arrayCombine.length) {
      return (
        <div className='remark-col'>
          {arrayCombine.map(v => (
            <Fragment key={`Remark_${uuidv4()}`}>{v}</Fragment>
          ))}
        </div>
      );
    } else {
      return null;
    }
  }

  renderProviderColumn(cellInfo) {
    const event = cellInfo.original;

    return (
      <div className='provider-cell'>
        {event.Provider && <p className='runner'>{event.Provider}</p>}
        {event.OtherProvider && (
          <Fragment key={`Provider_${this.state.key}`}>
            <Button
              id={'OP' + cellInfo.index}
              type='button'
              className='button-provider'>
              Other Provider
            </Button>
            <UncontrolledPopover
              className='channel-popover'
              trigger='legacy'
              placement='right'
              target={'OP' + cellInfo.index}>
              <PopoverBody>{event.OtherProvider}</PopoverBody>
            </UncontrolledPopover>
          </Fragment>
        )}
        <p className='wb'>{event.WatchBet}</p>
        {event.Phone && (
          <p className='phone'>
            <i className='icon mdi mdi-phone' />
            {event.Phone}
          </p>
        )}
      </div>
    );
  }

  onOddsChange(_, data) {
    const { events } = this.state;
    const _data = cloneDeep(data);
    const _index = findIndex(events, ['OWPMatchId', _data.Handicap.OWPMatchId]);
    const event = events[_index];
    event.MatchOddsRow = cloneDeep(data);

    this.setState({ events }, this.props.onOddsChange(_index, event));
  }

  renderOddsRow(cellInfo) {
    const event = cellInfo.original;
    const index = cellInfo.index;
    return (
      <OddsRow
        key={`OddsRow_${this.state.key}`}
        policy={this.props.policy}
        data={event.MatchOddsRow}
        matchSettingData={event.MatchSettings}
        event={event}
        onChange={data => this.onOddsChange(index, data)}
      />
    );
  }

  filterByColumn = (events, item) => {
    const keyword = toLowerCaseForFilter(item.keyword);
    if (isEqual(keyword, '')) {
      return events;
    }

    return this.filterByColumnWithNonEmptyKeyword(item, events, keyword);
  };

  filter = deepMemoized((events, columnFilters) => {
    let filterEvents = events;
    columnFilters.forEach(item => {
      filterEvents = this.filterByColumn(filterEvents, item);
    });

    return filterEvents;
  });

  onHandleClearTextFilter = async () => {
    const { columnFiltersChange } = this.props;
    columnFiltersChange([]);
  };

  onHandleClickFilter = async (name, keyword) => {
    const { columnFiltersChange, columnFilters } = this.props;
    const item = find(columnFilters, column => column.name === name);
    if (item) {
      item.keyword = keyword;
    } else {
      columnFilters.push({ name, keyword });
    }
    columnFiltersChange(columnFilters);
  };

  filterByColumnWithNonEmptyKeyword(item, events, keyword) {
    switch (item.name) {
      case 'MatchNo':
        return this.filterByColumnMatchNo(events, keyword);
      case 'KickOffTime':
        return this.filterByColumnKickOffTime(events, keyword);
      case 'Event':
        return this.filterByColumnEvent(events, keyword);
      case 'Remark':
        return this.filterByColumnRemark(events, keyword);
      case 'Provider':
        return this.filterByColumnProvider(events, keyword);
      case 'WebLink':
        return this.filterByColumnWebLink(events, keyword);
      case 'MaxBet_Spread':
        return this.filterByColumnMaxBetSpread(events, keyword);
      case 'Market':
        return this.filterByColumnMarket(events, keyword);
      default:
        return events;
    }
  }

  filterByColumnMatchNo(events, keyword) {
    return filter(events, event => {
      return (
        includes(toLowerCaseForFilter(event.MatchNo), keyword) ||
        includes(toLowerCaseForFilter(event.MatchStatus), keyword)
      );
    });
  }

  filterByColumnKickOffTime(events, keyword) {
    return filter(events, event => {
      if (event.KickOffTimeToDisplay || event.OtherTimeToDisplay) {
        return (
          includes(toLowerCaseForFilter(event.KickOffTimeToDisplay), keyword) ||
          includes(toLowerCaseForFilter(event.OtherTimeToDisplay), keyword)
        );
      }
      return false;
    });
  }

  filterByColumnEvent(events, keyword) {
    return filter(events, event => {
      const isIncludeEventInfo =
        includes(toLowerCaseForFilter(event.League), keyword) ||
        includes(toLowerCaseForFilter(event.Home), keyword) ||
        includes(toLowerCaseForFilter(event.Away), keyword);
      const isIncludeNoOfHDP =
        event.NoOfHDP && includes(toLowerCaseForFilter(event.NoOfHDP), keyword);

      return (
        isIncludeEventInfo ||
        isIncludeNoOfHDP ||
        findIndex(event.MatchOffers, mo =>
          includes(toLowerCaseForFilter(mo.MatchOfferValue), keyword)
        ) >= 0
      );
    });
  }

  filterByColumnRemark(events, keyword) {
    return filter(events, event => {
      if (event.Remark || event.Division || event.Round) {
        return (
          includes(toLowerCaseForFilter(event.Remark), keyword) ||
          includes(toLowerCaseForFilter(event.Division), keyword) ||
          includes(toLowerCaseForFilter(event.Round), keyword)
        );
      }
      return false;
    });
  }

  filterByColumnProvider(events, keyword) {
    return filter(events, event => {
      if (event.Provider || event.OtherProvider || event.WatchBet) {
        return (
          includes(toLowerCaseForFilter(event.Provider), keyword) ||
          includes(toLowerCaseForFilter(event.OtherProvider), keyword) ||
          includes(toLowerCaseForFilter(event.WatchBet), keyword)
        );
      }
      return false;
    });
  }

  filterByColumnWebLink(events, keyword) {
    return filter(events, event => {
      return (
        includes(toLowerCaseForFilter(event.Website1), keyword) ||
        includes(toLowerCaseForFilter(event.Website2), keyword) ||
        includes(toLowerCaseForFilter(event.Website3), keyword) ||
        includes(toLowerCaseForFilter(event.Website4), keyword)
      );
    });
  }

  filterByColumnMaxBetSpread(events, keyword) {
    return filter(events, event => {
      let maxbetHDP = '';
      let spreadHDP = '';
      let spreadOU = '';

      const handicapSetting = event.MatchSettings.find(m =>
        isEqual(m.Bettype, BettypeId.handicapSetting)
      );
      const overUnderSetting = event.MatchSettings.find(m =>
        isEqual(m.Bettype, BettypeId.overUnderSetting)
      );
      if (handicapSetting) {
        maxbetHDP = convertThousandToK(handicapSetting.MaxBet, true);
        spreadHDP = convertSpreadToPercent(handicapSetting.Spread, true);
      }

      if (overUnderSetting) {
        spreadOU = convertSpreadToPercent(overUnderSetting.Spread, true);
      }

      const isMBCValid = maxbetHDP || spreadHDP || spreadOU;

      const isIncludeMBC = this.isMatchedByMaxBetAndSpread(
        maxbetHDP,
        spreadHDP,
        spreadOU,
        keyword
      );

      return isMBCValid && isIncludeMBC;
    });
  }

  isMatchedByMaxBetAndSpread(maxbetHDP, spreadHDP, spreadOU, keyword) {
    return (
      includes(toLowerCaseForFilter(maxbetHDP), keyword) ||
      includes(toLowerCaseForFilter(spreadHDP), keyword) ||
      includes(toLowerCaseForFilter(spreadOU), keyword)
    );
  }

  filterByColumnMarket(events, keyword) {
    return filter(events, event => {
      const { Market } = event;
      return includes(toLowerCaseForFilter(Market), keyword);
    });
  }

  renderKickOffTimeColumn(cellInfo) {
    const event = cellInfo.original;
    const {
      KickOffTimeToDisplay,
      KickOffTime,
      InfoSiteTimeDisplay,
      InfoSiteTime,
      OtherTimeToDisplay,
      OtherTime,
      index
    } = event;

    const propsTime = {
      KickOffTimeDisplay: KickOffTimeToDisplay,
      KickOffTime,
      InfoSiteTimeDisplay,
      InfoSiteTime,
      OtherTimeDisplay: OtherTimeToDisplay,
      OtherTime,
      rowIndex: index
    };

    return (
      <div className='kickoff-cell'>
        <TimeOfMatch {...propsTime} />
      </div>
    );
  }

  getKeyword = name => {
    const { columnFilters } = this.props;
    const item = find(columnFilters, column => column.name === name);
    return item ? item.keyword : '';
  };

  genTableColumns() {
    const { policy } = this.props;
    return [
      {
        fixed: 'left',
        Header: 'Math',
        columns: [
          {
            Header: (
              <FilterHeader
                name='MatchNo'
                title='No#'
                keyword={this.getKeyword('MatchNo')}
                onClear={this.onHandleClearTextFilter}
                onSubmit={this.onHandleClickFilter}
              />
            ),
            accessor: 'No',
            width: 60,
            Cell: this.renderMatchNumber
          },
          {
            Header: (
              <FilterHeader
                name='KickOffTime'
                title='KO Time'
                keyword={this.getKeyword('KickOffTime')}
                onClear={this.onHandleClearTextFilter}
                onSubmit={this.onHandleClickFilter}
              />
            ),
            width: 72,
            Cell: this.renderKickOffTimeColumn
          },
          {
            Header: (
              <FilterHeader
                name='Event'
                title='Event'
                keyword={this.getKeyword('Event')}
                onClear={this.onHandleClearTextFilter}
                onSubmit={this.onHandleClickFilter}
              />
            ),
            accessor: 'OWPMatchId',
            width: 300,
            height: 100,
            sortable: false,
            filterable: false,
            Cell: this.renderEventColumn
          }
        ]
      },
      {
        Header: 'Odds',
        columns: [
          {
            Header: (
              <FilterHeader
                name='Remark'
                title='Remark'
                keyword={this.getKeyword('Remark')}
                onClear={this.onHandleClearTextFilter}
                onSubmit={this.onHandleClickFilter}
              />
            ),
            width: 200,
            Cell: this.renderRemarkColumn
          },

          {
            Header: (
              <FilterHeader
                name='Provider'
                title='Provider'
                keyword={this.getKeyword('Provider')}
                onClear={this.onHandleClearTextFilter}
                onSubmit={this.onHandleClickFilter}
              />
            ),
            width: 180,
            accessor: 'Provider',
            Cell: this.renderProviderColumn
          },
          {
            Header: (
              <FilterHeader
                name='WebLink'
                title='Web Link'
                keyword={this.getKeyword('WebLink')}
                onClear={this.onHandleClearTextFilter}
                onSubmit={this.onHandleClickFilter}
              />
            ),
            accessor: 'Website',
            width: 85,
            Cell: this.renderWebLink
          },
          {
            Header: (
              <FilterHeader
                name='MaxBet_Spread'
                title='MB'
                keyword={this.getKeyword('MaxBet_Spread')}
                onClear={this.onHandleClearTextFilter}
                onSubmit={this.onHandleClickFilter}
              />
            ),
            width: 110,
            Cell: this.renderMaxbetSpreadColumn
          },
          {
            Header: (
              <FilterHeader
                name='Market'
                title='Market'
                keyword={this.getKeyword('Market')}
                onClear={this.onHandleClearTextFilter}
                onSubmit={this.onHandleClickFilter}
              />
            ),

            width: 165,
            Cell: this.renderMarketColumn
          },
          {
            Header: (
              <>
                <div className='title-header scroll-space'>
                  <div className='HDP'>HDP</div>
                  <div className='value'>Home</div>
                  <div className='value'>Away</div>
                  <div className='value'>O/U</div>
                  <div className='value'>Over</div>
                  <div className='value'>Under</div>
                  <div className='value'>1</div>
                  <div className='value'>X</div>
                  <div className='value'>2</div>
                  {policy.HasWrite && <div className='action'>Clear</div>}
                </div>
              </>
            ),
            headerClassName: 'cell-column-header',
            className: 'cell-odds-body',
            width: 636,
            Cell: this.renderOddsRow
          }
        ]
      }
    ];
  }

  render() {
    const { page, pageSize, events, key } = this.state;
    const { columnFilters } = this.props;
    const data = this.filter(events, columnFilters);
    const columns = this.genTableColumns();
    return (
      <div className='table-block'>
        <ReactTableFixedColumns
          key={key}
          columns={columns}
          data={data}
          noDataText='There is no data.'
          sortable={false}
          resizable={false}
          filterable={false}
          pageSizeOptions={PageSizes}
          defaultPageSize={DefaultPageSize}
          pageSize={pageSize}
          minRows={3}
          page={page}
          onPageChange={this.handlePageChange}
          onPageSizeChange={this.updatePageSize}
          className='-striped'
        />
        <span className='total-records'>Total records: {data.length}</span>
      </div>
    );
  }

  updatePageSize = async pageSize => {
    this.setState({ key: uuidv4(), pageSize, page: 0 });
    this.props.onPageSizeChange(pageSize);
  };

  handlePageChange = async pageIndex => {
    this.setState({ page: pageIndex, key: uuidv4() });
  };
}

Table.propTypes = propTypes;
export default Table;
