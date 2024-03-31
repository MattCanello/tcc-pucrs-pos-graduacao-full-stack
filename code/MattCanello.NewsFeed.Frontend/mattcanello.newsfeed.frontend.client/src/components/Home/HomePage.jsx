import React from 'react';
import { useLoaderData } from "react-router-dom";
import ArticleList from '../Article/ArticleList';

function HomePage() {
    const articles = useLoaderData();

    return (
        <ArticleList articles={articles} />
    );
}

export default HomePage;