import ToastrWrapper from '../../../components/ToastrWrapper';
import { GroupDataSource, RecipientModel } from '../Type';
import {
  buildGetRecipientGroupsURL,
  buildSaveRecipientGroupsURL,
  validateGroupName,
  validateRecipients
} from './groupHelper';

jest.mock('../../../components/ToastrWrapper');
ToastrWrapper.showError = jest.fn();

const setUpPathname = () => {
  global = Object.create(window);
  Object.defineProperty(window, 'netCoreRootUrl', {
    value: 'v1/test'
  });
};

describe('groupHelper under test', () => {
  afterEach(() => {
    delete process.env.NODE_ENV;
    jest.clearAllMocks();
  });

  it('method validateGroupName, groupName is Empty, should call toast', () => {
    validateGroupName(1, '', []);
    expect(ToastrWrapper.showError).toHaveBeenCalled();
  });

  it('method validateGroupName, groupName is exist, should call toast', () => {
    const groupDataSource = [
      {
        groupId: 1,
        groupName: 'LST'
      }
    ] as GroupDataSource[];
    validateGroupName(3, 'LST', groupDataSource);
    expect(ToastrWrapper.showError).toHaveBeenCalled();
  });

  it('method validateGroupName, groupName exceeds 50 characters, should call toast', () => {
    const groupName =
      'The sample project we will look at is a small counter application that lets us add or subtract from a number as we click buttons. It may not be very exciting, but it shows all the important pieces of a React+Redux application in action.The project has been created using the official Redux template for Create-React-App. Out of the box, it has already been configured with a standard Redux application structure, using Redux Toolkit to create the Redux store and logic, and React-Redux to connect together the Redux store and the React components.';
    validateGroupName(1, groupName, []);
    expect(ToastrWrapper.showError).toHaveBeenCalled();
  });

  it('method validateGroupName, groupName is valid, should not call toast', () => {
    const groupName = 'Mira';
    validateGroupName(1, groupName, []);
    expect(ToastrWrapper.showError).toHaveBeenCalledTimes(0);
  });

  it('method validateRecipients, recipients is [], should call toast', () => {
    validateRecipients([]);
    expect(ToastrWrapper.showError).toHaveBeenCalled();
  });

  it('method validateRecipients, recipients is valid, should not call toast', () => {
    const recipients = [{ recipientId: 1 }] as RecipientModel[];
    validateRecipients(recipients);
    expect(ToastrWrapper.showError).toHaveBeenCalledTimes(0);
  });

  it('method buildGetRecipientGroupsURL, should return dev url', () => {
    process.env.NODE_ENV = 'development';
    const devURL = buildGetRecipientGroupsURL();
    expect(devURL).toEqual('/api/RecipientGroup/get-recipient-group');
  });

  it('method buildGetRecipientGroupsURL, should return window url', () => {
    process.env.NODE_ENV = 'test';
    setUpPathname();
    const devURL = buildSaveRecipientGroupsURL();
    expect(devURL).toEqual('/api/RecipientGroup/save-recipient-group');
  });
});
