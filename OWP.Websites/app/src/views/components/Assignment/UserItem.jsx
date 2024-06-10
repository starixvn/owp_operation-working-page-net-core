import { Component } from 'react';
import PropTypes from 'prop-types';
import { noop } from 'lodash';

const propTypes = {
  user: PropTypes.object.isRequired,
  highlightedDisplay: PropTypes.object,
  onClickItem: PropTypes.func,
  index: PropTypes.number
};

class UserItem extends Component {
  constructor(props) {
    super(props);
    this.onClickItem = this.onClickItem.bind(this);
  }

  onClickItem() {
    const { user } = this.props;
    this.props.onClickItem(user);
  }

  render() {
    const { user, highlightedDisplay, index } = this.props;
    return (
      <span
        data-testid="content-input"
        key={`${user.UserName}_UserItem_${index}`}>
        {highlightedDisplay || user.UserName}{' '}
        {user.NickName && `(${user.NickName})`}
      </span>
    );
  }
}
UserItem.propTypes = propTypes;
UserItem.defaultProps = {
  user: {},
  index: 0,
  highlightedDisplay: null,
  onClickItem: () => noop
};
export default UserItem;
