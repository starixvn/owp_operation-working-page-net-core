import { Component } from 'react';
import Filter from './Filter';
import NormalizeDataHelper from './Helper/NormalizeDataHelper';
import Table from './Table';
import { v4 as uuidv4 } from 'uuid';
import axios from 'axios';
import Spinner from '../components/Spinner';
import DateTimeHelper from './Helper/DateTimeHelper';
import AuthorizeService from '../Authorize/Service';
import { FunctionKeys } from '../Shared/Constants';
import Service from './Service';
import { isEqual, includes } from 'lodash';
import Utils from '../Shared/Utils';
import ReportConst from './constant/ReportConst';
import OrderHelper from './Helper/OrderHelper';
class LstReport extends Component {
  constructor(props) {
    super(props);
    this.state = {
      severTime: null,
      isShowTable: false,
      columnIsShow: [],
      dataSource: [],
      columnFilter: [],
      columnOrder: {}
    };
    this.onExport = this.onExport.bind(this);
    this.handleColumnChange = this.handleColumnChange.bind(this);
    this.onSetFilter = this.onSetFilter.bind(this);
    this.onFilterData = this.onFilterData.bind(this);
    this.resetData = this.resetData.bind(this);
    this.onSetOrder = this.onSetOrder.bind(this);
    this.onOrderData = this.onOrderData.bind(this);
    this.onOrderDesc = this.onOrderDesc.bind(this);
    this.onOrderAsc = this.onOrderAsc.bind(this);
    this.onOrderMatch = this.onOrderMatch.bind(this);
  }

  onOrderMatch = (source, columnOrder) => {
    const { columnId, orderName, subColumnId } = columnOrder;
    let dataOrder = source;
    if (isEqual(orderName, ReportConst.ORDER_ASC)) {
      dataOrder = OrderHelper.orderMatchAsc(source, columnId, subColumnId);
    }
    if (isEqual(orderName, ReportConst.ORDER_DESC)) {
      dataOrder = OrderHelper.orderMatchDesc(source, columnId, subColumnId);
    }
    return dataOrder;
  };

  onExport = model => {
    const fromDateString = DateTimeHelper.getMMDDYYYYStringFromDateTime(
      model.fromDate
    );
    const toDateString = DateTimeHelper.getMMDDYYYYStringFromDateTime(
      model.toDate
    );
    window.location.href = Service.exportUrl(fromDateString, toDateString);
  };

  resetData = () => {
    this.setState({
      columnFilter: []
    });
  };

  onSetOrder = (columnId, subColumnId, orderName) => {
    this.setState({
      columnOrder: {
        columnId,
        subColumnId,
        orderName
      }
    });
  };

  onOrderAsc = (source, columnId, subColumId) => {
    Spinner.show();
    const orderData = OrderHelper.orderSubColumnAsc(
      source,
      columnId,
      subColumId
    );
    Spinner.hide();
    return orderData;
  };

  onOrderDesc = (source, columnId, subColumId) => {
    Spinner.show();
    const orderData = OrderHelper.orderSubColumnDesc(
      source,
      columnId,
      subColumId
    );
    Spinner.hide();
    return orderData;
  };

  onOrderData = (source, columnOrder) => {
    const { columnId, orderName, subColumnId } = columnOrder;
    if (isEqual(orderName, ReportConst.ORDER_ASC)) {
      return this.onOrderAsc(source, columnId, subColumnId);
    }
    if (isEqual(orderName, ReportConst.ORDER_DESC)) {
      return this.onOrderDesc(source, columnId, subColumnId);
    }
    return source;
  };

  onSetFilter = (columnName, keyword) => {
    const { columnFilter } = this.state;
    const item = columnFilter.find(col => isEqual(col.columnName, columnName));
    if (item) {
      item.keyword = keyword;
    } else {
      columnFilter.push({ columnName, keyword });
    }
    this.setState({ columnFilter });
  };

  onFilterData = (dataSource, columnFilter) => {
    const { toLowerCaseForFilter } = Utils;
    if (!columnFilter.length) {
      return dataSource;
    }
    const dataFilter = dataSource.filter(data => {
      for (const element of columnFilter) {
        const keyword = toLowerCaseForFilter(element.keyword);
        const dataTrader = toLowerCaseForFilter(data.Trader);
        const dataSection = toLowerCaseForFilter(data.Section);
        if (
          (isEqual(element.columnName, 'Trader') &&
            !includes(dataTrader, keyword)) ||
          (isEqual(element.columnName, 'Section') &&
            !includes(dataSection, keyword))
        ) {
          return false;
        }
      }
      return true;
    });
    return dataFilter;
  };

  submit = model => {
    (async () => {
      Spinner.show();
      const response = await Service.query(model);
      const { policy } = this.state;
      let dataSource = [];
      if (policy.traderAssignment.HasRead && policy.traderPerformace.HasRead) {
        dataSource = [
          ...[],
          ...NormalizeDataHelper.mappingResponseToModelViewAll(response)
        ];
      } else {
        if (policy.traderAssignment.HasRead) {
          dataSource = [
            ...[],
            ...NormalizeDataHelper.mappingResponseToModelAssignment(response)
          ];
        }
        if (policy.traderPerformace.HasRead) {
          dataSource = [
            ...dataSource,
            ...NormalizeDataHelper.mappingResponseToModelPerformance(response)
          ];
        }
      }
      this.setState(
        {
          isShowTable: true,
          dataSource,
          data: dataSource
        },
        () => Spinner.hide()
      );
    })();
  };

  handleColumnChange = columnSelected => {
    this.setState({
      columnSelected,
      isShowTable: true
    });
  };

  componentDidMount = () => {
    (async () => {
      Spinner.show();
      const [
        traderAssignmentPolicy,
        traderPerformacePolicy,
        serverTime
      ] = await axios.all([
        AuthorizeService.authorize(FunctionKeys.TraderReport_TraderAssignment),
        AuthorizeService.authorize(FunctionKeys.TraderReport_TraderPerformance),
        Service.getServerTime()
      ]);
      const policy = {
        userName: traderAssignmentPolicy.Username,
        traderAssignment: {
          HasRead: traderAssignmentPolicy.HasRead
        },
        traderPerformace: {
          HasRead: traderPerformacePolicy.HasRead
        }
      };
      this.setState(
        {
          serverTime,
          policy
        },
        () => Spinner.hide()
      );
    })();
  };

  render() {
    const {
      isShowTable,
      columnSelected,
      policy,
      serverTime,
      columnFilter,
      dataSource,
      columnOrder
    } = this.state;
    const dataFilter = this.onFilterData(dataSource, columnFilter);
    const dataOrder = isEqual(columnOrder.columnId, ReportConst.match)
      ? this.onOrderMatch(dataFilter, columnOrder)
      : this.onOrderData(dataFilter, columnOrder);
    return (
      <div className='body-content'>
        <div className='filter'>
          {serverTime && (
            <Filter
              serverTime={serverTime}
              onSubmit={this.submit}
              onExport={this.onExport}
              canShowHideColumn={isShowTable}
              onShowColumnChange={this.handleColumnChange}
              policy={policy}
            />
          )}
        </div>
        {isShowTable && columnSelected && (
          <Table
            key={uuidv4()}
            columnSelected={columnSelected}
            columnFilter={columnFilter}
            columnOrder={columnOrder}
            onFilter={this.onSetFilter}
            onOrder={this.onSetOrder}
            onClear={this.resetData}
            data={dataOrder}
          />
        )}
      </div>
    );
  }
}

export default LstReport;
