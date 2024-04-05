import React from 'react';
import '../../style/ChannelNameAndPublishDate.css';
import Time from './Time';

function ChannelNameAndPublishDate({ channelName, publishDate, useAbsoluteTime }) {
    return (
        <small>{channelName}, <Time dateTimeString={publishDate} useAbsoluteTime={useAbsoluteTime} /></small>
  );
}

export default ChannelNameAndPublishDate;