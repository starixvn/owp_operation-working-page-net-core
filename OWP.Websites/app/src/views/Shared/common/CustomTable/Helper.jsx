const calculateCustomTableWidth = (columnSchema, initialWidth) => {
  let sumWidth = initialWidth || 0;
  columnSchema.forEach(item => {
    sumWidth += item.width;
  });
  return sumWidth;
};

const calculateTableDimension = (widthInfo, heightInfo) => {
  const { containerWidth, width } = widthInfo;

  const { containerHeight, defaultHeight, filterOffset } = heightInfo;

  const actualWidth = containerWidth > width ? width : containerWidth;
  let actualHeight;
  if (filterOffset) {
    actualHeight = window.innerHeight - filterOffset;
  } else {
    actualHeight =
      containerHeight > defaultHeight ? defaultHeight : containerHeight;
  }

  return {
    actualWidth,
    actualHeight
  };
};

export { calculateCustomTableWidth, calculateTableDimension };
