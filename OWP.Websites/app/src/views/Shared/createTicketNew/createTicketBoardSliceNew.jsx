import { createSlice } from '@reduxjs/toolkit';

const createTicketBoardSliceNew = createSlice({
  name: 'createTicketBoardNew',
  initialState: {
    isOpenCreateTicketInLGA: false,
    isOpenPopup: false,
    isEventChecking: false,
    isLiveIncident: false,
    isOthers: false,
    isOpenLogWork: false,
    valueEvent: null,
    valueOther: '',
    creatorInfo: null,
    selectedRecipients: [],
    defaultRecipients: [],
    errorOther: false,
    errorInput: false,
    errorRecipient: false,
    errorTextField: '',
    eventChecking: [],
    recipientAssigned: [],
    typeTicketText: '',
    subTypeOption: [],
    subTypeSelected: []
  },
  reducers: {
    setDefaultRecipients: {
      reducer: (state, { payload }) => {
        state.defaultRecipients = payload;
      }
    },
    setSubTypeSelected: {
      reducer: (state, action) => {
        state.subTypeSelected = action.payload;
      }
    },
    setSubTypeOption: {
      reducer: (state, action) => {
        state.subTypeOption = action.payload;
      }
    },
    setTypeTicketText: {
      reducer: (state, action) => {
        state.typeTicketText = action.payload;
      }
    },
    setRecipientAssigned: {
      reducer: (state, action) => {
        state.recipientAssigned = action.payload;
      }
    },
    setEventChecking: {
      reducer: (state, action) => {
        state.eventChecking = action.payload;
      }
    },
    changeErrorTextField: {
      reducer: (state, action) => {
        state.errorTextField = action.payload;
      }
    },
    setSelectedRecipients: {
      reducer: (state, action) => {
        state.selectedRecipients = action.payload;
      }
    },
    changeErrorOther: {
      reducer: (state, action) => {
        state.errorOther = action.payload;
      }
    },
    changeErrorInput: {
      reducer: (state, action) => {
        state.errorInput = action.payload;
      }
    },
    changeErrorRecipient: {
      reducer: (state, action) => {
        state.errorRecipient = action.payload;
      }
    },
    otherSubject: {
      reducer: (state, action) => {
        state.valueOther = `${action.payload}`;
      }
    },
    selectedEvent: {
      reducer: (state, action) => {
        state.valueEvent = action.payload;
      }
    },
    closeCreatePopup: {
      reducer: state => {
        state.isOpenPopup = false;
        state.isEventChecking = false;
        state.isOthers = false;
        state.isLiveIncident = false;
        state.isOpenLogWork = false;
      }
    },
    openPopupEventChecking: {
      reducer: state => {
        state.subTypeSelected = [{ value: 0, label: '' }];
        state.errorInput = false;
        state.errorRecipient = false;
        state.valueEvent = null;
        state.selectedRecipients = state.defaultRecipients;
        state.isOpenPopup = true;
        state.isEventChecking = true;
        state.isLiveIncident = false;
        state.isOthers = false;
      }
    },
    openPopupLiveIncident: {
      reducer: state => {
        state.subTypeSelected = [{ value: 0, label: '' }];
        state.errorInput = false;
        state.errorRecipient = false;
        state.valueEvent = null;
        state.selectedRecipients = state.defaultRecipients;
        state.isOpenPopup = true;
        state.isLiveIncident = true;
        state.isEventChecking = false;
        state.isOthers = false;
      }
    },
    openPopupOthers: {
      reducer: state => {
        state.subTypeSelected = [{ value: 0, label: '' }];
        state.selectedRecipients = state.defaultRecipients;
        state.errorOther = false;
        state.errorRecipient = false;
        state.valueOther = '';
        state.isOpenPopup = true;
        state.isOthers = true;
        state.isEventChecking = false;
        state.isLiveIncident = false;
      }
    },
    closePopupCreateTicketInLGA: {
      reducer: state => {
        state.isOpenCreateTicketInLGA = false;
      }
    },
    openPopupCreateTicketInLGA: {
      reducer: state => {
        state.isOpenCreateTicketInLGA = true;
        state.errorRecipient = false;
        state.subTypeSelected = [{ value: 0, label: '' }];
        state.selectedRecipients = state.defaultRecipients;
      }
    },
    openPopupLogWork: {
      reducer: state => {
        state.isEventChecking = false;
        state.isLiveIncident = false;
        state.isOthers = false;
        state.isOpenLogWork = true;
      }
    },
    closePopupLogWork: {
      reducer: state => {
        state.isOpenLogWork = false;
      }
    }
  }
});

const { actions, reducer } = createTicketBoardSliceNew;
const {
  closeCreatePopup,
  openPopupEventChecking,
  openPopupLiveIncident,
  openPopupOthers,
  selectedEvent,
  otherSubject,
  changeErrorInput,
  changeErrorRecipient,
  changeErrorOther,
  setSelectedRecipients,
  changeErrorTextField,
  setEventChecking,
  setRecipientAssigned,
  setTypeTicketText,
  setDefaultRecipients,
  setSubTypeOption,
  setSubTypeSelected,
  closePopupCreateTicketInLGA,
  openPopupCreateTicketInLGA,
  openPopupLogWork,
  closePopupLogWork
} = actions;

export {
  closeCreatePopup,
  openPopupEventChecking,
  openPopupLiveIncident,
  openPopupOthers,
  selectedEvent,
  otherSubject,
  changeErrorInput,
  changeErrorRecipient,
  changeErrorOther,
  setSelectedRecipients,
  changeErrorTextField,
  setEventChecking,
  setRecipientAssigned,
  setTypeTicketText,
  setDefaultRecipients,
  setSubTypeOption,
  setSubTypeSelected,
  closePopupCreateTicketInLGA,
  openPopupCreateTicketInLGA,
  openPopupLogWork,
  closePopupLogWork
};

export default reducer;
