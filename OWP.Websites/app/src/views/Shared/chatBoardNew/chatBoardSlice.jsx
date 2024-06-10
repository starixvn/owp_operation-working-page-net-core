import { createSlice, current } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../Constants';
import {
  saveMessage,
  getTicketDetail,
  createNewMessage,
  getModelByRowIndex,
  getTicketSubTypes,
  loadMoreMessageThunk
} from './chatBoardThunkNative';

const chatBoardSlice = createSlice({
  name: 'chatBoard',
  initialState: {
    isOpenPopup: false,
    isOpenTicket: false,
    modelByRowIndex: {},
    loading: REQUEST_STATUS.idle,
    loadMoreMessage: REQUEST_STATUS.idle,
    error: null,
    isShowAllMessage: false,
    ticketDetail: {
      ticketId: '',
      eventType: 0,
      ticketTypeId: '',
      ticketTypeName: '',
      ticketSubTypeId: 0,
      ticketSubTypeName: '',
      canShowMoreMessage: false,
      messages: [],
      recipients: [],
      owpLeagueId: '',
      owpLeagueName: '',
      owpHomeId: '',
      owpAwayId: '',
      homeName: '',
      awayName: '',
      subject: '',
      ticketStatus: '',
      eventName: '',
      creatorInfo: {
        id: 0,
        userName: '',
        nickName: ''
      }
    },
    ticketSubTypeOptions: [],
    inputMessage: '',
    modelQuoteMessage: {},
    imagesLoaded: 0,
    isAllImagesLoaded: false,
    isDisplayCloseButton: true,
    isBlinkTitle: false
  },
  reducers: {
    updateSubTypeTicketDetailByTicketId: {
      reducer: (state, action) => {
        const { ticketId, ticketSubTypeId, ticketSubTypeName } = action.payload;
        if (state.ticketDetail.ticketId === ticketId) {
          state.ticketDetail.ticketSubTypeId = ticketSubTypeId;
          state.ticketDetail.ticketSubTypeName = ticketSubTypeName;
        }
      }
    },
    setModelQuoteMessage: {
      reducer: (state, action) => {
        state.modelQuoteMessage = action.payload;
      }
    },
    updateRecipient: {
      reducer: (state, action) => {
        state.ticketDetail.recipients = action.payload;
      }
    },
    setMessage: {
      reducer: (state, action) => {
        state.inputMessage = action.payload;
      }
    },
    openTicketPopup: {
      reducer: state => {
        state.isOpenTicket = true;
        state.isDisplayCloseButton = true;
      }
    },
    closeTicketPopup: {
      reducer: state => {
        state.isOpenTicket = false;
      }
    },
    closePopup: {
      reducer: state => {
        state.isOpenPopup = false;
      }
    },
    openPopup: {
      reducer: state => {
        state.isOpenPopup = true;
        state.isDisplayCloseButton = true;
        state.imagesLoaded = 0;
        state.isAllImagesLoaded = false;
      }
    },
    setImagesLoaded: {
      reducer: state => {
        state.imagesLoaded++;
        const msgImage = state.ticketDetail.messages.filter(
          msg => msg.imageFileName
        );

        if (state.imagesLoaded === msgImage.length) {
          state.isAllImagesLoaded = true;
        }
      }
    },
    setNewMessage: (state, { payload: data }) => {
      state.ticketDetail.messages.push(data);
    },
    setTicketStatus: (state, { payload: data }) => {
      state.ticketDetail.ticketStatus = data;
    },
    setIsDisplayCloseButton: (state, { payload: data }) => {
      state.isDisplayCloseButton = data;
    },
    setIsBlinkTitle: (state, { payload: data }) => {
      state.isBlinkTitle = data;
    },
    updateMessageIconRealTime: (
      state,
      { payload: { messageReactIcon, action } }
    ) => {
      const msg = state.ticketDetail.messages.find(
        msg => msg.messageId === messageReactIcon.messageId
      );
      msg.messageReactIcons =
        action === 'Inserted'
          ? [...msg.messageReactIcons, messageReactIcon]
          : msg.messageReactIcons.filter(
              x =>
                x.iconKey !== messageReactIcon.iconKey ||
                x.reactUserId !== messageReactIcon.reactUserId
            );
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getModelByRowIndex.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(getModelByRowIndex.fulfilled, (state, action) => {
        const { ...rest } = action.payload;
        state.loading = REQUEST_STATUS.idle;
        state.modelByRowIndex = rest;
      })
      .addCase(getModelByRowIndex.rejected, (state, action) => {
        state.loading = REQUEST_STATUS.idle;
        state.error = action.error;
      })
      .addCase(saveMessage.fulfilled, (state, action) => {
        const { messageId, isSaved } = action.payload;
        const message = state.ticketDetail.messages.find(
          x => x.messageId === messageId
        );
        message.isSaved = isSaved;
      })
      .addCase(getTicketDetail.fulfilled, (state, action) => {
        state.ticketDetail = action.payload.data;
        state.loading = REQUEST_STATUS.idle;
        chatBoardSlice.caseReducers.openPopup(state);
      })
      .addCase(getTicketSubTypes.fulfilled, (state, { payload }) => {
        state.ticketSubTypeOptions = payload;
      })
      .addCase(loadMoreMessageThunk.pending, state => {
        state.loadMoreMessage = REQUEST_STATUS.pending;
      })
      .addCase(loadMoreMessageThunk.fulfilled, (state, { payload }) => {
        const { canShowMoreMessage, messages } = payload;
        state.loadMoreMessage = REQUEST_STATUS.idle;
        state.ticketDetail.canShowMoreMessage = canShowMoreMessage;
        state.ticketDetail.messages = messages.concat(
          state.ticketDetail.messages
        );
      });
  }
});

const { actions, reducer } = chatBoardSlice;

const {
  updateRecipient,
  setMessage,
  closePopup,
  openTicketPopup,
  closeTicketPopup,
  setModelQuoteMessage,
  setImagesLoaded,
  setNewMessage,
  setTicketStatus,
  updateSubTypeTicketDetailByTicketId,
  setIsDisplayCloseButton,
  setIsBlinkTitle,
  updateMessageIconRealTime
} = actions;

export {
  actions,
  updateRecipient,
  setMessage,
  getTicketDetail,
  createNewMessage,
  closePopup,
  saveMessage,
  getModelByRowIndex,
  openTicketPopup,
  closeTicketPopup,
  getTicketSubTypes,
  setModelQuoteMessage,
  setImagesLoaded,
  setNewMessage,
  setTicketStatus,
  updateSubTypeTicketDetailByTicketId,
  setIsDisplayCloseButton,
  setIsBlinkTitle,
  updateMessageIconRealTime
};

export default reducer;
