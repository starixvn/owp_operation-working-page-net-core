import axios from 'axios';

const netCoreURLTicKetCenter = `/api/TicketCenter`;

const transformFunctionResponse = response =>
  response.data.map(data => ({
    value: data.functionId,
    label: data.functionName
  }));

const queryFunction = async () => {
  const url = `${netCoreURLTicKetCenter}/get-function-feedback`;
  const response = await axios.get(url);
  return transformFunctionResponse(response);
};

const queryUserFeedbackInfo = async () => {
  const url = `${netCoreURLTicKetCenter}/get-user-feedback-info`;
  const response = await axios.get(url);
  return response.data;
};

export { queryFunction, queryUserFeedbackInfo };
