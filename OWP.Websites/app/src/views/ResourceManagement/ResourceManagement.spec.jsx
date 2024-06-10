import { render, waitFor } from '@testing-library/react';
import ResourceManagement from './ResourceManagement';
import AuthorizeService from '../Authorize/Service';
import Service from './Service';
import Select from 'react-select';

jest.mock('../Authorize/Service');
jest.mock('./Service');
Select.displayName = 'Select';

describe('Render ResourceManagement', () => {
  const policy = { UserId: 8, Username: 'sa', HasRead: true, HasWrite: true };
  const data = [{ Id: 1, Value: '1', RefName: 'abc' }];

  it('ResourceManagement component should render <ResourceTable />', async () => {
    // Arrange
    AuthorizeService.authorize.mockResolvedValue(policy);
    Service.getByRefName.mockResolvedValue(data);
    // Act
    const { container } = render(<ResourceManagement />);

    // Assert
    await waitFor(() => {
      expect(
        container.querySelector('.resource-management')
      ).toBeInTheDocument();
    });
  });
});
