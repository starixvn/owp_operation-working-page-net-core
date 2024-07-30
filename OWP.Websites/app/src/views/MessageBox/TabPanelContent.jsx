import { Component } from 'react';
import PropTypes from 'prop-types';
import Loading from '../components/Loading';
import Search from './Search';
import MessageItem from './MessageItem';
import Service from './Service';
import Pagination from '../Shared/Paging/Pagination';
import { pageSizeOptions } from '../Shared/Constants';

const propTypes = {
  tabId: PropTypes.string,
  fetchOnLoad: PropTypes.string,
  isWatcher: PropTypes.bool
};

class TabPanelContent extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isLoading: true,
      searchText: '',
      activeTab: '1',
      currentMessages: [],
      currentPage: null,
      pageSizeOption: pageSizeOptions[0]
    };
  }

  componentDidMount() {
    const { tabId, fetchOnLoad } = this.props;

    this.setState({ isLoading: true });
    if (tabId === fetchOnLoad) {
      this.setState({ currentPage: 1, isLoading: true }, () => {
        this.searchAll();
      });
    }
  }

  componentDidUpdate(prevProps) {
    if (
      prevProps.fetchOnLoad !== this.props.fetchOnLoad &&
      this.props.tabId === this.props.fetchOnLoad
    ) {
      // eslint-disable-next-line no-warning-comments
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ currentPage: 1, isLoading: true }, () => {
        this.searchAll();
      });
    }
  }

  onPageSizeChange = pageSizeOption => {
    this.setState(
      { currentPage: 1, pageSizeOption, isLoading: true },
      () => {
        this.searchAll();
      }
    );
  };

  onPageChanged = data => {
    const { currentPage } = data;
    this.setState({ isLoading: true, currentPage }, () => {
      this.searchAll();
    });
  };

  submitFilter(value) {
    this.setState(
      { isLoading: true, searchText: value, currentPage: 1 },
      () => {
        this.searchAll();
      }
    );
  }

  searchAll() {
    (async () => {
      const { pageSizeOption, searchText, currentPage } = this.state;
      const response = await Service.searchAll({
        PageNumber: currentPage,
        PageSize: pageSizeOption.value,
        Keyword: searchText,
        IsWatcher: this.props.isWatcher
      });

      const currentMessages = response.Messages;
      this.setState({ currentPage, currentMessages, isLoading: false });
    })();
  }

  onClickMessageItem(evt) {
    if (evt) {
      (async () => {
        const messageId = evt.MessageId;
        await Service.updateStatus(messageId);
      })();
    }
  }

  render() {
    const { isLoading, currentMessages, currentPage } = this.state;
    return (
      <>
        <Search submit={evt => this.submitFilter(evt)} />
        {isLoading && <Loading />}
        <>
          <div className='message-list'>
            {currentMessages.map((message, index) => (
              <MessageItem
                key={`${index}_${message.MessageId}`}
                message={message}
                onClick={evt => this.onClickMessageItem(evt)}
              />
            ))}
          </div>
          <div className='paging-group'>
            <div className='pagination'>
              <Pagination
                currentPage={currentPage}
                onPageChanged={this.onPageChanged}
                onPageSizeChange={this.onPageSizeChange}
                pageSizeOptions={pageSizeOptions}
              />
            </div>
          </div>
        </>
      </>
    );
  }
}
TabPanelContent.propTypes = propTypes;
export default TabPanelContent;
