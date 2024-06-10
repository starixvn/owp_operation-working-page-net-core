import Axios from 'axios';
import { chain } from 'lodash';
import Service from '../Service';
import { urlUploadFile } from '../const/const';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { TICKET_TYPE } from '../../TicketCenterPrematch/const/const.json';

const informResult = result => {
  if (result) {
    ToastrWrapper.showInfo(<span>Your ticket has been created</span>);
  } else {
    ToastrWrapper.showError(<span>Please try again</span>);
  }
};

const formatRecipientModel = (recipients, recipientByCreator) => {
  const result = recipients.map(x => x);
  result.push(recipientByCreator);
  const seen = new Map();
  const filteredArr = result.filter(el => {
    const duplicate =
      seen.has(el.recipientId) && seen.get(el.recipientId) === el.recipientType;
    seen.set(el.recipientId, el.recipientType);
    return !duplicate && el.recipientId;
  });
  const reformatted = chain(filteredArr)
    .groupBy('recipientType')
    .map((value, key) => ({
      RecipientType: parseInt(key, 10),
      RecipientIds: value.map(x => x.recipientId)
    }))
    .value();
  return reformatted;
};

const getRecipientDefaultBySubType = async ticketTypeId => {
  const recipientByType = await Service.recipientBySubType(ticketTypeId);
  return recipientByType.data;
};

const getSubTypeByType = async ticketTypeId => {
  const recipientByType = await Service.getSubType(ticketTypeId);
  return recipientByType.data;
};

const saveFileToBackEnd = async files => {
  const formData = new FormData();
  const fl = files.length;
  let count = 0;
  while (count < fl) {
    const file = files[count];
    formData.append('files', file, file.name);
    count++;
  }
  const response = await Axios.post(urlUploadFile, formData);
  const fileId = response.data;
  return fileId;
};
const filterResult = value => {
  const event = value.map(x => x.eventInfoViewModel);
  return event.reduce((a, b) => a.concat(b), []);
};

const filterFeedbackSubtype = (ticketType, option) => {
  if (ticketType === TICKET_TYPE.GROUP_CHAT) {
    return option.filter(x => x.label !== 'Feedback');
  }

  return option;
};

export {
  informResult,
  formatRecipientModel,
  getRecipientDefaultBySubType,
  saveFileToBackEnd,
  getSubTypeByType,
  filterResult,
  filterFeedbackSubtype
};
