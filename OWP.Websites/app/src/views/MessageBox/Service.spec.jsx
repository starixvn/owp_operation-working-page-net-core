import axios from 'axios';
import MessageBoxService from './Service';

jest.mock('axios');

describe('MessageBoxService test', () => {
  beforeEach(() => {
    axios.post.mockReset();
    axios.put.mockReset();
  });

  it('return data when getAll call', async () => {
    // Arrange
    const responseData = { data: 'myData' };
    axios.post.mockResolvedValue(responseData);
    const model = {
      PageNumber: 1,
      PageSize: 10,
      Keyword: ''
    };
    // Act
    const data = await MessageBoxService.searchAll(model);
    // Assert
    expect.assertions(3);
    expect(data).toEqual(responseData.data);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith('/api/message_management/search', model);
  });

  it('return data when update_read_status call', async () => {
    // Arrange

    const responseData = { data: 'myData' };
    axios.put.mockResolvedValue(responseData);
    const model = {
      MessageId: 1
    };
    // Act
    const data = await MessageBoxService.updateStatus(model);
    // Assert

    expect(data).toEqual(responseData.data);
    expect(axios.put).toBeCalledTimes(1);
  });
});
