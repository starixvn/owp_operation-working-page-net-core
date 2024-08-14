import TreeView from '../../../Shared/treeView/TreeView';

const Subtype = () => {
  return (
    <TreeView dataSource={data}/>
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