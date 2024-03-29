import React from 'react';
import '../../style/Header.css';
import Search from './Search';
import ChannelNavigation from './ChannelNavigation';

function Header() {
  return (
      <header>
          <h1>
              <a href="index.html">News Feed</a>
          </h1>

          <Search />

          <ChannelNavigation />
      </header>
  );
}

export default Header;