import { useState } from 'react';
import { TTreeDataSource, TNode } from './type';

const TreeNode = (node: TNode) => {
  const [isOpen, setIsOpen] = useState(false);
  const toggleNode = () => setIsOpen(!isOpen);
  const {childNode, label}= node;
  const leafNodeClassName= childNode
  ? 'parent__node--label'
  : 'leaf__node--label';
  return (
    <div className='tree-node'>
      {childNode && (
        <button onClick={toggleNode} className='toggle-icon'>
          {isOpen ? '-' : '+'}
        </button>
      )}
      <span onClick={toggleNode} className={leafNodeClassName}>{label}</span>
      {isOpen && childNode && <TreeView  dataSource={childNode} />}
    </div>
  );
};

const TreeView = (props: TTreeDataSource) =>
    <div className='tree-view'>
      {props.dataSource?.map(node => (
        <TreeNode key={node.id} id={node.id} childNode={node.childNode} label={node.label}/>
      ))}
    </div>;


export default TreeView;