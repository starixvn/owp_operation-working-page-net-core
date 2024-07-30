import { createRef, Component } from 'react';
import DatePicker from 'react-datepicker';
import Utils from '../Shared/Utils';
import Service from './Service';
import AuthService from '../Authorize/Service';
import CalendarIcon from 'mdi-react/CalendarIcon';
import isEqual from 'lodash.isequal';
import Spinner from '../components/Spinner';
import { buildTimeRangeLast30Day } from '../Shared/DateTime/dateTimeHelper';
import DropdownNative from '../Shared/common/Dropdown/DropdownNative';
import { FunctionKeys } from '../Shared/Constants';
import {
  assignmentList,
  assignmentTimetable,
  assignmentTraderTimetable
} from './AutoAssignmentConst.json';

const Status = {
  Error: -1,
  Success: 0,
  Create: 1,
  Running: 2
};
const Latest30Days = buildTimeRangeLast30Day(new Date());

class AutoAssignment extends Component {
  timerId;
  constructor(props) {
    super(props);
    this.state = {
      selectedDate: new Date(),
      creatorId: null,
      data: []
    };
    this.datePicker = createRef();
  }

  componentDidMount() {
    (async () => {
      await this.getRequests();
      await this.getCreatorId();
    })();
  }

  dateChange(date) {
    this.setState({
      selectedDate: date
    });
  }

  openCalendar() {
    this.datePicker.current.setOpen(true);
  }

  async getRequests() {
    const data = await Service.getRequests();
    this.setState({
      data
    });
  }

  async getCreatorId() {
    const { UserId } = await AuthService.authorize(FunctionKeys.AutoAssignment);
    this.setState({
      creatorId: UserId
    });
  }

  async createRequest(selectedDate, creatorId) {
    Spinner.show();
    const createModel = {
      EventDate: Utils.formatDate(selectedDate),
      CreatorId: creatorId
    };
    await Service.createRequest(createModel);
    await this.getRequests();
    Spinner.hide();
  }

  async checkRequest(request, index) {
    Spinner.show();
    const response = await Service.checkRequest(request.requestId);
    const { data } = this.state;
    data[index].status = response;
    this.setState({ data });
    Spinner.hide();
  }

  handleExportAssignment(index, exportType) {
    const { data } = this.state;
    const { eventDate } = data[index];
    const eventDateFormat = Utils.formatDate(eventDate);

    const exportUrl = {
      assignmentList: `${Service.url}/export-excel?eventDate=${eventDateFormat}`,
      assignmentTimetable: `api/auto-assignment/export-visualization?eventDate=${eventDateFormat}`,
      assignmentTraderTimetable: `api/auto-assignment/export-trader?eventDate=${eventDateFormat}`
    };

    window.location.href = exportUrl[exportType];
  }

  async removeRequest(index) {
    Spinner.show();
    const { data } = this.state;
    const request = data[index];
    await Service.removeRequest(request.requestId);
    const newData = data.filter(
      req => !isEqual(req.requestId, request.requestId)
    );
    Spinner.hide();
    this.setState({ data: newData });
  }

  render() {
    const { selectedDate, creatorId } = this.state;
    return (
      <div className='auto-assignment'>
        <div className='filter  '>
          <div className='calendar'>
            <div className='label'>Date</div>
            <div style={{ float: 'left' }}>
              <DatePicker
                minDate={Latest30Days}
                selected={selectedDate}
                onChange={e => this.dateChange(e)}
                dateFormat='MMM dd, yyyy'
                ref={this.datePicker}
              />
            </div>
            <div>
              <span
                className='icon-calendar'
                onClick={() => this.openCalendar()}>
                <CalendarIcon size={14} />
              </span>
              <button
                className='button-submit'
                onClick={() => this.createRequest(selectedDate, creatorId)}>
                Request
              </button>
            </div>
          </div>
        </div>
        <div className='table'>{this.renderTable()}</div>
      </div>
    );
  }

  formatStatus(status) {
    switch (status) {
      case Status.Running:
        return 'Running';
      case Status.Success:
        return 'Success';
      case Status.Error:
        return 'Error';
      default:
        return 'Create';
    }
  }

  renderTable() {
    const { data } = this.state;
    if (data === null) {
      return <div>Loading ...</div>;
    } else if (data.length === 0) {
      return <div>There is no data</div>;
    } else {
      return (
        <table>
          <thead>
            <tr>
              <th>RequestId</th>
              <th>EventDate</th>
              <th>CreatedDate</th>
              <th>Creator</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {data.map((request, index) => (
              <tr key={`${request.requestId}${request.status}`}>
                <td>{request.requestId}</td>
                <td>{Utils.formatDate(request.eventDate)}</td>
                <td>{Utils.formatDateTimeCreateDate(request.createdDate)}</td>
                <td>{request.username}</td>
                <td>{this.formatStatus(request.status)}</td>
                <td>
                  {(isEqual(request.status, Status.Running) ||
                    isEqual(request.status, Status.Create)) && (
                    <>
                      <button onClick={() => this.checkRequest(request, index)}>
                        Check
                      </button>
                      <button onClick={() => this.removeRequest(index)}>
                        Remove
                      </button>
                    </>
                  )}
                  {isEqual(request.status, Status.Success) &&
                    this.renderCommand(index)}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      );
    }
  }

  renderCommand(index) {
    const items = [
      {
        title: 'Auto Assignment List',
        handler: () => this.handleExportAssignment(index, assignmentList)
      },
      {
        title: 'LA Auto Assignment Timetable',
        handler: () => this.handleExportAssignment(index, assignmentTimetable)
      },
      {
        title: 'Trader Timetable',
        handler: () =>
          this.handleExportAssignment(index, assignmentTraderTimetable)
      }
    ];
    return (
      <>
        <DropdownNative items={items} />
        <button onClick={() => this.removeRequest(index)}>Remove</button>
      </>
    );
  }
}
export default AutoAssignment;
