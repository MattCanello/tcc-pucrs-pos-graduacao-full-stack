import React from 'react';
import '../../style/Header.css';
import Search from './Search';
import AppName from './AppName';
import ChannelNavigation from './ChannelNavigation';

function Header({ displayChannels }) {
  return (
      <header>
          <AppName />

          <Search />

          {displayChannels ? <ChannelNavigation /> : null}
      </header>
  );
}

export default Header;