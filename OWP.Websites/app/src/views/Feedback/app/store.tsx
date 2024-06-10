import { create } from 'zustand';
import update from 'immutability-helper';
import { RateIconEnum, TFeedback, StatusEnum } from '../type';
import { queryUserFeedbackInfo } from '../services.native';
import { validateInfoCreateFeedback } from '../utils.native';
import { RECIPIENT_TYPE } from '../const.json';
import {
  formatRecipientModel,
  getRecipientDefaultBySubType,
  saveFileToBackEnd
} from '../../Shared/createTicketNew/Helper/HelperCreateTicket';
import { urlRequestOther } from '../../Shared/createTicketNew/const/const';
import { post } from '../../Shared/Service/Service';

const { INDIVIDUAL } = RECIPIENT_TYPE;
const useStore = create<TFeedback>((set, get) => ({
  feedBackInfo: {
    functionName: '',
    rate: RateIconEnum.DEFAULT,
    inputMessage: '',
    selectedFile: null,
    selectedImage: null
  },
  statusForm: StatusEnum.FEED_BACK,
  setStatusForm: (status: StatusEnum) => {
    set(state =>
      update(state, {
        statusForm: { $set: status }
      })
    );
  },
  setRateStatus: (rate: RateIconEnum) => {
    set(state =>
      update(state, {
        feedBackInfo: {
          rate: { $set: rate }
        }
      })
    );
  },
  setFunctionName: (functionName: string) => {
    set(state =>
      update(state, {
        feedBackInfo: {
          functionName: { $set: functionName }
        }
      })
    );
  },
  validateBeforeCreate: () => validateInfoCreateFeedback(get()),
  postFeedback: async () => {
    const { selectedFile, selectedImage, functionName, rate, inputMessage } =
      get().feedBackInfo;
    const userFeedbackInfo = await queryUserFeedbackInfo();
    const creatorInfo = {
      groupName: null,
      nickName: userFeedbackInfo.creatorName,
      recipientId: userFeedbackInfo.creatorId,
      recipientType: INDIVIDUAL,
      userName: userFeedbackInfo.creatorName
    };
    const recipientDefaults = await getRecipientDefaultBySubType(
      userFeedbackInfo.subtypeId
    );
    const Recipients = formatRecipientModel(recipientDefaults, creatorInfo);

    const files = [selectedFile, selectedImage].filter(x => x != null);
    const fileResults = await saveFileToBackEnd(files);

    const newAddModel = {
      Files: fileResults,
      CreatorId: creatorInfo.recipientId,
      Subject: `${functionName} - ${rate}`,
      Recipients,
      Message: inputMessage.length > 0 ? inputMessage : 'Please add',
      SubType: userFeedbackInfo.subtypeId,
      TicketType: userFeedbackInfo.ticketType,
      url: urlRequestOther
    };
    const result = await post(newAddModel);
    result &&
      set(state =>
        update(state, {
          statusForm: {
            $set: StatusEnum.THANK_YOU
          }
        })
      );
  },
  updateFeedbackInfo: (key: string, value: string | File | null) => {
    set(state =>
      update(state, {
        feedBackInfo: {
          [key]: { $set: value }
        }
      })
    );
  }
}));

export default useStore;
