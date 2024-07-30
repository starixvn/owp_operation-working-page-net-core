import { Component } from 'react';
import PropTypes from 'prop-types';
import { TabContent, TabPane, Nav, NavItem, NavLink } from 'reactstrap';
import classnames from 'classnames';
import NotificationService from './Service';

const propTypes = {
  policy: PropTypes.object.isRequired
};

class List extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isLoading: true,
      activeTab: '1',
      assignees: [],
      watchers: []
    };

    this.toggle = this.toggle.bind(this);
    this.mapToRow = this.mapToRow.bind(this);
    this.onChecked = this.onChecked.bind(this);
  }

  componentDidMount() {
    NotificationService.getByCurrentUser().then(data => {
      this.setState({
        isLoading: false,
        assignees: data.Assignees,
        watchers: data.Watchers
      });
    });
  }

  toggle(tab) {
    if (this.state.activeTab !== tab) {
      this.setState({
        activeTab: tab
      });
    }
  }

  async onChecked(event) {
    const model = {
      NotificationSettingId: event.target.value,
      Value: event.target.checked ? 'True' : 'False'
    };
    await NotificationService.update(model);
  }

  mapToRow(item, index) {
    return (
      <li className='setting-item' key={'NotificationSetting_' + index}>
        <span>{item.Name}</span>
        <p className='checkbox'>
          <input
            data-testid={`notification-${item.NotificationSettingId}-checkbox`}
            disabled={item.NotificationSettingId === 1}
            id={item.NotificationSettingId}
            value={item.NotificationSettingId}
            type='checkbox'
            defaultChecked={item.Value === 'True'}
            onChange={this.onChecked}
          />
          <label htmlFor={item.NotificationSettingId}>
            <span />
          </label>
        </p>
      </li>
    );
  }

  render() {
    const { policy } = this.props;
    const { isLoading, assignees, watchers } = this.state;
    const displayDataAssignees = assignees.map(this.mapToRow);
    const displayDataWatchers = watchers.map(this.mapToRow);
    return (
      <div>
        {isLoading && 'Loading...'}
        <Nav tabs>
          <NavItem>
            <NavLink
              className={classnames({ active: this.state.activeTab === '1' })}
              onClick={() => {
                this.toggle('1');
              }}>
              My Message
            </NavLink>
          </NavItem>
          {policy.HasRead && (
            <NavItem>
              <NavLink
                className={classnames({ active: this.state.activeTab === '2' })}
                onClick={() => {
                  this.toggle('2');
                }}>
                My Section Message
              </NavLink>
            </NavItem>
          )}
        </Nav>
        <TabContent activeTab={this.state.activeTab}>
          <TabPane tabId='1'>
            <div>
              <h3 className='title'>
                <span>
                  Choose the notifications my assigned match would like to
                  receive<p>請選擇接收有關本賽事的訊息</p>
                </span>
                <span className='text-center'>
                  Notify Me<p>訊息</p>
                </span>
              </h3>
              <ul className='setting-list'>{displayDataAssignees}</ul>
            </div>
          </TabPane>
          {policy.HasRead && (
            <TabPane tabId='2'>
              <div>
                <h3 className='title'>
                  <span>
                    Choose the notifications related your Section would like to
                    receive
                    <p>請選擇接收有關你部門的訊息</p>
                  </span>
                  <span className='text-center'>
                    Notify Me
                    <p>訊息</p>
                  </span>
                </h3>
                <ul className='setting-list'>{displayDataWatchers}</ul>
              </div>
            </TabPane>
          )}
        </TabContent>
      </div>
    );
  }
}

List.propTypes = propTypes;
export default List;
