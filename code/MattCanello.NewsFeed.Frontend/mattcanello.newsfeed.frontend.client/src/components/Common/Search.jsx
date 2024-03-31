import React from 'react';
import '../../style/Search.css';

function Search() {
    return (
        <search>
            <form onSubmit={onSubmit}>
                <label htmlFor="searchInput" className="material-symbols-outlined">search</label>
                <input type="search" placeholder="Busca..." name="searchInput" id="searchInput" />

                <label htmlFor="searchInput"></label>
            </form>
        </search>
    );

    function onSubmit() {
        return false;
    }
}

export default Search;