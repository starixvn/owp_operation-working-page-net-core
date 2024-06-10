import { Cell } from 'fixed-data-table-2';
import { post } from '../../../../../Shared/Service/Service';
import {
  buildUpdateProductUrl,
  buildPostModelWhenUpdateUserInfo
} from '../../../../utils';
import { dbColumnName } from '../../../../Const.json';
import PropTypes from 'prop-types';
import { TooltipText } from '../../../../../Shared/common/Tooltip';
import store from '../../../../app/storeNative';

const propTypes = {
  productInfo: PropTypes.object.isRequired
};

const RemarkCell = ({ productInfo }) => {
  const { assignmentRemark, userId, username } = productInfo;

  const { policy } = store.getState();
  const { HasWrite, ActorId, Username } = policy.permission;

  const handleUpdateRemark = async event => {
    const { value } = event.target;
    const url = buildUpdateProductUrl();
    const postModel = buildPostModelWhenUpdateUserInfo(
      userId,
      username,
      dbColumnName.assignmentRemark,
      value
    );
    const prop = {
      url,
      ...postModel,
      actorId: ActorId,
      actorName: Username
    };
    await post(prop);
  };

  const renderRemarkReadOnly = () => {
    return (
      assignmentRemark && (
        <TooltipText
          text={assignmentRemark}
          toolTipContent={assignmentRemark}
          innerClassName='provider'
          rowIndex={userId}
          placement='top-start'
        />
      )
    );
  };

  const renderRemarkCanEdit = (
    <input
      type='text'
      defaultValue={assignmentRemark}
      onBlur={handleUpdateRemark}
    />
  );

  const selectViewMode = hasWrite =>
    hasWrite ? renderRemarkCanEdit : renderRemarkReadOnly();
  return <Cell>{selectViewMode(HasWrite)}</Cell>;
};

RemarkCell.propTypes = propTypes;
export default RemarkCell;
