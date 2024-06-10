import { render, waitFor } from '@testing-library/react';
import axios from 'axios';
import LastUpdate from './LastUpdate';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { buildLastUpdateUrl } from '../../utils';

jest.mock('axios');

describe('LastUpdate component', () => {
  it('should render last update info', async () => {
    const response = {
      data: { date: new Date('2022-05-06T00:00:00'), username: 'liam' }
    };
    axios.get.mockResolvedValue(response);

    const { getByText } = render(<LastUpdate />);
    await waitFor(() =>
      expect(axios.get).toHaveBeenCalledWith(buildLastUpdateUrl())
    );

    expect(
      getByText('Last Updated at May 6, 2022 12:00 AM by liam')
    ).toBeInTheDocument();
  });

  it('should not render last update info when response is empty', async () => {
    axios.get.mockResolvedValue({ data: {} });

    const { container } = render(<LastUpdate />);
    await waitFor(() =>
      expect(axios.get).toHaveBeenCalledWith(buildLastUpdateUrl())
    );
  });
});
