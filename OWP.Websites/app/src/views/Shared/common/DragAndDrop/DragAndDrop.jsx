import { useState } from 'react';
import PropTypes from 'prop-types';

const propTypes = {
  children: PropTypes.node,
  onDrop: PropTypes.func.isRequired,
  className: PropTypes.string
};

const DragAndDrop = props => {
  const [dragging, setDragging] = useState(false);

  const handleDragenter = e => {
    e.preventDefault();
    setDragging(true);
  };

  const handleDragLeave = e => {
    e.preventDefault();
    setDragging(false);
  };

  const handleDrop = e => {
    e.preventDefault();
    props.onDrop(e, e.dataTransfer.files[0]);
    setDragging(false);
  };

  return (
    <div
      className={`drop-zone${dragging ? '-dragging' : ''} ${props.className}`}
      onDragLeave={handleDragLeave}
      onDragOver={handleDragenter}
      onDrop={handleDrop}>
      {props.children}
    </div>
  );
};

DragAndDrop.propTypes = propTypes;
export default DragAndDrop;
