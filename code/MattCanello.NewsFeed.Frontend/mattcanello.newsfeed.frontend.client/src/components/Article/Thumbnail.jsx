import React from 'react';
import '../../style/Thumbnail.css';
import ChannelNameAndPublishDate from './ChannelNameAndPublishDate';

function Thumbnail({ channelName, publishDate, imageTitle, imageSrc }) {

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
          <ChannelNameAndPublishDate channelName={channelName} publishDate={publishDate} />
      </figure>
  );
}

export default Thumbnail;