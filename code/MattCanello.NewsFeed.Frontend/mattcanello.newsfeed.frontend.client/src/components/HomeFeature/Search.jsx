import React from 'react';
import '../../style/Search.css';

function Search() {
  return (
      <search>
          <form onsubmit="return false;">
              <label for="searchInput" class="material-symbols-outlined">search</label>
              <input type="search" placeholder="Busca..." name="searchInput" id="searchInput" />

              <label for="searchInput"></label>
          </form>
      </search>
  );
}

export default Search;