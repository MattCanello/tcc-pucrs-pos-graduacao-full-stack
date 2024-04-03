import React from 'react';
import '../../style/Header.css';
import Search from './Search';
import AppName from './AppName';
import ChannelNavigation from './ChannelNavigation';

function Header() {
  return (
      <header>
          <AppName />

          <Search />

          <ChannelNavigation />
      </header>
  );
}

export default Header;