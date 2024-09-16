import React from 'react';
import IconEmoji from '../../../Shared/IconCustom/IconEmoji';
import PropTypes from 'prop-types';

const EmojiDisplay = ({
  messageReactIcons = [],
  ticketId,
  isConsolidationView
}) => {
  const result = groupByIconKey(messageReactIcons);

  return (
    <>
      {result.map((item, i) => (
        <IconEmoji
          key={i}
          iconKey={item.iconKey}
          userReact={item.userReact}
          messageId={messageReactIcons[0].messageId}
          ticketId={ticketId}
          isConsolidationView={isConsolidationView}
        />
      ))}
    </>
  );
};

const groupByIconKey = icons => {
  const grouped = icons.reduce((acc, icon) => {
    if (!acc[icon.iconKey]) {
      acc[icon.iconKey] = {
        messageId: icon.messageId,
        iconKey: icon.iconKey,
        userReact: []
      };
    }

    acc[icon.iconKey].userReact.push({
      userId: icon.reactUserId,
      userName: icon.reactUserName,
      avatarLink: icon.reactAvatarName
    });

    return acc;
  }, {});

  return Object.values(grouped);
};

EmojiDisplay.propTypes = {
  messageReactIcons: PropTypes.array
};

export default React.memo(EmojiDisplay);
