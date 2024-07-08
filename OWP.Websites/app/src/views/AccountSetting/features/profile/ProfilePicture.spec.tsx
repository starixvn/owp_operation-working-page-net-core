import React from 'react';
import { render, screen, fireEvent, waitFor, queryAllByRole } from '@testing-library/react';
import '@testing-library/jest-dom';
import axios from 'axios';
import ProfilePicture from './ProfilePicture';
import { get, post } from '../../../Shared/Service/Service';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { urlGetProfileAvatar, urlUploadFile, urlSaveProfileAvatar } from '../../const/const';

// Mocking external dependencies
jest.mock('axios');
jest.mock('../../../Shared/Service/Service', () => ({
  get: jest.fn(),
  post: jest.fn(),
}));
jest.mock('../../../components/ToastrWrapper', () => ({
  showError: jest.fn(),
}));

describe('ProfilePicture Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  test('renders correctly and displays the profile picture', async () => {
    const mockData = 'mock-avatar.jpg';
    (get as jest.Mock).mockResolvedValue({ data: mockData });

    render(<ProfilePicture />);

    await waitFor(() => {
      expect(screen.getByText('Profile Picture')).toBeInTheDocument();
      // Check if AvatarDisplay is being rendered with the correct props
      expect(screen.getByAltText('Avatar')).toBeInTheDocument(); // Assuming AvatarDisplay has alt text 'Avatar'
    });
  });

  test('handles file upload correctly', async () => {
    const mockFile = new File(['dummy content'], 'image.jpg', { type: 'image/jpeg' });
    const mockUploadResponse = { data: 'file-id' };
    const mockPostResponse = {};
    (axios.post as jest.Mock).mockResolvedValue(mockUploadResponse);
    (post as jest.Mock).mockResolvedValue(mockPostResponse);
    (get as jest.Mock).mockResolvedValue({ data: 'mock-avatar.jpg' });

    render(<ProfilePicture />);

    // Simulate selecting a file
    const fileInput = screen.getByTestId('file-input') as HTMLInputElement;
    Object.defineProperty(fileInput, 'files', {
      value: [mockFile],
    });
    fireEvent.change(fileInput);

    // Simulate clicking the upload button to trigger the hidden file input
    const uploadButton = screen.getByText('Upload image');
    fireEvent.click(uploadButton);

    await waitFor(() => {
      expect(axios.post).toHaveBeenCalledWith(urlUploadFile, expect.any(FormData));
      expect(post).toHaveBeenCalledWith({
        url: urlSaveProfileAvatar,
        FileId: 'file-id',
      });
    });
  });

  test('shows an error if the file size exceeds 5MB', async () => {
    const largeFile = new File([new Array(6 * 1024 * 1024).fill('a').join('')], 'large-image.jpg', { type: 'image/jpeg' });
    (ToastrWrapper.showError as jest.Mock).mockImplementation(() => { });
    const mockPostResponse = {};
    const mockUploadResponse = { data: 'file-id' };
    (axios.post as jest.Mock).mockResolvedValue(mockUploadResponse);
    (post as jest.Mock).mockResolvedValue(mockPostResponse);
    (get as jest.Mock).mockResolvedValue({ data: 'mock-avatar.jpg' });

    render(<ProfilePicture />);
    const fileInput = screen.getByTestId('file-input') as HTMLInputElement;
    Object.defineProperty(fileInput, 'files', {
      value: [largeFile],
    });
    fireEvent.change(fileInput);

    const uploadButton = screen.getByText('Upload image');
    fireEvent.click(uploadButton);

    await waitFor(() => {
      expect(ToastrWrapper.showError).toHaveBeenCalledWith('The file size can not exceed 5 MB. Please try another one');
    });
  });
});
