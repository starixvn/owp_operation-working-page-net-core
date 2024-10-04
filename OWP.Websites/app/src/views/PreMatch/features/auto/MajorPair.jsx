import React from 'react';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';
import Assignment from './Assignment';
import InputAssignment from './InputAssignment';
import { ASSIGNMENT_TYPE, BETTYPE } from '../../const/const';
import { updateMajorPairs } from './autoSlice';
import { uniqueId } from 'lodash';


const MajorPair = () => {
  const dispatch = useDispatch();
  const assignee = useSelector(state => state.auto.pairs.major);
  
  const assigneeTakeOver = useSelector(state => state.auto.pairs.majorTakeOver);
  const findAssignmentByBettypeAndType=(dataSource, bettype, assignmentType)=> dataSource.find(a=>a.bettype===bettype && a.assignmentType===assignmentType);
 
  const [hdp,hdpOther, ou, ouOther]= [
    findAssignmentByBettypeAndType(assignee, BETTYPE.HDP, ASSIGNMENT_TYPE.MAIN),
    findAssignmentByBettypeAndType(assignee, BETTYPE.HDP, ASSIGNMENT_TYPE.OTHER),
    findAssignmentByBettypeAndType(assignee, BETTYPE.OU, ASSIGNMENT_TYPE.MAIN),
    findAssignmentByBettypeAndType(assignee, BETTYPE.OU, ASSIGNMENT_TYPE.OTHER)];
    const [hdpTakeOver,hdpOtherTakeOver, ouTakeOver, ouOtherTakeOver]= [
      findAssignmentByBettypeAndType(assigneeTakeOver, BETTYPE.HDP, ASSIGNMENT_TYPE.MAIN),
      findAssignmentByBettypeAndType(assigneeTakeOver, BETTYPE.HDP, ASSIGNMENT_TYPE.OTHER),
      findAssignmentByBettypeAndType(assigneeTakeOver, BETTYPE.OU, ASSIGNMENT_TYPE.MAIN),
      findAssignmentByBettypeAndType(assigneeTakeOver, BETTYPE.OU, ASSIGNMENT_TYPE.OTHER)];
  
       
  const handleChangeAssignment=({userName, userId, assignmentType, bettype})=>{
    
    dispatch(updateMajorPairs({
    userName,
    userId,
    assignmentType,
    bettype
    }));
  };

  return (
    <div className='major__pair--container wrapper'>
        <div className='major__pair'>
          <label className='title'>Major Pair</label>
          <Assignment
           key={uniqueId()}
            assignmentHdp={<>
            <InputAssignment
           
            onChange={handleChangeAssignment} 
            userId={hdp.userId} 
            userName={hdp.userName}
            assignmentType={ASSIGNMENT_TYPE.MAIN}
            bettype={BETTYPE.HDP}/>
            {hdpOther && <InputAssignment
             onChange={handleChangeAssignment}
              userId={hdpOther.userId} 
              userName={hdpOther.userName}
                assignmentType={ASSIGNMENT_TYPE.OTHER}
            bettype={BETTYPE.HDP}/>}
            </>}
            assignmentOu={<>
            <InputAssignment
            onChange={handleChangeAssignment}
             userId={ou.userId}
              userName={ou.userName}
               assignmentType={ASSIGNMENT_TYPE.MAIN}
            bettype={BETTYPE.OU}/>
            {ouOther && <InputAssignment
             onChange={handleChangeAssignment}
              userId={ouOther.userId}
               userName={ouOther.userName}
               assignmentType={ASSIGNMENT_TYPE.OTHER}
            bettype={BETTYPE.OU}/>}
            </>
          }/>
        </div>
          <div className='major__pair--take-over'>
          <label className='title'>Major Pair (Take Over)</label>
          <Assignment
            assignmentHdp={<>
            <InputAssignment onChange={handleChangeAssignment} userId={hdpTakeOver.userId} userName={hdpTakeOver.userName}/>
            {hdpOtherTakeOver && <InputAssignment onChange={handleChangeAssignment} userId={hdpOtherTakeOver.userId} userName={hdpOtherTakeOver.userName}/>}
            </>}
            assignmentOu={<>
            <InputAssignment  onChange={handleChangeAssignment} userId={ouTakeOver.userId} userName={ouTakeOver.userName}/>
            {ouOtherTakeOver && <InputAssignment onChange={handleChangeAssignment} userId={ouOtherTakeOver.userId} userName={ouOtherTakeOver.userName}/>}
            </>
          }/>
          </div>
        </div>
      );
};

MajorPair.propTypes = {
  
};

export default MajorPair;