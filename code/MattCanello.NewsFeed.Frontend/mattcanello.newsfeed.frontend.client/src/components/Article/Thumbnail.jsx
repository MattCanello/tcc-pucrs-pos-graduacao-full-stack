import React from 'react';
import '../../style/Thumbnail.css';

function Thumbnail({ imageTitle, imageSrc }) {

    const imgContent = (imageSrc)
        ? <img title={imageTitle} src={imageSrc} />
        : null;

    const figCaptionContent = (imageSrc)
        ? <figcaption>{imageTitle}</figcaption>
        : null;

  return (
      <figure>
          {imgContent}
          {figCaptionContent}
      </figure>
  );
}

export default Thumbnail;