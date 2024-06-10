import { createRef } from 'react';
import { AssignmentInfo } from '../type/TableType';
import ContactInfo from './ContactInfo';

const Assignment = (props: {
  assignment: AssignmentInfo[];
  userNameFilter: string;
}) => {
  return (
    <>
      {props.assignment.map(x => {
        const ref = createRef<HTMLAnchorElement>();
        return !x.IsCurrentUser && x.IsHasContactInfo ? (
          <span key={`contact-${x.UserId}`} className='assignee'>
            <a
              ref={ref}
              className={`${
                x.Username?.toLowerCase() ===
                props.userNameFilter?.toLowerCase()
                  ? 'highlighted dropdown-button js-traderName'
                  : 'dropdown-button js-traderName'
              }`}>
              {x.Username}
            </a>
            <ContactInfo target={ref} assignment={x} />
          </span>
        ) : (
          <span
            key={`trader-${x.UserId}`}
            className={`${
              x.IsCurrentUser ? 'badge-label my-assign' : 'assignee'
            }  `}>
            <span
              className={`${
                x.Username?.toLowerCase() ===
                props.userNameFilter?.toLowerCase()
                  ? 'highlighted'
                  : ''
              }`}>
              {x.Username}
            </span>
          </span>
        );
      })}
    </>
  );
};

export default Assignment;
