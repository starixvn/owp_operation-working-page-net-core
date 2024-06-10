import { useEffect, useState } from 'react';
import useStableDispatch from '../../customHook/useStableDispatch';
import Header from './component/Header';
import Subject from './component/Subject';
import {
  formatRecipientModel,
  getRecipientDefaultBySubType,
  getSubTypeByType,
  informResult,
  saveFileToBackEnd
} from '../Helper/HelperCreateTicket';
import Subtype from './component/Subtype';
import {
  STATUS,
  ticketType,
  subjectMaxLength,
  subtypeTicketCheck
} from './logWorkConst.json';
import Status from './component/Status';
import Recipient from './component/Recipient';
import Comment from './component/Comment';
import Attachments from '../Attachments/Attachments';
import ButtonGroup from './component/ButtonGroup';
import {
  getSubjectBySubtype,
  isErrorInput,
  isErrorMatchCheck,
  isMatchCheckType
} from './LogWorkHelper';
import { closePopupLogWork } from './../createTicketBoardSliceNew';
import { post } from '../../Service/Service';
import EventSubject from './component/EventSubject';
import { urlRequestLogWork } from '../const/const';
import {
  LogWorkProps,
  MatchInfo,
  StatusOption,
  SubType,
  SubTypeOption,
  TRecipient
} from './Type';

const LogWorkPopup = ({ creatorInfo }: LogWorkProps) => {
  const [subject, setSubject] = useState('');
  const [errorSubject, setErrorSubject] = useState('');
  const [subTypeOption, setSubTypeOption] = useState<SubTypeOption[]>([]);
  const [subTypeSelected, setSubTypeSelected] = useState<SubTypeOption>();
  const [statusSelected, setStatusSelected] = useState<StatusOption>(STATUS[1]);
  const [recipient, setRecipient] = useState<TRecipient[]>([]);
  const [comment, setComment] = useState('');
  const [selectedFiles, setSelectedFiles] = useState([]);
  const [errorRecipient, setErrorRecipient] = useState(false);
  const [adminSiteId, setAdminSiteId] = useState('');
  const [matchInfo, setMatchInfo] = useState<MatchInfo | null>(null);
  const [errorMatchCheck, setErrorMatchCheck] = useState('');
  const dispatch = useStableDispatch();

  useEffect(() => {
    (async () => {
      const subType: SubType[] = await getSubTypeByType(ticketType);
      const option = subType.map(
        ({ subTypeId: value, subTypeName: label }) => ({
          value,
          label
        })
      );
      const subTypeDefault = option.find(x => x.value === subtypeTicketCheck);
      const recipientDefault = (await getRecipientDefaultBySubType(
        subTypeDefault?.value
      )) as TRecipient[];
      setSubTypeOption(option);
      setSubTypeSelected(subTypeDefault);
      setRecipient(recipientDefault.concat([creatorInfo]));
      subTypeDefault && setSubject(subTypeDefault.label);
    })();
  }, []);

  const handleOnChangeSubtype = async (subtype: SubTypeOption | null) => {
    if (subtype) {
      setSubTypeSelected(subtype);
      const recipientBySubtype = (await getRecipientDefaultBySubType(
        subtype?.value
      )) as TRecipient[];
      setRecipient(recipientBySubtype.concat([creatorInfo]));
      setSubject(getSubjectBySubtype(subtype));
      setErrorSubject('');
      setErrorRecipient(false);
      setMatchInfo(null);
      setAdminSiteId('');
      setErrorMatchCheck('');
    }
  };

  const handleOnSubmit = async () => {
    if (isErrorInput(recipient, subTypeSelected, subject)) {
      !recipient.length && setErrorRecipient(true);
      !subject.length && setErrorSubject('This field is required');
      subject.length > subjectMaxLength &&
        setErrorSubject('The maximum of characters is 200. Please try again');
      return;
    }

    if (isErrorMatchCheck(subTypeSelected, adminSiteId)) {
      setErrorMatchCheck('Invalid MatchId');
      return;
    }

    const Recipients = formatRecipientModel(recipient, creatorInfo);
    const resultSaveFile =
      selectedFiles.length && (await saveFileToBackEnd(selectedFiles));
    const newAddModel = {
      Files: resultSaveFile || [],
      CreatorId: creatorInfo.recipientId,
      Recipients,
      Message: comment || 'Please add',
      SubType: subTypeSelected?.value,
      Status: statusSelected.value,
      SubTypeName: subTypeSelected?.label,
      TicketTypeName: 'S&VR',
      OwpLeagueName: matchInfo?.leagueName,
      OwpHomeName: matchInfo?.homeName,
      OwpAwayName: matchInfo?.awayName,
      EventDateTimeGmt8: matchInfo?.koTime,
      TicketType: ticketType,
      AdminSiteId: adminSiteId,
      Subject: subject,
      url: urlRequestLogWork
    };

    const result = await post(newAddModel);
    informResult(result);
    dispatch(closePopupLogWork(false));
  };

  return (
    <div className='chat-board-popup' id='create-ticket-other-popup'>
      <Header
        ticketHeader='S&VR'
        handleClosePopup={() => dispatch(closePopupLogWork(false))}
      />
      <div className='log-work'>
        <div className='subtype-status'>
          <Subtype
            options={subTypeOption}
            handleChangeSubType={handleOnChangeSubtype}
            selected={subTypeSelected}
          />
          <Status
            options={STATUS}
            selected={statusSelected}
            handleOnChange={value => setStatusSelected(value as StatusOption)}
          />
        </div>

        {isMatchCheckType(subTypeSelected) ? (
          <EventSubject
            errorInvalid={errorMatchCheck}
            setErrorInvalid={setErrorMatchCheck}
            setMatchId={setAdminSiteId}
            matchInfo={matchInfo}
            setMatchInfo={setMatchInfo}
          />
        ) : (
          <Subject
            errorValue={errorSubject}
            subjectValue={subject}
            handleOnChangeSubject={value => {
              setSubject(value);
              setErrorSubject('');
            }}
          />
        )}
        <Recipient
          isErrorRecipient={errorRecipient}
          handleOnErrorSubmit={setErrorRecipient}
          handleOnChange={setRecipient}
          selectedRecipients={recipient}
        />
        <Comment comment={comment} handleOnChange={setComment} />
        <Attachments
          selectedFiles={selectedFiles}
          setSelectedFiles={setSelectedFiles}
        />
        <ButtonGroup
          handleOnClosePopup={() => dispatch(closePopupLogWork(false))}
          handleSubmit={handleOnSubmit}
        />
      </div>
    </div>
  );
};

export default LogWorkPopup;
