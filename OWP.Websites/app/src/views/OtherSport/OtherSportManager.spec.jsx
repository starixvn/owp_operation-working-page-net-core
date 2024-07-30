import { render, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import OtherSportManager from './OtherSportManager';
import getPermission from '../Shared/permission/getPermission';

jest.mock('../Shared/permission/getPermission');
describe('OtherSportManager under unit test', () => {
  beforeEach(() => {
    getPermission.mockReturnValue({
      HasWrite: true
    });
  });
  it('render loading othersport page', async () => {
    const { container } = render(<OtherSportManager />);
    waitFor(() => {
      expect(container.querySelector('.loading-process')).toBeInTheDocument();
    });
  });
});
