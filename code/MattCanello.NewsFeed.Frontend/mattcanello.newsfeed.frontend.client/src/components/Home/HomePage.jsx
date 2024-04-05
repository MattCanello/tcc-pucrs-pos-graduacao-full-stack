import React from 'react';
import { useLoaderData } from "react-router-dom";
import ArticleList from '../Article/ArticleList';
import NewArticlesIndicator from '../SignalR/NewArticlesIndicator';

function HomePage() {
    const { articles, q } = useLoaderData();

    return (
        <>
            <NewArticlesIndicator />

            <ArticleList articles={articles} query={q} />
        </>
    );
}

export default HomePage;