import { Component } from 'react';
import { TabContent, TabPane, Nav, NavItem, NavLink } from 'reactstrap';
import classnames from 'classnames';
import TabPanelContent from './TabPanelContent';
import AuthorizeService from '../Authorize/Service';
import { FunctionKeys } from '../Shared/Constants';
export default class MessageBox extends Component {
  constructor(props) {
    super(props);

    this.toggle = this.toggle.bind(this);
    this.state = {
      activeTab: '1',
      policy: {}
    };
  }

  componentDidMount() {
    (async () => {
      const policy = await AuthorizeService.authorize(
        FunctionKeys.NotificationSetting_MySectionMessage
      );
      this.setState({
        policy
      });
      if (policy && !policy.HasRead) {
        this.setState({
          activeTab: '2'
        });
      }
    })();
  }

  toggle(tab) {
    if (this.state.activeTab !== tab) {
      this.setState({
        activeTab: tab
      });
    }
  }

  render() {
    const { activeTab, policy } = this.state;
    return (
      <div className='message-box animated fadeIn'>
        <Nav tabs>
          {policy && policy.HasRead && (
            <NavItem>
              <NavLink
                className={classnames({ active: this.state.activeTab === '1' })}
                onClick={() => {
                  this.toggle('1');
                }}>
                All Messages
              </NavLink>
            </NavItem>
          )}
          <NavItem>
            <NavLink
              className={classnames({ active: this.state.activeTab === '2' })}
              onClick={() => {
                this.toggle('2');
              }}>
              My Message
            </NavLink>
          </NavItem>
          {policy && policy.HasRead && (
            <NavItem>
              <NavLink
                className={classnames({ active: this.state.activeTab === '3' })}
                onClick={() => {
                  this.toggle('3');
                }}>
                My Section Message
              </NavLink>
            </NavItem>
          )}
        </Nav>
        <TabContent activeTab={this.state.activeTab}>
          {policy && policy.HasRead && (
            <TabPane tabId='1'>
              <TabPanelContent
                tabId='1'
                fetchOnLoad={activeTab}
                isWatcher={null}
              />
            </TabPane>
          )}
          <TabPane tabId='2'>
            <TabPanelContent
              tabId='2'
              fetchOnLoad={activeTab}
              isWatcher={false}
            />
          </TabPane>
          {policy && policy.HasRead && (
            <TabPane tabId='3'>
              <TabPanelContent tabId='3' fetchOnLoad={activeTab} isWatcher />
            </TabPane>
          )}
        </TabContent>
      </div>
    );
  }
}
