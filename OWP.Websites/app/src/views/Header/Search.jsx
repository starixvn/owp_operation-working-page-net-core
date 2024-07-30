import { createRef, Component } from 'react';
import QuickSearchService from '../QuickSearch/Service';
import SearchResultItem from './SearchResultItem';
import PropTypes from 'prop-types';
import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import Toastr from '../components/ToastrWrapper';
let delayRequestTimer;

const propTypes = {
  isVisible: PropTypes.bool,
  handleVisibleSearchArea: PropTypes.func
};

class Search extends Component {
  constructor(props) {
    super(props);

    this.state = {
      searchResults: [],
      keywords: '',
      isNumericKeywords: false,
      lastOWPMatchId: 0,
      isLoading: false,
      isOpen: false,
      liveDetailUrl: ''
    };

    this.searchInput = createRef();
    this.iframeRef = createRef();
    this.handleOnClickCloseSearchForm = this.handleOnClickCloseSearchForm.bind(
      this
    );
    this.handleOnInput = this.handleOnInput.bind(this);
    this.handleOnScroll = this.handleOnScroll.bind(this);
    this.openLiveDetail = this.openLiveDetail.bind(this);
  }

  handleOnClickCloseSearchForm() {
    this.props.handleVisibleSearchArea(false);
  }

  handleOnInput(event) {
    const keywords = event.target.value;
    clearTimeout(delayRequestTimer);
    delayRequestTimer = setTimeout(
      () => {
        this.getSearchResults(keywords, true);
      },
      300,
      keywords
    );
  }

  handleOnScroll(event) {
    const element = event.target;
    const isAtBottom =
      element.scrollHeight - element.scrollTop === element.clientHeight;
    if (isAtBottom && this.state.lastOWPMatchId > 0) {
      this.getSearchResults(this.state.keywords, false);
    }
  }

  getSearchResults(keywords, isReload) {
    const isNotEmptyKeywords = keywords.trim() !== '';
    const isNumeric = isNotEmptyKeywords && isNaN(keywords) === false;
    if (isNotEmptyKeywords && (isNumeric || keywords.length >= 2)) {
      let { searchResults, lastOWPMatchId } = this.state;

      if (isReload) {
        this.setState({
          ...this.state,
          isLoading: true
        });
        searchResults = [];
        lastOWPMatchId = 0;
      }

      const model = {
        SelectTop: 15,
        LastOWPMatchId: lastOWPMatchId,
        SearchKeywords: keywords,
        IsNumericKeywords: isNumeric
      };

      QuickSearchService.query(model).then(data => {
        const newLastOWPMatchId =
          data.length > 0 ? data[data.length - 1].OWPMatchId : 0;
        searchResults = searchResults.concat(data);

        this.setState({
          ...this.state,
          searchResults,
          keywords,
          isNumericKeywords: isNumeric,
          lastOWPMatchId: newLastOWPMatchId,
          isLoading: false
        });
      });
    } else {
      this.setState({
        ...this.state,
        searchResults: [],
        keywords,
        isNumericKeywords: isNumeric,
        lastOWPMatchId: 0,
        isLoading: false
      });
    }
  }

  componentDidUpdate() {
    setTimeout(() => {
      this.searchInput.current.focus();
    }, 100);
  }

  openLiveDetail = url => {
    this.setState({
      isOpen: true,
      liveDetailUrl: url
    });
  };

  closeLiveDetail = () => {
    const editGeneralInfo = this.iframeRef.current.getAttribute(
      'edit-general-info'
    );
    const editMatchInfo = this.iframeRef.current.getAttribute(
      'edit-match-info'
    );
    const editTradingInfo = this.iframeRef.current.getAttribute(
      'edit-trading-info'
    );
    const editProviderInfo = this.iframeRef.current.getAttribute(
      'edit-provider-info'
    );
    if (
      editGeneralInfo === '1' ||
      editMatchInfo === '1' ||
      editTradingInfo === '1' ||
      editProviderInfo === '1'
    ) {
      Toastr.confirm('Your unsaved changes will be lost. Are you sure?').then(
        confirmResult => {
          if (confirmResult.value) {
            this.setState({
              isOpen: false
            });
          }
        }
      );
    } else {
      this.setState({
        isOpen: false
      });
    }
  };

  render() {
    const {
      isOpen,
      liveDetailUrl,
      searchResults,
      isLoading,
      keywords,
      isNumericKeywords
    } = this.state;
    const searchAreaClass =
      'cd-main-search ' + (this.props.isVisible ? 'is-visible' : '');

    return (
      <div id='search' className={searchAreaClass}>
        <form
          onSubmit={e => {
            e.preventDefault();
          }}>
          <input
            id='txtSearchKeywords'
            type='search'
            placeholder='Search By Match Number or Home/Away'
            tabIndex='0'
            ref={this.searchInput}
            onInput={this.handleOnInput}
          />
        </form>
        <Modal
          tag='div'
          size='lg'
          isOpen={isOpen}
          className='modal-dark-title modal-edit-event'>
          <ModalHeader toggle={this.closeLiveDetail}>
            Detail Information
          </ModalHeader>
          <ModalBody tag='div'>
            <div className='masonry'>
              <iframe
                ref={this.iframeRef}
                style={{
                  overflow: 'hidden',
                  height: '100%',
                  width: '100%',
                  minHeight: '700px',
                  border: '0'
                }}
                src={liveDetailUrl}
              />
            </div>
          </ModalBody>
        </Modal>
        <div className='cd-search-suggestions'>
          <div className='news'>
            <ul id='ulSearchSuggestion' onScroll={this.handleOnScroll}>
              {isLoading ? (
                <li className='no-results'>Loading...</li>
              ) : (
                <SearchResultItem
                  data={searchResults}
                  keywords={keywords}
                  isNumeric={isNumericKeywords}
                  openLiveDetail={this.openLiveDetail}
                />
              )}
            </ul>
          </div>
        </div>
        <a
          href={null}
          className='close cd-text-replace'
          onClick={this.handleOnClickCloseSearchForm}>
          close
        </a>
      </div>
    );
  }
}

Search.propTypes = propTypes;
export default Search;
