import { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import CustomTable from '../../../../Shared/common/CustomTable/CustomTable';
import { getConfigTable } from './configTable';

const ProductPreferenceTable = () => {
  const model = useSelector(state => state.productAssignment);
  const [tableSchema, setTableSchema] = useState(
    getConfigTable(window.innerWidth)
  );

  const handleResizeBrowser = () => {
    const newConfig = getConfigTable(window.innerWidth);
    setTableSchema(newConfig);
  };

  useEffect(() => {
    window.addEventListener('resize', handleResizeBrowser);
    return () => window.removeEventListener('resize', handleResizeBrowser);
  }, []);

  return (
    <CustomTable
      model={model}
      config={tableSchema}
      containerWidth={window.innerWidth - 80}
    />
  );
};

export default ProductPreferenceTable;
