import { RefObject } from 'react';
import { UncontrolledPopover, PopoverBody } from 'reactstrap';
import { AssignmentInfo } from '../type/TableType';
import DeskPhoneIcon from 'mdi-react/DeskphoneIcon';
import SkypeIcon from 'mdi-react/SkypeIcon';
import CellPhoneIcon from 'mdi-react/CellphoneIphoneIcon';

type Props = {
  target: RefObject<HTMLAnchorElement>;
  assignment: AssignmentInfo;
};

const ContactInfo = ({ target, assignment }: Props) => {
  const { Skype, Phone, Extension } = assignment;
  const renderSkypeInfo = (skype: string) => (
    <li>
      <a className='link-skype' href={`skype:${skype}?chat`}>
        <SkypeIcon className='icn-skype' />
        <span className='skype-value'> {skype}</span>
      </a>
    </li>
  );

  const renderExtension = (extension: string) => (
    <li>
      <a>
        <DeskPhoneIcon className='icn-deskphone' />
        <span className='extension-value'>{extension}</span>
      </a>
    </li>
  );

  const renderPhoneNumber = (phone: string) => (
    <li>
      <a>
        <CellPhoneIcon className='icn-cellphone' />
        <span className='phone-value'>{phone}</span>
      </a>
    </li>
  );

  return (
    <UncontrolledPopover
      className='popup-contact'
      trigger='legacy'
      placement='right'
      target={target}>
      <PopoverBody>
        <ul className='menu'>
          {Skype && renderSkypeInfo(Skype)}
          {Extension && renderExtension(Extension)}
          {Phone && renderPhoneNumber(Phone)}
        </ul>
      </PopoverBody>
    </UncontrolledPopover>
  );
};
export default ContactInfo;
