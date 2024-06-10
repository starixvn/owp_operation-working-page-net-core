const prepareValueCopy = (dataRow, key) =>
  key.includes('TakeOver')
    ? {
        MainAssignee1: dataRow.HDPTakeOver,
        MainAssignee2: dataRow.OUTakeOver,
        OtherAssignee1: dataRow.OtherHDPTakeOver,
        OtherAssignee2: dataRow.OtherOUTakeOver
      }
    : {
        MainAssignee1: dataRow.HDP,
        MainAssignee2: dataRow.OU,
        OtherAssignee1: dataRow.OtherHDP,
        OtherAssignee2: dataRow.OtherOU
      };

const isTakeOver = id => id.includes('TakeOver');

const notificationContent = messageContent => (
  <div className='toast-content'>
    <div
      className='message-content'
      dangerouslySetInnerHTML={{ __html: messageContent }}
    />
  </div>
);

export { prepareValueCopy, isTakeOver, notificationContent };
