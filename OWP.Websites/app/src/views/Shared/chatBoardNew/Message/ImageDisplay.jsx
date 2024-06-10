import { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { setImagesLoaded } from '../chatBoardSlice';
import useStableDispatch from '../../customHook/useStableDispatch';
import { getUrlTicketFile } from '../helper/chatBoardHelper';
const propTypes = {
  imageFileName: PropTypes.string.isRequired
};

const ImageDisplay = ({ imageFileName }) => {
  const [imageUrl] = useState(getUrlTicketFile(imageFileName));
  const [imageDimensions, setImageDimensions] = useState({
    width: 1,
    height: 1
  });
  const dispatch = useStableDispatch();

  useEffect(() => {
    const image = new Image();
    image.src = imageUrl;
    image.onload = () => {
      setImageDimensions({ width: image.width, height: image.height });
    };
  }, [imageUrl]);

  const ratio = (() => {
    const { width, height } = imageDimensions;
    const maxSize = 300;
    if (width < maxSize && height < maxSize) {
      return 1;
    }
    return width > height ? width / maxSize : height / maxSize;
  })();

  const handleImageLoad = () => {
    dispatch(setImagesLoaded());
  };
  return (
    <>
      {
        <img
          onLoad={handleImageLoad}
          className='image-responsive'
          width={imageDimensions.width / ratio}
          height={imageDimensions.height / ratio}
          src={imageUrl}
        />
      }
    </>
  );
};

ImageDisplay.propTypes = propTypes;

export default ImageDisplay;
