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

    useEffect(() => {
        function onDequeueArticles(e) {
            if (e === undefined || e === null) {
                return;
            }

            if (e.detail === undefined || e.detail === null) {
                return;
            }

            if (e.detail.articles === undefined || e.detail.articles === null) {
                return;
            }

            const newArticles = [];

            for (let a of e.detail.articles) {
                if (newArticles.find(item => item.id == a.id))
                    continue;

                newArticles.push(a);
            }

            for (let a of articles) {
                if (newArticles.find(item => item.id == a.id))
                    continue;

                newArticles.push(a);
            }

            newArticles.sort(function (a, b) {
                return new Date(b.publishDate) - new Date(a.publishDate);
            });

            setArticles(newArticles);
        }

        document.addEventListener(
            "dequeueArticles",
            onDequeueArticles,
            false
        );

        return () => {
            document.removeEventListener("dequeueArticles", onDequeueArticles);
        };
    }, [articles]);

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