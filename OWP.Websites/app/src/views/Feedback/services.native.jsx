import Axios from 'axios';
import { baseApiUrl } from '../Shared/Constants';

const netCoreURLTicKetCenter = `${baseApiUrl}/api/TicketCenter`;

const transformFunctionResponse = response =>
  response.data.map(data => ({
    value: data.functionId,
    label: data.functionName
  }));

const queryFunction = async () => {
  const url = `${netCoreURLTicKetCenter}/get-function-feedback`;
  const response = await Axios.get(url);
  return transformFunctionResponse(response);
};

const queryUserFeedbackInfo = async () => {
  const url = `${netCoreURLTicKetCenter}/get-user-feedback-info`;
  const response = await Axios.get(url);
  return response.data;
};

export { queryFunction, queryUserFeedbackInfo };
