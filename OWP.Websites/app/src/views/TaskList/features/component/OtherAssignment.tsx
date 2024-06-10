import { createRef } from 'react';
import { TOtherAssignment } from '../../type/TTaskListTable';
import { getNoteOrUserName } from '../utils/utils';
import ContactInfo from './ContactInfo';
import parse from 'html-react-parser';
const OtherAssignment = (props: TOtherAssignment) => {
  const { assignment } = props;

  return (
    <>
      {assignment.map((x, index) => {
        const refOther = createRef<HTMLSpanElement>();
        const refContact = createRef<HTMLAnchorElement>();
        return !x.IsCurrentUser && x.IsHasContactInfo ? (
          <p key={`contact-${x.UserId}-${index}`}>
            <span ref={refOther} className='assignee-other'>
              <a
                id={`contact-info-${x.UserId}`}
                ref={refContact}
                className='dropdown-button js-traderName'>
                {parse(
                  getNoteOrUserName(
                    x.AssignmentNote,
                    x.IsCurrentUser,
                    x.Username
                  )
                )}
              </a>
              <ContactInfo target={refContact} assignment={x} />
            </span>
          </p>
        ) : (
          <p key={`contact-${x.UserId}-${index}`}>
            <span ref={refOther} className='assignee-other'>
              {parse(
                getNoteOrUserName(x.AssignmentNote, x.IsCurrentUser, x.Username)
              )}
            </span>
          </p>
        );
      })}
    </>
  );
};

export default OtherAssignment;
