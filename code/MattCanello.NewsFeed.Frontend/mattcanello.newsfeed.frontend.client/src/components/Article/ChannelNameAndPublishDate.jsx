import React from 'react';
import '../../style/ChannelNameAndPublishDate.css';
import Time from './Time';

function ChannelNameAndPublishDate({ channelName, publishDate }) {
    return (
        <small>{channelName}, <Time dateTimeString={publishDate} /></small>
  );
}

export default ChannelNameAndPublishDate;