import React from 'react';
import '../../style/Thumbnail.css';
import ChannelNameAndPublishDate from './ChannelNameAndPublishDate';

function Thumbnail() {
  return (
      <figure>
          <img title="'They're so fluffy!' (From left) Rodney, Roy and Raymond."
              src="https://i.guim.co.uk/img/media/4b91e61aa1e89de5f2128166a6d837d4e07ac470/0_252_1440_864/master/1440.jpg?width=1300&dpr=2&s=none" />
          <figcaption>'They're so fluffy!' (From left) Rodney, Roy and Raymond.</figcaption>
          <ChannelNameAndPublishDate />
      </figure>
  );
}

export default Thumbnail;