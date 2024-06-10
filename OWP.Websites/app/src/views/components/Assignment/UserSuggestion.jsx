import Autosuggest from 'react-autosuggest';
import { Component } from 'react';
import UserService from '../../Shared/User/UserService';
import PropTypes from 'prop-types';
import ToastrWrapper from '../ToastrWrapper';

const renderSuggestion = suggestion => {
  return (
    <div>
      {`${suggestion.UserName} ${
        suggestion.NickName ? `(${suggestion.NickName})` : ''
      }`}
    </div>
  );
};

export class UserSuggestion extends Component {
  constructor() {
    super();
    this.state = {
      value: '',
      suggestions: [],
      userIsvalid: true
    };
    this.getSuggestionValue = this.getSuggestionValue.bind(this);
    this.onChange = this.onChange.bind(this);
    this.onBlur = this.onBlur.bind(this);
    this.onSuggestionsFetchRequested = this.onSuggestionsFetchRequested.bind(
      this
    );
    this.onSuggestionsClearRequested = this.onSuggestionsClearRequested.bind(
      this
    );
    this.onKeyPress = this.onKeyPress.bind(this);
  }

  getSuggestionValue = suggestion => {
    this.props.onHandleChange(suggestion.UserName, true);
    return suggestion.UserName;
  };

  onChange = (_, { newValue }) => {
    let userNameIsValid = false;
    let userName = '';
    this.setState({ value: newValue }, () => {
      if (newValue === '') {
        userNameIsValid = true;
        userName = newValue;
      } else {
        const user = UserService.getInfoByUserName(newValue);
        if (user) {
          userNameIsValid = true;
          userName = user.UserName;
        }
      }

      this.props.onHandleChange(userName, userNameIsValid);
    });
  };

  onBlur = () => {
    if (this.props.exactly) {
      const { value } = this.state;
      if (value) {
        const user = UserService.getInfoByUserName(value);
        if (!user) {
          ToastrWrapper.showError('User not found');
          this.setState({ userIsvalid: false });
        } else {
          this.setState({ value: user.UserName, userIsvalid: true });
        }
      }
    }
  };

  onSuggestionsFetchRequested = ({ value }) => {
    if (
      value &&
      value.trim() &&
      value.length >= this.props.minLengthToSuggest
    ) {
      const response = UserService.getSuggestion(value);
      this.setState({
        suggestions: response
      });
    }
  };

  onSuggestionsClearRequested = () => {
    this.setState({
      suggestions: []
    });
  };

  onKeyPress = event => {
    if (event.key === 'Enter') {
      this.onBlur();
    }
  };

  render() {
    const { value, suggestions } = this.state;
    const inputProps = {
      placeholder: 'Username or nickname',
      value,
      onChange: this.onChange,
      onBlur: this.onBlur,
      onKeyPress: this.onKeyPress,
      'data-testid': 'content-input'
    };

    return (
      <Autosuggest
        highlightFirstSuggestion
        suggestions={suggestions}
        onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
        onSuggestionsClearRequested={this.onSuggestionsClearRequested}
        getSuggestionValue={this.getSuggestionValue}
        renderSuggestion={renderSuggestion}
        inputProps={inputProps}
        focusInputOnSuggestionClick
      />
    );
  }
}

UserSuggestion.propTypes = {
  onHandleChange: PropTypes.func,
  minLengthToSuggest: PropTypes.number,
  exactly: PropTypes.bool
};

UserSuggestion.defaultProps = {
  onHandleChange: f => f,
  minLengthToSuggest: 2,
  exactly: true
};
