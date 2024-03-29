import React from 'react';
import '../../style/Thumbnail.css';
import ChannelNameAndPublishDate from './ChannelNameAndPublishDate';

function Thumbnail({ channelName, publishDate, imageTitle, imageSrc }) {
  return (
      <figure>
          <img title={imageTitle} src={imageSrc} />
          <figcaption>{imageTitle}</figcaption>
          <ChannelNameAndPublishDate channelName={channelName} publishDate={publishDate} />
      </figure>
  );
}

export default Thumbnail;