
import OtherIssueContent from '../../../../Shared/common/CustomTable/commonCell/commonContentNew/OtherIssueContent';

const OtherIssueCell = ({ subject, isRead }: { subject: string, isRead: boolean }) => {
  return (
    <span
      className={
        !isRead
          ? 'new-add-info-cell has-unread conversation-cell'
          : 'new-add-info-cell cell conversation-cell'
      }>
      <OtherIssueContent subject={subject} />
    </span>
  );
};

export default OtherIssueCell;
