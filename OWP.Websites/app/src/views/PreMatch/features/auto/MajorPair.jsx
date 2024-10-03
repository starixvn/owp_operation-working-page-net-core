import React from 'react';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';
import Assignment from './Assignment';
import InputAssignment from './InputAssignment';
import { ASSIGNMENT_TYPE, BETTYPE } from '../../const/const';


const MajorPair = () => {
  const dispatch = useDispatch();
  const assignee = useSelector(state => state.auto.pairs.major);
  const assigneeTakeOver = useSelector(state => state.auto.pairs.majorTakeOver);
  const findAssignmentByBettypeAndType=(bettype, assignmentType)=> assignee.find(a=>a.bettype===bettype && a.assignmentType===assignmentType);
 
  const [hdp,hdpOther, ou, ouOther]= [
    findAssignmentByBettypeAndType(BETTYPE.HDP, ASSIGNMENT_TYPE.MAIN),
    findAssignmentByBettypeAndType(BETTYPE.HDP, ASSIGNMENT_TYPE.OTHER),
    findAssignmentByBettypeAndType(BETTYPE.OU, ASSIGNMENT_TYPE.MAIN),
    findAssignmentByBettypeAndType(BETTYPE.OU, ASSIGNMENT_TYPE.OTHER)];
    const [hdpTakeOver,hdpOtherTakeOver, ouTakeOver, ouOtherTakeOver]= [
      findAssignmentByBettypeAndType(BETTYPE.HDP, ASSIGNMENT_TYPE.MAIN),
      findAssignmentByBettypeAndType(BETTYPE.HDP, ASSIGNMENT_TYPE.OTHER),
      findAssignmentByBettypeAndType(BETTYPE.OU, ASSIGNMENT_TYPE.MAIN),
      findAssignmentByBettypeAndType(BETTYPE.OU, ASSIGNMENT_TYPE.OTHER)];
  
       
  const handleChangeAssignment=()=>{

  }
  return (
    <div className='major__pair--container wrapper'>
        <div className='major__pair'>
          <label className='title'>Major Pair</label>
          <Assignment
            assignmentHdp={<>
            <InputAssignment onChange={handleChangeAssignment} userId={hdp.userId} userName={hdp.userName}/>
            <InputAssignment onChange={handleChangeAssignment} userId={hdpOther.userId} userName={hdpOther.userName}/>
            </>}
            assignmentOu={<>
            <InputAssignment  onChange={handleChangeAssignment} userId={ou.userId} userName={ou.userName}/>
            <InputAssignment onChange={handleChangeAssignment} userId={ouOther.userId} userName={ouOther.userName}/>
            </>
          }/>
        </div>
          <div className='major__pair--take-over'>
          <label className='title'>Major Pair (Take Over)</label>
          <Assignment
            assignmentHdp={<>
            <InputAssignment onChange={handleChangeAssignment} userId={hdpTakeOver.userId} userName={hdpTakeOver.userName}/>
            <InputAssignment onChange={handleChangeAssignment} userId={ouTakeOver.userId} userName={ouTakeOver.userName}/>
            </>}
            assignmentOu={<>
            <InputAssignment  onChange={handleChangeAssignment} userId={hdpOtherTakeOver.userId} userName={hdpOtherTakeOver.userName}/>
            <InputAssignment onChange={handleChangeAssignment} userId={ouOtherTakeOver.userId} userName={ouOtherTakeOver.userName}/>
            </>
          }/>
          </div>
        </div>
      );
};

MajorPair.propTypes = {
  
};

export default MajorPair;