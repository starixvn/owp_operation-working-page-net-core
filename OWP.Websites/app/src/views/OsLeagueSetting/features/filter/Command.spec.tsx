import { render, fireEvent } from '@testing-library/react';
import Command from './Command';
import useStore from '../../app/store';
import ToastrWrapper from '../../../components/ToastrWrapper';

jest.mock('../../../components/ToastrWrapper');

ToastrWrapper.showError = jest.fn();
const queryDataSource = jest.fn();
const setPopupType = jest.fn();

describe('<Command/> under test', () => {
  it('should not show button add new when hasWrite is false', () => {
    useStore.setState({
      policy: {
        Username: 'sa',
        HasRead: false,
        SiteId: 0,
        ActorId: 8,
        HasWrite: false
      }
    });
    const { container } = render(<Command />);
    expect(container.querySelector('.btn-add-league')).toBeNull();
  });

  it('should call queryDataSource when click search', () => {
    useStore.setState({
      queryDataSource,
      filter: {
        status: '1,2,3',
        round: '1,2,3',
        category: '1,2,3'
      },
      policy: {
        Username: 'sa',
        HasRead: false,
        SiteId: 0,
        ActorId: 8,
        HasWrite: true
      }
    });
    const { container } = render(<Command />);
    const buttonSearch = container.querySelector('.btn-search');
    buttonSearch && fireEvent.click(buttonSearch);
    expect(queryDataSource).toHaveBeenCalledTimes(1);
  });

  it('should call show error when click search', () => {
    useStore.setState({
      queryDataSource,
      filter: {
        status: '',
        round: '',
        category: ''
      },
      policy: {
        Username: 'sa',
        HasRead: false,
        SiteId: 0,
        ActorId: 8,
        HasWrite: true
      }
    });
    const { container } = render(<Command />);
    const buttonSearch = container.querySelector('.btn-search');
    buttonSearch && fireEvent.click(buttonSearch);
    expect(ToastrWrapper.showError).toHaveBeenCalledTimes(1);
  });

  it('should call setPopupFunction when click add', () => {
    useStore.setState({
      queryDataSource,
      filter: {
        status: '',
        round: '',
        category: ''
      },
      policy: {
        Username: 'sa',
        HasRead: false,
        SiteId: 0,
        ActorId: 8,
        HasWrite: true
      },
      setPopupType
    });
    const { container } = render(<Command />);
    const buttonAdd = container.querySelector('.btn-add-league');
    buttonAdd && fireEvent.click(buttonAdd);
    expect(setPopupType).toHaveBeenCalledTimes(1);
  });
});
