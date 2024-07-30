import { createRef } from 'react';
import { AssignmentInfo } from '../type/TableType';
import ContactInfo from './ContactInfo';
import { UncontrolledTooltip } from 'reactstrap';
import parse from 'html-react-parser';

const OtherAssignment = (props: {
  assignment: AssignmentInfo[];
  userNameFilter: string;
}) => {
  const { assignment } = props;
  const getNoteOrUserName = (
    note: string | null,
    isCurrentUser: boolean,
    userName: string,
    userNameFilter: string
  ) => {
    const userNameText = () =>
      `<span className=${isCurrentUser ? 'badge-label my-assign' : 'assignee'}>
        <span className=${
          userName?.toLowerCase() === userNameFilter?.toLowerCase()
            ? 'highlighted'
            : ''
        }>
        ${userName}
        </span>
    </span>`;
    return note ? note.replace('#<user>', userNameText()) : '';
  };
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
                    x.Username,
                    props.userNameFilter
                  )
                )}
              </a>
              <ContactInfo target={refContact} assignment={x} />
            </span>
            <UncontrolledTooltip target={refOther}>
              {parse(
                getNoteOrUserName(
                  x.AssignmentNote,
                  x.IsCurrentUser,
                  x.Username,
                  props.userNameFilter
                )
              )}
            </UncontrolledTooltip>
          </p>
        ) : (
          <p key={`contact-${x.UserId}-${index}`}>
            <span ref={refOther} className='assignee-other'>
              {parse(
                getNoteOrUserName(
                  x.AssignmentNote,
                  x.IsCurrentUser,
                  x.Username,
                  props.userNameFilter
                )
              )}
            </span>
            <UncontrolledTooltip target={refOther}>
              {parse(
                getNoteOrUserName(
                  x.AssignmentNote,
                  x.IsCurrentUser,
                  x.Username,
                  props.userNameFilter
                )
              )}
            </UncontrolledTooltip>
          </p>
        );
      })}
    </>
  );
};

export default OtherAssignment;
