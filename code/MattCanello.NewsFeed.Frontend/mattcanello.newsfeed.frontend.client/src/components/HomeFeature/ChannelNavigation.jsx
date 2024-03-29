import React from 'react';
import '../../style/ChannelNavigation.css';

function ChannelNavigation() {
  return (
      <nav>
          <ol>
              <li class="selected"><a href="#">Tudo</a></li>
              <li><a href="#">The Guardian</a></li>
              <li><a href="#">G1</a></li>
              <li><a href="#">Folha de S. Paulo</a></li>
              <li><a href="#">Exame</a></li>
              <li><a href="#">GZH</a></li>
              <li><a href="#">The Verge</a></li>
              <li><a href="#">TecMundo</a></li>
              <li><a href="#">TechCrunch</a></li>
          </ol>
      </nav>
  );
}

export default ChannelNavigation;