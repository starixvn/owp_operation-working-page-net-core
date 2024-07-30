import { Component } from 'react';
import PropTypes from 'prop-types';
import { split, join } from 'lodash';
import Utils from '../Shared/Utils';
import { AppContext } from '../Shared/Context';
import dayjs from 'dayjs';
import ContentCopyIcon from 'mdi-react/ContentCopyIcon';

const propTypes = {
  data: PropTypes.array.isRequired,
  keywords: PropTypes.string.isRequired,
  isNumeric: PropTypes.bool.isRequired,
  openLiveDetail: PropTypes.func.isRequired
};

const eventMouse = {
  duringMouseDown: 0,
  duringMouseUp: 0
};

class SearchResultItem extends Component {
  constructor(props) {
    super(props);
    this.state = {
      copied: -1
    };
    this.openLiveDetail = this.openLiveDetail.bind(this);
    this.handleEventMouse = this.handleEventMouse.bind(this);
    this.copyMatchInfo = this.copyMatchInfo.bind(this);
    this.renderArrayResult = this.renderArrayResult.bind(this);
    this.renderFormatItemResult = this.renderFormatItemResult.bind(this);
  }

  handleEventMouse = event => {
    if (event.type === 'mousedown') {
      eventMouse.duringMouseDown = new Date().getTime();
    }
    if (event.type === 'mouseup') {
      eventMouse.duringMouseUp = new Date().getTime();
    }
  };

  getMatchNoText = (isToday, matchNo) => {
    if (isToday && matchNo) {
      const searchKeywords = this.props.keywords;
      const highlightMatchNo = this.renderHighlightKeywords(
        searchKeywords,
        matchNo
      );
      return `Match No.<span class="default-highlight">${highlightMatchNo}</span>`;
    }

    return '';
  };

  renderHighlightKeywords = (keywords, text) => {
    let result = text;
    const textInLower = text.toLowerCase();
    const keywordInLower = keywords.toLowerCase();
    if (textInLower.includes(keywordInLower)) {
      const startIndexOfKeyword = textInLower.indexOf(keywordInLower);
      const keywordResultInText = text.substr(
        startIndexOfKeyword,
        keywords.length
      );
      result = join(
        split(text, keywordResultInText),
        `<span class='highlight'>${keywordResultInText}</span>`
      );
    }
    return result;
  };

  renderMatchInfo = item => {
    const searchKeywords = this.props.keywords;
    const isNumericKeywords = this.props.isNumeric;
    const Event = !isNumericKeywords
      ? this.renderHighlightKeywords(searchKeywords, item.Event)
      : item.Event;
    const eventElement = `<span class="default-highlight">${Event}</span>`;
    const formatEventDate = item.EventDate ? this.getTimeExceptZone(item.EventDate, 'hh:mm A') : '';
    const eventDate = `<span class="default-highlight">${formatEventDate}</span>`;
    return `${this.getMatchNoText(
      item.IsToday,
      item.MatchNo
    )} K.O ${eventDate} ${item.LeagueName}</></br>${eventElement}`;
  };

  renderAssignmentInfo(info) {
    const searchKeywords = this.props.keywords;
    const isNumericKeywords = this.props.isNumeric;
    const html = !isNumericKeywords
      ? this.renderHighlightKeywords(
        searchKeywords,
        info.AssigneesNameConjunction
      )
      : info.AssigneesNameConjunction;
    return (
      <span className='assignee-item'>
        <span className='position'>
          <span>{info.PositionName}:</span>
        </span>
        <span className='name' dangerouslySetInnerHTML={{ __html: html }} />
      </span>
    );
  }

  renderNoResults() {
    const searchKeywords = this.props.keywords;
    const isNumeric = this.props.isNumeric;
    if (isNumeric || searchKeywords.length >= 2) {
      return (
        <li className='no-results'>
          <i className='mdi mdi-emoticon-sad icon' />
          No results found
        </li>
      );
    }

    return null;
  }

  renderAssigneSeparator(isHasAssignee) {
    return isHasAssignee ? <span className='assignee-separator'> </span> : null;
  }

  openLiveDetail = item => {
    if (eventMouse.duringMouseUp - eventMouse.duringMouseDown > 200) {
      return;
    }
    const { detailInformationPolicy } = this.context;
    if (detailInformationPolicy.HasRead && item.IsAllowedToViewDetail) {
      const url =
        Utils.resolveUrl('/EventManagement/LiveDetail/Index/') +
        item.OWPMatchId;
      this.props.openLiveDetail(url);
    }
  };

  allAssigneeInString = data => {
    let assignee = '';

    if (data.MainTrader) {
      assignee += `${data.MainTrader.PositionName}:${data.MainTrader.AssigneesNameConjunction}`;
    }

    if (data.OtherTrader) {
      assignee += `, ${data.OtherTrader.PositionName}:${data.OtherTrader.AssigneesNameConjunction}`;
    }

    if (data.MainLA) {
      assignee += `${data.MainTrader || data.OtherTrader ? ', ' : ''}${data.MainLA.PositionName
        }:${data.MainLA.AssigneesNameConjunction}`;
    }

    if (data.OtherLA) {
      assignee += `, ${data.OtherLA.PositionName}:${data.OtherLA.AssigneesNameConjunction}`;
    }

    return assignee;
  };

  copyMatchInfo = matchInfo => {
    const {
      EventDate,
      LeagueName,
      Event,
      AdminSiteId,
      IPTVCopy,
      Provider
    } = matchInfo;

    const eventKOTime = EventDate ? this.getTimeExceptZone(EventDate, 'hh:mm A') : '';
    const koTime = `K.O Time: ${eventKOTime}`;
    const league = `League: ${LeagueName}`;
    const match = `Match: ${Event}`;
    const matchId = `Match ID: ${AdminSiteId ?? ''}`;
    const iptv = `TV/IPTV: ${IPTVCopy ?? ''} `;
    const provider = `Provider: ${Provider ?? ''}`;
    window.executeCopy(koTime, league, match, matchId, iptv, provider);
  };

  renderFormatItemResult(index, className, eventDate, item) {
    return (
      <li key={index}>
        <a
          className={className}
          onMouseUp={this.handleEventMouse}
          onMouseDown={this.handleEventMouse}
          onClick={() => this.openLiveDetail(item)}>
          <time className='default-highlight'>{eventDate}</time>
          <div>
            <p
              className='event-info'
              dangerouslySetInnerHTML={{ __html: this.renderMatchInfo(item) }}
            />
            {item.IsHasAssignee && (
              <p className='assignee'>
                {item.MainTrader && this.renderAssignmentInfo(item.MainTrader)}
                {item.OtherTrader &&
                  this.renderAssignmentInfo(item.OtherTrader)}
                {item.MainLA && this.renderAssignmentInfo(item.MainLA)}
                {item.OtherLA && this.renderAssignmentInfo(item.OtherLA)}
              </p>
            )}
          </div>
        </a>
        <ContentCopyIcon onClick={() => this.copyMatchInfo(item, index)} />
      </li>
    );
  }

  renderArrayResult(searchResults, detailInformationPolicy) {
    return searchResults.map((item, index) => {
      const canViewDetail =
        detailInformationPolicy.HasRead && item.IsAllowedToViewDetail;
      const className = `cd-nowrap${canViewDetail ? ' pointer' : ' cursordefault'
        }`;
      const eventDate = item.EventDate ? this.getTimeExceptZone(item.EventDate, 'MMM DD, YYYY') : '';
      const eventKOTime = item.EventDate ? this.getTimeExceptZone(item.EventDate, 'hh:mm A') : '';
      const matchInfo = `${eventDate}\n${item.IsToday && item.MatchNo ? `Match No.${item.MatchNo}\t` : ''
        }K.O ${eventKOTime}\t${item.LeagueName}\n${item.Event}${item.IsHasAssignee ? `\n${this.allAssigneeInString(item)}` : ''
        }`;
      return this.renderFormatItemResult(
        index,
        className,
        eventDate,
        item,
        matchInfo
      );
    });
  }

  getTimeExceptZone(date, format) {
    return dayjs(date.substring(0, date.length - 6)).format(format);
  }

  render() {
    const searchResults = this.props.data;
    const { detailInformationPolicy } = this.context;
    return searchResults.length > 0
      ? this.renderArrayResult(searchResults, detailInformationPolicy)
      : this.renderNoResults();
  }
}

SearchResultItem.propTypes = propTypes;
SearchResultItem.contextType = AppContext;
export default SearchResultItem;
