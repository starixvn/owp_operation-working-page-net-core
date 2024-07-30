import { Component } from 'react';
import Filter from './FilterNative';
import Table from './Table';
import Service from './Service';
import AuthorizeService from '../Authorize/Service';
import ResourceManagementService from '../ResourceManagement/Service';
import Loading from '../components/Loading';
import Utils from '../Shared/Utils';
import axios from 'axios';
import { FunctionKeys, DefaultPageSize } from '../Shared/Constants';
import { v4 as uuidv4 } from 'uuid';
import { cloneDeep } from 'lodash';

export default class OddsCompiler extends Component {
  constructor(props) {
    super(props);
    this.dataSource = [];
    this.state = {
      pageSize: DefaultPageSize,
      pageIndex: 0,
      isLoading: true,
      policy: {
        HasWrite: false,
        HasRead: false
      },
      originalPolicy: {
        HasWrite: false,
        HasRead: false
      },
      events: [],
      selectedDate: new Date(),
      columnFilters: [],
      key: uuidv4()
    };
    this.queryModel = {};
    this.onColumnFiltersChange = this.onColumnFiltersChange.bind(this);
    this.submit = this.submit.bind(this);
    this.onOddsChange = this.onOddsChange.bind(this);
    this.onPageSizeChange = this.onPageSizeChange.bind(this);
    this.onUpdateDataSource = this.onUpdateDataSource.bind(this);
    this.onExport = this.onExport.bind(this);
  }

  onUpdateDataSource = (index, event) => {
    this.dataSource[index] = event;
  };

  onPageSizeChange(pageSize) {
    this.setState({ pageSize });
  }

  componentDidMount() {
    (async () => {
      const [originalPolicy, matchStatuses] = await axios.all([
        AuthorizeService.authorize(FunctionKeys.OddsCompiler),
        ResourceManagementService.getByRefName('MatchStatus')
      ]);
      this.setState({
        originalPolicy,
        policy: originalPolicy,
        matchStatuses,
        isLoading: false
      });
    })();
  }

  submit = async e => {
    this.setState(
      {
        League: e.League || '',
        isSuccess: false,
        hasChangeOdds: false,
        selectedDate: e.selectedDate || new Date(),
        selectedMatchStatus: e.selectedMatchStatus,
        isExcludeSpecialProducts: e.isExcludeSpecialProducts,
        pageIndex: 0
      },
      () => this.query()
    );
  };

  query = () => {
    (async () => {
      const {
        selectedDate,
        League,
        selectedMatchStatus,
        isExcludeSpecialProducts
      } = this.state;
      this.setState({
        isLoading: true
      });
      this.queryModel = {
        Date: Utils.formatDate(selectedDate),
        League: League || '',
        MatchStatusIds: selectedMatchStatus || [],
        isExcludeSpecialProducts
      };
      await this.queryWithModel();
    })();
  };

  async queryWithModel() {
    const { originalPolicy, selectedDate } = this.state;
    const isYesterdayOrAfter = Utils.isYesterdayOrAfterForAssignment(
      selectedDate
    );
    const policy = cloneDeep(originalPolicy);
    policy.HasWrite = originalPolicy.HasWrite && isYesterdayOrAfter;
    const events = await Service.query(this.queryModel);
    this.dataSource = cloneDeep(events);
    this.setState({
      events,
      policy,
      isLoading: false,
      key: uuidv4()
    });
  }

  onColumnFiltersChange = columnFilters => {
    const events = cloneDeep(this.dataSource);
    this.setState({ columnFilters, key: uuidv4(), events });
  };

  onOddsChange(index, data) {
    const { events } = this.state;
    events[index] = data;
    this.dataSource[index] = data;
    this.setState({ events });
  }

  onExport = model => {
    window.location.href = Service.exportExcel(model);
  };

  render() {
    const {
      isLoading,
      policy,
      matchStatuses,
      events,
      columnFilters,
      key,
      pageSize,
      pageIndex
    } = this.state;
    return (
      <div className='event-assignment'>
        {isLoading && <Loading />}
        {matchStatuses && (
          <Filter
            matchStatuses={matchStatuses}
            submit={this.submit}
            export={this.onExport}
          />
        )}
        {matchStatuses && (
          <Table
            key={key}
            policy={policy}
            events={events}
            onOddsChange={this.onOddsChange}
            columnFilters={columnFilters}
            columnFiltersChange={this.onColumnFiltersChange}
            updateDataSource={this.onUpdateDataSource}
            pageSize={pageSize}
            pageIndex={pageIndex}
            onPageSizeChange={this.onPageSizeChange}
          />
        )}
      </div>
    );
  }
}
