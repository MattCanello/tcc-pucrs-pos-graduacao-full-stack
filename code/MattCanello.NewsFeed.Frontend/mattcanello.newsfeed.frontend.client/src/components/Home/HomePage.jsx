import React, { useEffect, useState } from 'react';
import ArticleList from '../Article/ArticleList';
import NewArticlesIndicator from '../SignalR/NewArticlesIndicator';
import { getHomePageArticles, searchArticles } from '../../functions/Home';
import { useLoaderData } from "react-router-dom";

function HomePage() {
    const { q } = useLoaderData();

    const [isLoading, setIsLoading] = useState(true);
    const [articles, setArticles] = useState([]);

    useEffect(() => {
        populateArticles();
    }, [q]);

    return (
        <>
            <NewArticlesIndicator />

            <ArticleList articles={articles} query={q} isLoading={isLoading} />
        </>
    );

    async function populateArticles() {
        setIsLoading(true);
        setArticles([]);

        const data = (q)
            ? await searchArticles(q)
            : await getHomePageArticles();

        setArticles(data);
        setIsLoading(false);
    }
}

export default HomePage;