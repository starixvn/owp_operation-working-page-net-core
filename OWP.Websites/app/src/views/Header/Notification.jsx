import { Component } from 'react';
import {
  Badge,
  UncontrolledDropdown,
  DropdownToggle,
  DropdownMenu,
  TabContent,
  TabPane,
  Nav,
  NavItem,
  NavLink
} from 'reactstrap';
import { toast } from 'react-toastify';
import { CDropdownHeader } from '@coreui/react';
import BellIcon from 'mdi-react/BellIcon';
import classnames from 'classnames';
import NotificationItem from './NotificationItem';
import AuthorizeService from '../Authorize/Service';
import Utils from '../Shared/Utils';
import MessageService, {
  MessageTypes,
  OpenPreMatchIssueURL,
  openTicketCenterIssueURL,
  OSMessageTypes,
  PreMatchMessageTypes,
  TicketCenterMessageTypes,
  LiveDetailTypes
} from '../Message/Service';
import { FunctionKeys } from '../Shared/Constants';
import { post } from '../Shared/Service/Service';
import { configSignalR } from '../Shared/NetCoreSignalR/NetCoreSignalRService';
import { isEqual } from 'lodash';
import { isEquals } from 'immutability-helper';
import { openNewChattingTab } from '../Shared/Utils';

const $ = window.jQuery;
const classNameToast = 'Toastify__toast--blink';

class Notification extends Component {
  constructor(props) {
    super(props);
    this.toggle = this.toggle.bind(this);
    this.state = {
      assigneeMessages: [],
      assigneeBadge: 0,
      watcherMessages: [],
      watcherBadge: 0,
      watcherPolicy: {
        HasRead: false
      },
      activeTab: 'assignee'
    };
  }

  toggle(activeTab) {
    if (this.state.activeTab !== activeTab) {
      this.setState({
        activeTab
      });
      const isWatcher = activeTab === 'watcher';
      this.updateIsViewed(isWatcher);
    }
  }

  componentDidMount() {
    if (process.env.NODE_ENV !== 'development') {
      // this.initializeSignalR();
      // this.initializeSignalRNetCore();
    }

    this.queryMessages(false);
    (async () => {
      const watcherPolicy = await AuthorizeService.authorize(
        FunctionKeys.NotificationSetting_MySectionMessage
      );
      if (watcherPolicy.HasRead) {
        this.queryMessages(true);
      }
      this.setState({
        watcherPolicy
      });
    })();
  }

  async onClickPreMatchIssueNotification(preMatchIssueId) {
    const model = {
      PreMatchIssueId: preMatchIssueId,
      url: OpenPreMatchIssueURL
    };
    await post(model);
  }

  async handleClickTicketCenterNotification(event, rest) {
    if (event.altKey) {
      openNewChattingTab(rest.actorId, rest.ticketId, rest.eventType);
      return;
    }
    const model = {
      ...rest,
      url: openTicketCenterIssueURL
    };
    await post(model);
  }

  showSuccess = (content, isBlinking = false) => {
    toast.success(content, {
      autoClose: false,
      closeOnClick: true,
      className: isBlinking ? classNameToast : ''
    });
  };

  showTicketCenterMessage = (content, rest) => {
    const { ticketId, eventType } = rest;
    const { UserId } = this.state.watcherPolicy;
    toast.success(content, {
      autoClose: false,
      closeOnClick: true,
      onClick: event =>
        this.handleClickTicketCenterNotification(event, {
          actorId: UserId,
          ticketId,
          eventType
        })
    });
  };

  showError = (content, isBlinking = false) => {
    toast.error(content, {
      autoClose: false,
      closeOnClick: true,
      className: isBlinking ? classNameToast : ''
    });
  };

  showWarn = (content, isBlinking = false) => {
    toast.warn(content, {
      autoClose: false,
      closeOnClick: true,
      className: isBlinking ? classNameToast : ''
    });
  };

  showInfo = (content, preMatchIssueId = 0) => {
    toast.info(content, {
      autoClose: false,
      closeOnClick: true,
      onClick: () => {
        if (preMatchIssueId > 0) {
          this.onClickPreMatchIssueNotification(preMatchIssueId);
        }
      }
    });
  };

  getNotification = (notificationContent, preMatchIssueId) => {
    const {
      AssignmentInsert,
      OtherAssignmentInsert,
      AssignmentDelete,
      OtherAssignmentDelete,
      MatchStatus,
      MinorMatchStatus,
      PreMatchIssue,
      EventCheckingResponse,
      NewAddRequest,
      NewResponse,
      Attachment
    } = MessageTypes;

    const types = [
      {
        type: AssignmentInsert,
        handler: () => this.showSuccess(notificationContent)
      },
      {
        type: OtherAssignmentInsert,
        handler: () => this.showSuccess(notificationContent)
      },
      {
        type: AssignmentDelete,
        handler: () => this.showError(notificationContent)
      },
      {
        type: OtherAssignmentDelete,
        handler: () => this.showError(notificationContent)
      },
      {
        type: MatchStatus,
        handler: () => this.showWarn(notificationContent)
      },
      {
        type: MinorMatchStatus,
        handler: () => this.showWarn(notificationContent)
      },
      {
        type: PreMatchIssue,
        handler: () => this.showInfo(notificationContent, preMatchIssueId)
      },
      {
        type: EventCheckingResponse,
        handler: () => this.showInfo(notificationContent, preMatchIssueId)
      },
      {
        type: NewAddRequest,
        handler: () => this.showInfo(notificationContent, preMatchIssueId)
      },
      {
        type: NewResponse,
        handler: () => this.showInfo(notificationContent, preMatchIssueId)
      },
      {
        type: Attachment,
        handler: () => this.showInfo(notificationContent, preMatchIssueId)
      }
    ];

    return types;
  };

  // initializeSignalR() {
  //   const connection = window.jQuery.connection.notifyHub;
  //   connection.client.notify = data => {
  //     const { messages } = data;

  //     messages.forEach(message => {
  //       if (!message.IsImportant) {
  //         return;
  //       }
  //       window.pushNotification(message);
  //       const messageContent = MessageService.customizeContent(message);
  //       const notificationContent = this.renderToastContent(messageContent);

  //       const notificationTypes = this.getNotification(
  //         notificationContent,
  //         message.PreMatchIssueId
  //       );
  //       const type = notificationTypes.find(
  //         x => x.type === message.MessageType
  //       );

  //       if (type) {
  //         type.handler();
  //       } else {
  //         this.showInfo(notificationContent);
  //       }
  //     });
  //   };

  //   this.startNetFwSignalr();

  //   $.connection.hub.disconnected(() => {
  //     this.connectionFailCount = this.connectionFailCount + 1;
  //     if (this.connectionFailCount < 3) {
  //       setTimeout(() => {
  //         this.startNetFwSignalr();
  //       }, 5000);
  //     }
  //   });
  // }

  startNetFwSignalr() {
    $.connection.hub.start({ transport: 'webSockets' }).done(() => {
      this.connectionFailCount = 0;
    });
  }

  getNetCoreNotificationTypes = (notificationContent, props) => {
    const { isBlinking, ...rest } = props;
    const {
      AssignmentInsert,
      OtherAssignmentInsert,
      AssignmentDelete,
      OtherAssignmentDelete,
      MatchStatus
    } = LiveDetailTypes;

    const {
      OSAssignmentInsert,
      OSOtherAssignmentInsert,
      OSAssignmentDelete,
      OSOtherAssignmentDelete,
      OSAssignmentUpdate,
      OSOtherAssignmentUpdate,
      OSEditMatchStatus
    } = OSMessageTypes;
    const {
      DayShiftAssignmentInsert,
      DayShiftAssignmentDelete,
      DayShiftOtherAssignmentUpdate,
      NightShiftAssignmentInsert,
      NightShiftAssignmentDelete,
      NightShiftOtherAssignmentUpdate,
      LGAAddLiveMatch,
      LGACancelLiveMatch,
      LGAChangeMatchStatus,
      NewLeague,
      UpdateAway,
      UpdateHome,
      UpdateKOTime
    } = PreMatchMessageTypes;

    const { NewTicket, AddRecipient, NewMessage, NewAttachment } =
      TicketCenterMessageTypes;

    const types = [
      {
        type: AssignmentInsert,
        handler: () => this.showSuccess(notificationContent)
      },
      {
        type: OtherAssignmentInsert,
        handler: () => this.showSuccess(notificationContent)
      },
      {
        type: AssignmentDelete,
        handler: () => this.showError(notificationContent)
      },
      {
        type: OtherAssignmentDelete,
        handler: () => this.showError(notificationContent)
      },
      {
        type: MatchStatus,
        handler: () => this.showWarn(notificationContent)
      },
      {
        type: NewTicket,
        handler: () => this.showTicketCenterMessage(notificationContent, rest)
      },
      {
        type: AddRecipient,
        handler: () => this.showTicketCenterMessage(notificationContent, rest)
      },
      {
        type: NewMessage,
        handler: () => this.showTicketCenterMessage(notificationContent, rest)
      },
      {
        type: NewAttachment,
        handler: () => this.showTicketCenterMessage(notificationContent, rest)
      },
      {
        type: OSAssignmentInsert,
        handler: () => this.showSuccess(notificationContent)
      },
      {
        type: OSOtherAssignmentInsert,
        handler: () => this.showSuccess(notificationContent)
      },
      {
        type: OSAssignmentUpdate,
        handler: () => this.showSuccess(notificationContent)
      },
      {
        type: OSOtherAssignmentUpdate,
        handler: () => this.showSuccess(notificationContent)
      },
      {
        type: OSAssignmentDelete,
        handler: () => this.showError(notificationContent)
      },
      {
        type: OSOtherAssignmentDelete,
        handler: () => this.showError(notificationContent)
      },
      {
        type: OSEditMatchStatus,
        handler: () => this.showWarn(notificationContent)
      },
      {
        type: DayShiftAssignmentInsert,
        handler: () => this.showSuccess(notificationContent, isBlinking)
      },
      {
        type: NightShiftAssignmentInsert,
        handler: () => this.showSuccess(notificationContent, isBlinking)
      },
      {
        type: DayShiftOtherAssignmentUpdate,
        handler: () => this.showSuccess(notificationContent, isBlinking)
      },
      {
        type: NightShiftOtherAssignmentUpdate,
        handler: () => this.showSuccess(notificationContent, isBlinking)
      },
      {
        type: DayShiftAssignmentDelete,
        handler: () => this.showError(notificationContent, isBlinking)
      },
      {
        type: NightShiftAssignmentDelete,
        handler: () => this.showError(notificationContent, isBlinking)
      },
      {
        type: LGAChangeMatchStatus,
        handler: () => this.showWarn(notificationContent, isBlinking)
      },
      {
        type: LGACancelLiveMatch,
        handler: () => this.showError(notificationContent, isBlinking)
      },
      {
        type: LGAAddLiveMatch,
        handler: () => this.showSuccess(notificationContent, isBlinking)
      },
      {
        type: NewLeague,
        handler: () => this.showSuccess(notificationContent, isBlinking)
      },
      {
        type: UpdateHome,
        handler: () => this.showSuccess(notificationContent, isBlinking)
      },
      {
        type: UpdateAway,
        handler: () => this.showSuccess(notificationContent, isBlinking)
      },
      {
        type: UpdateKOTime,
        handler: () => this.showSuccess(notificationContent, isBlinking)
      }
    ];

    return types;
  };

  renderToastContent = messageContent => {
    return (
      <div className='toast-content'>
        <div
          className='message-content'
          dangerouslySetInnerHTML={{ __html: messageContent }}
        />
      </div>
    );
  };

  initializeSignalRNetCore() {
    const hub = {
      method: 'notifyChangedInformationData',
      hubName: 'changedInfoNotifyHub'
    };
    const connection = configSignalR(hub.hubName);

    if (connection) {
      connection.start().then(() => {
        connection.on(hub.method, data => {
          const { isImportant, messageContent, messageType, ...rest } =
            data.message;

          if (!isImportant) {
            return;
          }

          const isDevelopment = isEquals(process.env.NODE_ENV, 'development');

          !isDevelopment && window.pushNotification(data.message);

          let formattedMessageContent = MessageService.customizeOSContent(
            messageType,
            messageContent
          );

          formattedMessageContent = MessageService.customizePreMatchContent(
            messageType,
            formattedMessageContent
          );

          formattedMessageContent = MessageService.customizeTicketCenterContent(
            messageType,
            formattedMessageContent
          );

          formattedMessageContent = MessageService.customizeLiveDetailContent(
            messageType,
            formattedMessageContent
          );

          const notificationContent = this.renderToastContent(
            formattedMessageContent
          );
          const notificationTypes = this.getNetCoreNotificationTypes(
            notificationContent,
            rest
          );
          isEqual(process.env.NODE_ENV, 'production') &&
            // eslint-disable-next-line no-warning-comments
            documentTitle.BlinkDocumentTitle(data.message); // NOSONAR

          const type = notificationTypes.find(x => x.type === messageType);

          if (type) {
            type.handler();
          } else {
            this.showInfo(notificationContent);
          }
        });
      });
    }

    $.connection.hub.disconnected(function () {
      setTimeout(function () {
        $.connection.hub.start();
      }, 5000);
    });
  }

  onBadgeClick() {
    const { activeTab } = this.state;
    const isWatcher = activeTab === 'watcher';
    this.updateIsViewed(isWatcher);
  }

  updateIsViewed(isWatcher) {
    const { assigneeBadge, watcherBadge } = this.state;
    const model = {
      isWatcher
    };
    if (isWatcher && watcherBadge > 0) {
      MessageService.updateIsViewed(model).then(() => {
        this.setState({ watcherBadge: 0 });
      });
    } else if (!isWatcher && assigneeBadge > 0) {
      MessageService.updateIsViewed(model).then(() => {
        this.setState({ assigneeBadge: 0 });
      });
    }
  }

  queryMessages(isWatcher) {
    (async () => {
      const model = {
        selectTop: 10,
        isWatcher
      };
      const data = await MessageService.query(model);
      const messages = data.Messages;
      if (isWatcher) {
        let { watcherMessages } = this.state;
        const watcherBadge = data.TotalNotViewMessage;
        watcherMessages = watcherMessages.concat(messages);
        this.setState({
          watcherBadge,
          watcherMessages
        });
      } else {
        let { assigneeMessages } = this.state;
        const assigneeBadge = data.TotalNotViewMessage;
        assigneeMessages = assigneeMessages.concat(messages);
        this.setState({
          assigneeBadge,
          assigneeMessages
        });
      }
    })();
  }

  renderNotificationItems = messages => {
    return (
      <>
        {messages &&
          messages.length > 0 &&
          messages.map((item, index) => {
            return (
              <NotificationItem
                key={`NotificationItem_${item.MessageId}_${index}`}
                data={item}
              />
            );
          })}
        {messages.length === 0 && (
          <div className='noti-item'>No new message.</div>
        )}
      </>
    );
  };

  getBadgeText(badge) {
    if (badge > 99) {
      return '99+';
    }

    return badge > 0 ? badge.toString() : '';
  }

  getBadgeTextOnTab(badge) {
    return badge > 0 ? badge.toString() : '';
  }

  redirectMessageBoxPage() {
    window.location.href = Utils.resolveUrl('/MessageBoxManagement');
  }

  render() {
    const {
      assigneeBadge,
      assigneeMessages,
      watcherBadge,
      watcherMessages,
      watcherPolicy,
      activeTab
    } = this.state;

    const badge = assigneeBadge + watcherBadge;
    return (
      <CDropdownHeader direction='down' className='notification'>
        <UncontrolledDropdown>
          <DropdownToggle nav onClick={() => this.onBadgeClick()}>
            <BellIcon size={20} className='icon-notification' />
            <Badge color='new'>{this.getBadgeText(badge)}</Badge>
          </DropdownToggle>
          <DropdownMenu right>
            <Nav tabs className='tabs-notification'>
              <NavItem>
                <NavLink
                  className={classnames({ active: activeTab === 'assignee' })}
                  onClick={() => {
                    this.toggle('assignee');
                  }}>
                  My Message{' '}
                  <Badge color='new'>
                    {this.getBadgeTextOnTab(assigneeBadge)}
                  </Badge>
                </NavLink>
              </NavItem>
              <NavItem hidden={!watcherPolicy.HasRead}>
                <NavLink
                  className={classnames({ active: activeTab === 'watcher' })}
                  onClick={() => {
                    this.toggle('watcher');
                  }}>
                  My Section Message{' '}
                  <Badge color='new'>
                    {this.getBadgeTextOnTab(watcherBadge)}
                  </Badge>
                </NavLink>
              </NavItem>
            </Nav>
            <TabContent activeTab={activeTab}>
              <TabPane tabId='assignee'>
                {this.renderNotificationItems(assigneeMessages)}
              </TabPane>
              <TabPane tabId='watcher'>
                {this.renderNotificationItems(watcherMessages)}
              </TabPane>
            </TabContent>
            <div
              className='show-all'
              onClick={() => this.redirectMessageBoxPage()}>
              Show all
            </div>
          </DropdownMenu>
        </UncontrolledDropdown>
      </CDropdownHeader>
    );
  }
}

export default Notification;
