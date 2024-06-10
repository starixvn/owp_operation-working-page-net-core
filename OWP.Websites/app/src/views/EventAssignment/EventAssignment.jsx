// eslint-disable-next-line no-warning-comments
/* eslint-disable react/jsx-handler-names */
import { createRef, Component } from 'react';
import Filter from './Filter';
import FilterToggle from './FilterToggle';
import EventAssignmentService from './Service';
import AuthorizeService from '../Authorize/Service';
import UserService from '../Shared/User/UserService';
import ResourceManagementService from '../ResourceManagement/Service';
import Loading from '../components/Loading';
import Utils from '../Shared/Utils';
import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import axios from 'axios';
import { HighlightUserContext } from '../Shared/Context';
import NoteContext from '../LiveDetail/MatchNote/NoteContext';
import AddEvent from './AddEvent';
import { FunctionKeys, EventNames } from '../Shared/Constants';
import Toastr from '../components/ToastrWrapper';
import { cloneDeep } from 'lodash';
import { v4 as uuidv4 } from 'uuid';
import EventAssignmentTable from './EventAssignmentTable';
import dayjs from 'dayjs';
import { RECIPIENT_TYPE } from '../Shared/createTicketNew/const/const';
import CreateTicketLGAView from '../Shared/createTicketNew/CreateTicketLGAView';
import CreateTicketBoardOther from '../Shared/createTicketNew/CreateTicketBoardOther';
import CreateTicketLiveIncident from '../Shared/createTicketNew/CreateTicketLiveIncident';
import GoToMyTicket from '../Shared/TicketCenterPrematch/GoToMyTicket';
import TicketDetailBoard from '../Shared/chatBoardNew/TicketDetailBoard';
import { connect } from 'react-redux';
import { setDataEvent, setUserId } from './reducer/tableReducer';
import { queryPolicy } from '../Shared/permission/policySlice';
import { selectOffVROptions } from './FilterHelper';
const { INDIVIDUAL } = RECIPIENT_TYPE;
class EventAssignment extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isLoading: true,
      creatorInfo: {},
      showUnreadMessage: false,
      policy: {
        HasWrite: false,
        HasRead: false,
        FunctionKey: '',
        MyTicket: {
          HasWrite: false
        }
      },
      dailyWorkingShiftPolicy: {
        HasWrite: false,
        HasRead: false
      },
      notePolicy: {},
      events: [],
      userName: '',
      userId: null,
      bettypeColumns: [],
      modalAddEvent: false,
      modalDetailInformation: false,
      detailMatchId: null,
      selectedDate: new Date(),
      isAllowAddEvent: true,
      matchIsScrollTo: 0,
      scrollLeft: 0,
      selectedMatchStatus: [],
      selectedSyncLive: 0,
      columnFilters: [],
      tableKey: uuidv4(),
      dateUpdated: '',
      syncOffVRLastUpdated: '',
      syncOffVRByUser: ''
    };
    this.originalPolicy = {};
    this.originalNotePolicy = {};
    this.onUpdateNotes = this.onUpdateNotes.bind(this);
    this.onToggleModal = this.onToggleModal.bind(this);
    this.onAddSpecialEvent = this.onAddSpecialEvent.bind(this);
    this.onOpenDetailInformation = this.onOpenDetailInformation.bind(this);
    this.onColumnFiltersChange = this.onColumnFiltersChange.bind(this);
    this.submit = this.submit.bind(this);
    this.onSaved = this.onSaved.bind(this);
    this.onCloseDetailModal = this.onCloseDetailModal.bind(this);
    this.iframeRef = createRef();
  }

  onCloseDetailModal = () => {
    this.setState({
      modalDetailInformation: false
    });
    this.query();
  };

  componentDidMount() {
    window.addEventListener(
      EventNames.CloseDetailModal,
      this.onCloseDetailModal
    );
    (async () => {
      const [
        fullBetTypesPolicy,
        pHBetTypesPolicy,
        notePolicy,
        dailyWorkingShiftPolicy,
        oddsPolicy,
        eventPickingPolicy,
        matchStatuses,
        offVR,
        myTicketPolicy
      ] = await axios.all([
        AuthorizeService.authorize(
          FunctionKeys.LiveGameAssignment_FullBetTypes
        ),
        AuthorizeService.authorize(FunctionKeys.LiveGameAssignment_PHBetTypes),
        AuthorizeService.authorize(FunctionKeys.Note),
        AuthorizeService.authorize(FunctionKeys.DailyWorkingShift),
        AuthorizeService.authorize(FunctionKeys.OddsCompiler),
        AuthorizeService.authorize(FunctionKeys.EventPicking),
        ResourceManagementService.getByRefName('MatchStatus'),
        ResourceManagementService.getByRefName('MatchOffer'),
        AuthorizeService.authorize(FunctionKeys.TicketCenter),
        UserService.dataSourceUsersFromServer()
      ]);
      // eslint-disable-next-line no-warning-comments
      // eslint-disable-next-line react/prop-types
      this.props.dispatch(setUserId(fullBetTypesPolicy.UserId));
      // eslint-disable-next-line no-warning-comments
      // eslint-disable-next-line react/prop-types
      this.props.dispatch(
        queryPolicy()({ functionKey: FunctionKeys.TicketCenter })
      );
      const policy = {
        UserId: fullBetTypesPolicy.UserId,
        Username: fullBetTypesPolicy.Username,
        FullBetTypes: {
          HasRead: fullBetTypesPolicy.HasRead,
          HasWrite: fullBetTypesPolicy.HasWrite
        },
        PHBetTypes: {
          HasRead: pHBetTypesPolicy.HasRead,
          HasWrite: pHBetTypesPolicy.HasWrite
        },
        EventPicking: {
          HasRead: eventPickingPolicy.HasRead,
          HasWrite: eventPickingPolicy.HasWrite
        },
        HasWrite: fullBetTypesPolicy.HasWrite,
        MyTicket: myTicketPolicy
      };
      const creatorInfo = {
        groupName: null,
        nickName: fullBetTypesPolicy.Username,
        recipientId: fullBetTypesPolicy.UserId,
        recipientType: INDIVIDUAL,
        userName: fullBetTypesPolicy.Username
      };
      this.originalPolicy = cloneDeep(policy);
      this.originalNotePolicy = cloneDeep(notePolicy);
      this.setState({
        dailyWorkingShiftPolicy,
        policy,
        notePolicy,
        oddsPolicy,
        matchStatuses,
        offVR: selectOffVROptions(offVR),
        isLoading: false,
        creatorInfo,
        userId: fullBetTypesPolicy.UserId
      });
    })();
  }

  componentWillUnmount() {
    window.removeEventListener(
      EventNames.CloseDetailModal,
      this.onCloseDetailModal
    );
  }

  onColumnFiltersChange = (columnFilters, scrollLeft) => {
    this.setState({
      columnFilters,
      scrollLeft,
      matchIsScrollTo: 0,
      tableKey: uuidv4()
    });
  };

  submit = (e, isAllowAddEvent, matchId) => {
    this.setState(
      {
        userName: e.userName || '',
        selectedDate: e.selectedDate || new Date(),
        selectedMatchType: e.selectedMatchType,
        selectedMatchStatus: e.selectedMatchStatus || [],
        selectedOffVRs: e.selectedOffVRs || [],
        selectedSyncLive: e.selectedSyncLive || 0,
        matchIsScrollTo: matchId || 0,
        scrollLeft: 0,
        isAllowAddEvent,
        showUnreadMessage: e.showUnreadMessage
      },
      this.query
    );
  };

  query = () => {
    (async () => {
      const {
        selectedDate,
        userName,
        selectedMatchStatus,
        selectedOffVRs,
        policy,
        selectedSyncLive,
        notePolicy,
        showUnreadMessage,
        selectedMatchType
      } = this.state;
      this.setState({
        isLoading: true
      });
      const postModel = {
        EventDate: Utils.formatDate(selectedDate),
        Username: userName || '',
        MatchStatusIds: selectedMatchStatus || [],
        SelectedOffVRs: selectedOffVRs || [],
        MatchType: selectedMatchType || [],
        SyncLiveStatus: selectedSyncLive,
        ShowUnreadMessage: showUnreadMessage
      };
      const events = await EventAssignmentService.query(postModel);
      // eslint-disable-next-line no-warning-comments
      // eslint-disable-next-line react/prop-types
      this.props.dispatch(setDataEvent(events));

      const lastUpdate = await EventAssignmentService.getLastUpdated(
        postModel.EventDate
      );
      const syncOffVRLastUpdate =
        await EventAssignmentService.getSyncOffVRLastUpdated(
          postModel.EventDate
        );
      const syncOffVRLastTime = syncOffVRLastUpdate.Item1;
      const syncOffVRByUser = syncOffVRLastUpdate.Item2;
      const isYesterdayOrAfter =
        Utils.isYesterdayOrAfterForAssignment(selectedDate);
      policy.FullBetTypes.HasWrite =
        this.originalPolicy.FullBetTypes.HasWrite && isYesterdayOrAfter;
      policy.PHBetTypes.HasWrite =
        this.originalPolicy.PHBetTypes.HasWrite && isYesterdayOrAfter;
      notePolicy.HasWrite =
        this.originalNotePolicy.HasWrite && isYesterdayOrAfter;
      this.setState({
        events,
        policy,
        isLoading: false,
        tableKey: uuidv4(),
        dateUpdated: lastUpdate
          ? dayjs(lastUpdate.substring(0, lastUpdate.length - 6)).format(
              'MMM DD, hh:mm A'
            )
          : '',
        syncOffVRLastUpdated: syncOffVRLastTime,
        syncOffVRByUser
      });
    })();
  };

  onSaved = matchId => {
    this.onToggleModal();
    this.setState({
      matchIsScrollTo: matchId
    });
    this.query();
  };

  onSelectedToggle = e => {
    this.setState({ bettypeColumns: e, tableKey: uuidv4() });
  };

  onToggleModal = () => {
    this.setState(prevState => ({
      modalAddEvent: !prevState.modalAddEvent,
      parentEvent: null
    }));
  };

  onAddSpecialEvent = event => {
    this.setState(prevState => ({
      modalAddEvent: !prevState.modalAddEvent,
      parentEvent: event
    }));
  };

  onOpenDetailInformation = matchId => {
    this.setState({
      modalDetailInformation: true,
      detailMatchId: matchId
    });
  };

  onCloseModal = () => {
    const updateResult = parseInt(
      this.iframeRef.current.getAttribute('updated'),
      10
    );
    if (updateResult > 0) {
      this.iframeRef.current.setAttribute('updated', 0);
      this.setState({
        matchIsScrollTo: this.state.detailMatchId,
        modalDetailInformation: false
      });
      this.query();
    } else {
      this.setState({
        modalDetailInformation: false
      });
    }
  };

  onHideDetailInformation = () => {
    const editGeneralInfo =
      this.iframeRef.current.getAttribute('edit-general-info');
    const editMatchInfo =
      this.iframeRef.current.getAttribute('edit-match-info');
    const editTradingInfo =
      this.iframeRef.current.getAttribute('edit-trading-info');
    const editProviderInfo =
      this.iframeRef.current.getAttribute('edit-provider-info');

    const isEditGeneralAndMatchInfo =
      editGeneralInfo === '1' || editMatchInfo === '1';
    const isEditTradingAndProviderInfo =
      editTradingInfo === '1' || editProviderInfo === '1';
    if (isEditGeneralAndMatchInfo || isEditTradingAndProviderInfo) {
      Toastr.confirm('Your unsaved changes will be lost. Are you sure?').then(
        confirmResult => {
          if (confirmResult.value) {
            this.onCloseModal();
          }
        }
      );
    } else {
      this.onCloseModal();
    }
  };

  render() {
    const {
      isLoading,
      creatorInfo,
      policy,
      notePolicy,
      oddsPolicy,
      matchStatuses,
      offVR: offVRs,
      events,
      userName,
      selectedDate,
      bettypeColumns,
      parentEvent,
      modalAddEvent,
      modalDetailInformation,
      detailMatchId,
      matchIsScrollTo,
      scrollLeft,
      isAllowAddEvent,
      columnFilters,
      dailyWorkingShiftPolicy,
      tableKey,
      dateUpdated,
      syncOffVRLastUpdated,
      syncOffVRByUser
    } = this.state;

    return (
      <div className='event-assignment'>
        {isLoading && <Loading />}
        <CreateTicketLGAView creatorInfo={creatorInfo} />
        <CreateTicketBoardOther creatorInfo={creatorInfo} />
        <CreateTicketLiveIncident creatorInfo={creatorInfo} />
        <TicketDetailBoard />
        {matchStatuses && offVRs && (
          <div className='filter'>
            <Filter
              matchStatuses={matchStatuses}
              offVRs={offVRs}
              hasWrite={policy.HasWrite}
              onChangeDate={this.onChangeDate}
              isHostEventPicking={policy.EventPicking.HasWrite}
              submit={this.submit}
              exportExcel={this.exportExcel}
              onToggleModal={this.onToggleModal}
              isAllowAddEvent={isAllowAddEvent}
              lastUpdated={dateUpdated}
            />
            <div className='filter-control-right'>
              <div className='top-info'>
                <span className='sync-off-vr-last-update'>
                  Sync Off VR by {syncOffVRByUser}: {syncOffVRLastUpdated}
                </span>
                <span className='lastupdate'>
                  | Last Updated: {dateUpdated}
                </span>
                <GoToMyTicket />
              </div>
              <FilterToggle
                onSelected={e => this.onSelectedToggle(e)}
                selectedDate={selectedDate}
                policy={policy}
                dailyWorkingShiftPolicy={dailyWorkingShiftPolicy}
              />
            </div>
          </div>
        )}
        {matchStatuses && (
          <HighlightUserContext.Provider value={{ userName }}>
            <NoteContext.Provider value={{ onUpdateNotes: this.onUpdateNotes }}>
              {events && bettypeColumns && (
                <EventAssignmentTable
                  key={tableKey}
                  dataSource={events}
                  notePolicy={notePolicy}
                  bettypeColumns={bettypeColumns}
                  policy={policy}
                  isAllowAddEvent={isAllowAddEvent}
                  addSpecialEvent={this.onAddSpecialEvent}
                  openDetailInformation={this.onOpenDetailInformation}
                  matchIsScrollTo={matchIsScrollTo}
                  scrollLeft={scrollLeft}
                  columnFiltersChange={this.onColumnFiltersChange}
                  columnFilters={columnFilters}
                />
              )}
            </NoteContext.Provider>
          </HighlightUserContext.Provider>
        )}
        {modalAddEvent && (
          <AddEvent
            oddsPolicy={oddsPolicy}
            parentEvent={parentEvent}
            selectedDate={selectedDate}
            matchStatuses={matchStatuses}
            policy={policy}
            onToggleModal={this.onToggleModal}
            onSaved={this.onSaved}
          />
        )}
        {modalDetailInformation && (
          <Modal
            tag='div'
            size='lg'
            isOpen
            className='modal-dark-title modal-edit-event'>
            <ModalHeader toggle={this.onHideDetailInformation}>
              Detail Information
            </ModalHeader>
            <ModalBody tag='div'>
              <div className='masonry'>
                <iframe
                  ref={this.iframeRef}
                  style={{
                    overflow: 'hidden',
                    height: '100%',
                    width: '100%',
                    minHeight: '700px',
                    border: '0'
                  }}
                  src={
                    Utils.resolveUrl('/EventManagement/LiveDetail/Index/') +
                    detailMatchId
                  }
                />
              </div>
            </ModalBody>
          </Modal>
        )}
      </div>
    );
  }

  onUpdateNotes(matchId, notes) {
    const { events } = this.state;
    const filterMatches = events.filter(e => e.OWPMatchId === matchId);
    filterMatches[0].Notes = notes;
    this.setState({ events });
  }
}
const mapStateToProps = state => ({
  dataEvent: state.tableReducer.dataEvent
});

export default connect(mapStateToProps, null)(EventAssignment);
