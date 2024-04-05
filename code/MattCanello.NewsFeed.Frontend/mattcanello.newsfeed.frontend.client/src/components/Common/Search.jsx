import React from 'react';
import '../../style/Search.css';
import { Form } from "react-router-dom";
import { useLoaderData } from "react-router-dom";

function Search() {
    const { q } = useLoaderData();

    function onFormSubmit() {
        document.getElementById("q").blur();
    }

    return (
        <search>
            <Form role="search" onSubmit={onFormSubmit}>
                <label htmlFor="searchInput" className="material-symbols-outlined">search</label>
                <input type="search" placeholder="Busca..." name="q" id="q" autoComplete="off" defaultValue={q} />

                <label htmlFor="searchInput"></label>
            </Form>
        </search>
    );
}

export default Search;