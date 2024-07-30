import store from './app/storeNative';
import { Provider } from 'react-redux';
import ProductPreferenceWrapper from './ProductPreferenceWrapper';

const ProductPreference = () => {
  return (
    <Provider store={store}>
      <ProductPreferenceWrapper />
    </Provider>
  );
};

export default ProductPreference;
