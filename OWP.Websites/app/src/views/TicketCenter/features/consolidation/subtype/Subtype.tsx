import { useEffect } from 'react';
import TreeView from '../../../../Shared/treeView/TreeView';
import { useAppDispatch, useAppSelector } from '../../../app/hooksNative';
import { isEqual } from 'lodash';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import Loading from '../../../../components/Loading';
import { querySubtypeThunk } from './subtypeThunk';

const Subtype = () => {
  const dispatch= useAppDispatch();
  const {loading, entities}= useAppSelector(state=>state.subtype);
  useEffect(()=>{
    dispatch(querySubtypeThunk());
  }, [dispatch]);

  if(isEqual(loading, REQUEST_STATUS.pending)){
    return <Loading/>;
  }
  return (
    <TreeView dataSource={entities}/>
  );
};

export default Subtype;

const data= [
  {
    id: "1",
    label: "NBA",
    childNode: [
      {
        id: "2",
        label: "Teams",
      }]},
    {
        id: "8",
        label: "Top Players",
        childNode: [
          {
            id: "9",
            label: "LeBron James",
          }
        ]
      },
      {
        id: "14",
        label: "Awards",
        childNode: [
          {
            id: "15",
            label: "Most Valuable Player",
          }
        ]
      }
];