import React from 'react';
import { useLoaderData } from "react-router-dom";
import ArticleList from '../Article/ArticleList';
import NewArticlesIndicatorComponent from '../SignalR/NewArticlesIndicatorComponent';

function HomePage() {
    const articles = useLoaderData();

    return (
        <>
            <NewArticlesIndicatorComponent />

            <ArticleList articles={articles} />
        </>
    );
}

export default HomePage;